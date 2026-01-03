using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Se futuramente adicionar mais efeito, adicione aqui no enumerado
public enum TipoEfeito { Nenhum, Queimadura, Veneno, Lentidao, CuraContinua }

public class GerenciadorDeStatus : MonoBehaviour
{
    [System.Serializable]
    public class EfeitoAtivo
    {
        public TipoEfeito tipo;
        public float duracaoRestante;
        public float intervaloDoTick; // Para quantos segundos é aplicado dano
        public float tempoParaProximoTick; // Contador interno
        public int potencia; // Dano ou % de lentidao
    }

    [Header("Estado Atual")]
    public List<EfeitoAtivo> efeitosAtivos = new List<EfeitoAtivo>();

    [Header("Visual FX")]
    public Animator animadorDeEfeitos;
    
    private Dictionary<TipoEfeito, string> animacoes = new Dictionary<TipoEfeito, string>()
    {
        { TipoEfeito.Queimadura, "Queimando" },
        { TipoEfeito.Veneno,     "Envenenado" },
        { TipoEfeito.Lentidao,   "Lento" },
        { TipoEfeito.Nenhum,     "Normal" } // Estado vazio
    };

    // Lista de Prioridade: "O que é mais importante mostrar?"
    // O topo da lista ganha do resto
    private List<TipoEfeito> prioridade = new List<TipoEfeito>()
    {
        TipoEfeito.CuraContinua,
        TipoEfeito.Queimadura,
        TipoEfeito.Veneno,
        TipoEfeito.Lentidao
    };

    // Referências aos componentes do dono
    private PlayerStats playerStats;
    private InimigoBase inimigoStats;
    private PlayerMovement playerMove;
    private SpriteRenderer[] spritesDoCorpo;
    private Color[] coresOriginais;

    // Controle de velocidade original para poder restaurar depois
    private float velocidadeOriginalPlayer;
    private float velocidadeOriginalInimigo;
    private bool estaComLentidao = false;

    void Start()
    {
        // Buscca referências de vida
        playerStats = GetComponent<PlayerStats>();
        inimigoStats = GetComponent<InimigoBase>();

        // Busca referências de Movimento para a lentidão
        playerMove = GetComponent<PlayerStats>() ? GetComponent<PlayerMovement>() : null;

        // Salva as velocidades originais
        if (playerMove != null) velocidadeOriginalPlayer = playerMove.moveSpeed;
        if (inimigoStats != null) velocidadeOriginalInimigo = inimigoStats.velocidade;

        // Configuração Visual (Paper Doll)
        spritesDoCorpo = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        // Percorre a lista de trás para frente
        for (int i = efeitosAtivos.Count - 1; i >= 0; i--)
        {
            EfeitoAtivo efeito = efeitosAtivos[i];

            // Diminui o tempo de vida de efeito
            efeito.duracaoRestante -= Time.deltaTime;

            // Processa o efeito (Ticks de Dano)
            if (efeito.duracaoRestante > 0)
            {
                ProcessarEfeito(efeito);
            }
            else
            {
                // O tempo acabou, remove o efeito
                RemoverEfeito(efeito);
                efeitosAtivos.RemoveAt(i);
            }
        }
    }

    // Função prinicipal, também a que tem que ser chamada
    public void AplicarEfeito(TipoEfeito tipo, int potencia, float duracao, float intervalo = 1f)
    {
        // Verifica se já tem esse efeito (para não acumular infinitamente)
        EfeitoAtivo efeitoExistente = efeitosAtivos.Find(x => x.tipo == tipo);

        if (efeitoExistente != null)
        {
            // Se já tem, só renova a duração
            efeitoExistente.duracaoRestante = duracao;
            // Se a potência nova ser maior
            if (potencia > efeitoExistente.potencia) efeitoExistente.potencia = potencia;
        }
        else
        {
            // Cria um efeito novo
            EfeitoAtivo novoEfeito = new EfeitoAtivo();
            novoEfeito.tipo = tipo;
            novoEfeito.potencia = potencia;
            novoEfeito.duracaoRestante = duracao;
            novoEfeito.intervaloDoTick = intervalo;
            novoEfeito.tempoParaProximoTick = 0; // Espera para aplicar o primeiro tick

            efeitosAtivos.Add(novoEfeito);

            // Aplica mudanças imediatas
            AoIniciarEfeito(novoEfeito);
        }
    }

