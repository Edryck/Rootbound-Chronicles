using UnityEngine;

public class InimigoBase : MonoBehaviour
{
    [Header("Status do Inimigo")]
    public string nomeInimigo;
    public int vidaMaxima = 10;
    public float velocidade = 3f;
    public float raioDeVisao = 5f; // Distância para começar a seguir
    public float raioDeAtaque = 1.5f; // Distância para bater
    public float taxaDeAtaque = 1f; // Ataques por segundo
    protected float proximoAtaque = 0f;

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
    protected Animator animator;
    private Vector2 ultimaPosicao;

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
        animator = GetComponent<Animator>();

        vidaAtual = vidaMaxima;
        // Acha o jogador de forma automatica se estiver a tag Player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            alvo = playerObj.transform;
        }
        pontoInicial = transform.position;
        ultimaPosicao = transform.position;
        DefinirNovoDestinoPatrulha();
    }
    
    protected virtual void Update()
    {
        if (vidaAtual <= 0) return;

        bool estaSeMovendo = Vector2.Distance(transform.position, ultimaPosicao) > 0.001f;
        if (animator != null)
        {
            animator.SetBool("IsMoving", estaSeMovendo);
        }
        ultimaPosicao = transform.position; // Atualiza para o próximo frame

        if (alvo == null) return;

        float distancia = Vector2.Distance(transform.position, alvo.position);
        // Máquina de estados de forma simplificada
        if (distancia > raioDeVisao)
        {
            estadoAtual = Estado.Ocioso;
            Patrulhar();
        }
        else if (distancia > raioDeAtaque)
        {
            estadoAtual = Estado.Perseguindo;
            MoverParaPlayer();
        }
        else
        {
            estadoAtual = Estado.Atacando;
            if (Time.time >= proximoAtaque)
            {
                TentarAtacar();
                proximoAtaque = Time.time + 1f / taxaDeAtaque;
            }
        }
    }

    void Patrulhar()
    {
        Vector2 novaPosPatrulha = Vector2.MoveTowards(rb.position, destinoPatrulha, velocidade * 0.5f * Time.deltaTime);
        rb.MovePosition(novaPosPatrulha);

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

    void DefinirNovoDestinoPatrulha()
    {
        destinoPatrulha = pontoInicial + (Random.insideUnitCircle * raioDePatrulha);
    }

    // Lógica de movimento padrão
    protected virtual void MoverParaPlayer()
    {
        Vector2 novaPosicao = Vector2.MoveTowards(rb.position, alvo.position, velocidade * Time.deltaTime);
        rb.MovePosition(novaPosicao);
        
        // Vira o Sprite (Flip)
        if (alvo.position.x > transform.position.x) transform.localScale = new Vector3(1, 1, 1);
        else transform.localScale = new Vector3(-1, 1, 1);
    }

    protected virtual void TentarAtacar()
    {
        // Aqui vai ser implementado no filho
    }

    public void ReceberDano(int dano, TipoDano tipoDano)
    {
        vidaAtual -= dano;
        Debug.Log(nomeInimigo + " tomou " + dano + " de dano!");

        // Para colocar a animação de quando ele leva dano
        if(animator != null) animator.SetTrigger("Hunt"); 

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    protected virtual void Morrer()
    {
        Debug.Log(nomeInimigo + " morreu!");

        if (animator != null)
        {
            animator.SetTrigger("Morrer");
        }
        // Impede que o corpo morto continue perseguindo ou dando dano
        rb.simulated = false;
        GetComponent<Collider2D>().enabled = false; // Jogador anda por cima do corpo
        this.enabled = false; // Desliga o Update()

        // O 1f é o tempo em segundos. Caso seja necessário mudar... mude (Conforme a duração da animação)
        Destroy(gameObject, 1f);
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
