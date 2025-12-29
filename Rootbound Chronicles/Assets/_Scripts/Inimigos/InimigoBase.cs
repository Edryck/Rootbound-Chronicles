using UnityEngine;

public class InimigoBase : MonoBehaviour
{
    [Header("Status do Inimigo")]
    public string nomeInimigo;
    public int vidaMaxima = 10;
    public float velocidade = 2f;
    public float raioDeVisao = 5f; // Distância para começar a seguir
    public float raioDeAtaque = 2f; // Distância para bater
    public float taxaDeAtaque = 1f; // Ataques por segundo
    public float proximoAtaque = 0f;

    [Header("Patrulha")]
    public float raioDePatrulha = 3f; // O quão longe ele anda do ponto inicial
    private Vector2 pontoInicial;
    private Vector2 destinoPatrulha;
    private float tempoEspera;
    public float tempoEntrePassos = 2f;

    [Header("Debug")]
    public int vidaAtual;
    private Rigidbody2D rb;
    protected Transform alvo;

    // Estados simples para IA
    protected enum Estado {
        Ocioso,
        Perseguindo, 
        Atacando
    }
    protected Estado estadoAtual = Estado.Ocioso;

    // Start é virtual para que os filhos possam adicionar coisas depois
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        vidaAtual = vidaMaxima;
        // Acha o jogador de forma automatica se estiver a tag Player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            alvo = playerObj.transform;
        }
        pontoInicial = transform.position;
        DefinirNovoDestinoPatrulha();
    }
    
    protected virtual void Update()
    {
        if (alvo == null) return;

        float distancia = Vector2.Distance(transform.position, alvo.position);
        // Máquina de estados de forma simplificada
        if (distancia > raioDeVisao)
        {
            estadoAtual = Estado.Ocioso;

            // Comportamento de patrulha
            // Anda mais devagar na patrulha
            Vector2 novaPosPatrulha = Vector2.MoveTowards(rb.position, destinoPatrulha, velocidade * 0.5f * Time.deltaTime);
            rb.MovePosition(novaPosPatrulha);

            // Se chegou no destino
            if (Vector2.Distance(transform.position, destinoPatrulha) < 0.2f)
            {
                if (tempoEspera <= 0)
                {
                    DefinirNovoDestinoPatrulha();
                    tempoEspera = tempoEntrePassos;
                }
                else
                {
                    tempoEspera -= Time.deltaTime;
                }
            }
        }
        else if (distancia > raioDeAtaque)
        {
            estadoAtual = Estado.Perseguindo;
            MoverParaPlayer();
        }
        else
        {
            estadoAtual = Estado.Atacando;
            TentarAtacar();
        }
    }

    void DefinirNovoDestinoPatrulha()
    {
        destinoPatrulha = pontoInicial + (Random.insideUnitCircle * raioDePatrulha);
    }

    // Lógica de movimento padrão
    protected virtual void MoverParaPlayer()
    {
        Vector2 novaPosicao = Vector2.MoveTowards(rb.position, alvo.position, velocidade * Time.fixedDeltaTime);
        rb.MovePosition(novaPosicao);
        // Adicionar a lógica para a troca de sprites de movimento aqui
    }

    protected virtual void TentarAtacar()
    {
        // Aqui vai ser implementado no filho
    }

    public void ReceberDano(int dano)
    {
        vidaAtual -= dano;
        Debug.Log(nomeInimigo + " tomou " + dano + "de dano!");

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    protected virtual void Morrer()
    {
        Debug.Log(nomeInimigo + " morreu!");
        Destroy(gameObject);
        // Colocar aqui a lógica de drop de item e a animação de morte
    }

    // Para o inimigo sentir o ambiente, ex: ele leva dano na lava agora
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Lava") // Caso tenha "dano ambiental", "area venenosa", etc. Adicionar um if/else
        {
            ReceberDano(1); // O inimigo vai recceber dano, futuramente vou trabalhar o sistema de resistencias, etc.
            Debug.Log(nomeInimigo + " caiu na lava!");
        }
    }

    // Desenha círculos no editor para ver a visão do inimigo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioDeVisao);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioDeAtaque);
    }
}