    void AtualizarVisual()
    {
        if (animadorDeEfeitos == null) return;

        TipoEfeito efeitoParaMostrar = TipoEfeito.Nenhum;

        // O Loop Mágico:
        // Percorre a lista de prioridade. O primeiro que encontrar ativo, ele escolhe e PARA.
        foreach (TipoEfeito tipo in prioridade)
        {
            if (efeitosAtivos.Exists(x => x.tipo == tipo))
            {
                efeitoParaMostrar = tipo;
                break; // Achou o mais importante, para de procurar!
            }
        }

        // Busca o nome da string no dicionário e toca
        // Isso evita erros de digitação de string ("Queimando" vs "queimando")
        if (animacoes.ContainsKey(efeitoParaMostrar))
        {
            string nomeDaAnimacao = animacoes[efeitoParaMostrar];
            animadorDeEfeitos.Play(nomeDaAnimacao);
        }
    }

    void AoIniciarEfeito(EfeitoAtivo efeito)
    {
        switch (efeito.tipo)
        {
            case TipoEfeito.Queimadura:
                AtualizarVisual();
                break;
            case TipoEfeito.Veneno:
                AtualizarVisual();
                break;
            case TipoEfeito.Lentidao:
                AtualizarVisual();
                AplicarLentidao(efeito.potencia);
                break;
        }
    }

    void ProcessarEfeito(EfeitoAtivo efeito)
    {
        // Lógica de Tick (Dano por Segundo)
        // Só se aplica a Queimadura, Veneno ou Cura
        if (efeito.tipo == TipoEfeito.Queimadura || efeito.tipo == TipoEfeito.Veneno)
        {
            efeito.tempoParaProximoTick -= Time.deltaTime;

            if (efeito.tempoParaProximoTick <= 0)
            {
                // Aplica Dano
                AplicarDano(efeito.potencia, efeito.tipo == TipoEfeito.Queimadura ? TipoDano.Fisico : TipoDano.Magico);
                
                // Reseta o timer do tick
                efeito.tempoParaProximoTick = efeito.intervaloDoTick;
            }
        }
        if (efeito.tipo == TipoEfeito.CuraContinua)
        {
            efeito.tempoParaProximoTick -= Time.deltaTime;

            if (efeito.tempoParaProximoTick <= 0)
            {
                // Aplica Cura
                AplicarCura(efeito.potencia);
            
                // Reseta o timer do tick
                efeito.tempoParaProximoTick = efeito.intervaloDoTick;
            }
        }
    }

    void RemoverEfeito(EfeitoAtivo efeito)
    {
        // Reverte as mudanças
        switch (efeito.tipo)
        {
            case TipoEfeito.Lentidao:
                RemoverLentidao();
                break;
        }

        // Se não tiver mais nenhum efeito ativo, restaura a cor original
        if (efeitosAtivos.Count <= 1)
        {
            AtualizarVisual();
        }
    }

    void AplicarDano(int dano, TipoDano tipo)
    {
        if (playerStats != null) playerStats.ReceberDano(dano, tipo);
        if (inimigoStats != null) inimigoStats.ReceberDano(dano, tipo);
    }

    void AplicarCura(int cura)
    {
        // Só aplica cura para o jogador
        if (playerStats != null) playerStats.Curar(cura);
    }

    void AplicarLentidao(int porcentagem)
    {
        if (estaComLentidao) return; // Para não aplicar duas vezes

        float fator = 1f - (porcentagem / 100f); // Ex: 30% lentidão = 0.7x velocidade

        if (playerMove != null) playerMove.moveSpeed *= fator;
        if (inimigoStats != null) inimigoStats.velocidade *= fator;

        estaComLentidao = true;
    }

    void RemoverLentidao()
    {
        if (!estaComLentidao) return;

        // Restaura os valores originais exatos
        if (playerMove != null) playerMove.moveSpeed = velocidadeOriginalPlayer;
        if (inimigoStats != null) inimigoStats.velocidade = velocidadeOriginalInimigo;

        estaComLentidao = false;
    }

    // Detector de áreas que aplica efeito
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Lava") 
            AplicarEfeito(TipoEfeito.Queimadura, 1, 3f, 1f);    // Dano 1, 3 seg, tick de 1s
            
        if (other.tag == "Veneno")
            AplicarEfeito(TipoEfeito.Veneno, 2, 5f, 2f);        // Dano 2, 5 seg, tick de 2s

        if (other.tag == "Lama")
            AplicarEfeito(TipoEfeito.Lentidao, 50, 2f);         // 50% mais lento por 2s
        
        if (other.tag == "Cura" || other.tag == "Checkpoint")
            AplicarEfeito(TipoEfeito.CuraContinua, 5, 10f, 1f); // Cura 5, 10 seg, ticks de 1s
    }
}
