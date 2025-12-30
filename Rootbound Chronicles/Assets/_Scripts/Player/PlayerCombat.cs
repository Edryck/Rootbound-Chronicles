using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Atributos de Combate")]
    public int danoBase = 2; 
    public int danoFisicoTotal;
    public int danoMagicoTotal;

    [Header("Configurações de Ataque")]
    public Transform pontoDeAtaque; // Um objeto vazio na frente do jogador
    public float raioDeAtaque = 0.75f;
    public float taxaDeAtaque = 1f; // Ataques por segundo
    private float proximoAtaque = 0f;
    
    [Header("Quem eu posso bater?")]
    public LayerMask camadaInimigos; // Para não bater em paredes ou nele mesmo

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>(); // Pega o Animator do Player
        RecalcularAtributosOfensivos();
    }
    
    void Update()
    {
        // Lê a direção que o jogador está tentando ir
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        // Se tiver movimento, atualiza a posição do ponto de ataque
        if (inputX != 0 || inputY != 0)
        {
            // Cria um vetor na direção do input
            Vector2 direcao = new Vector2(inputX, inputY).normalized;

            // Define a posição relativa ao centro do jogador
            pontoDeAtaque.localPosition = direcao * 0.8f;
        }

        // Se já passou o tempo de recarga e apertou o botão (Mouse esq)
        if (Time.time >= proximoAtaque && Input.GetButtonDown("Fire1"))
        {
            Atacar();
            proximoAtaque = Time.time + 1f / taxaDeAtaque;
        }
    }

    void Atacar()
    {
        // Toca a animação
        if (animator != null)
        {
            animator.SetTrigger("Atacar");
        }

        // Verifica se tem inimigos na área e cria uma lista temporária de quem foi atingido
        Collider2D[] inimigosAtingidos = Physics2D.OverlapCircleAll(pontoDeAtaque.position, raioDeAtaque, camadaInimigos);

        // Dá dano em cada um deles
        foreach (Collider2D inimigo in inimigosAtingidos)
        {
            InimigoBase scriptInimigo = inimigo.GetComponent<InimigoBase>();
            if (scriptInimigo != null)
            {
                // Aplica Dano Físico
                if (danoFisicoTotal > 0)
                    scriptInimigo.ReceberDano(danoFisicoTotal, TipoDano.Fisico);

                // Aplica Dano Mágico (se tiver espada mágica)
                if (danoMagicoTotal > 0)
                    scriptInimigo.ReceberDano(danoMagicoTotal, TipoDano.Magico);
                
                Debug.Log("Hit no: " + inimigo.name);
            }
        }
    }

    public void RecalcularAtributosOfensivos()
    {
        danoFisicoTotal = danoBase;
        danoMagicoTotal = 0;

        if (EquipmentManager.instance != null)
        {
            var bonus = EquipmentManager.instance.CalcularAtaqueTotal();
            danoFisicoTotal += bonus.danoFisico;
            danoMagicoTotal += bonus.danoMagico;
        }
    }

    // Desenha o círculo no editor para ajustar o tamanho
    void OnDrawGizmosSelected()
    {
        if (pontoDeAtaque == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pontoDeAtaque.position, raioDeAtaque);
    }
}
