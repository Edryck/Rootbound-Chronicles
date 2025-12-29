using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Configurações de Ataque")]
    public Transform pontoDeAtaque; // Um objeto vazio na frente do jogador
    public float raioDeAtaque = 0.5f;
    public int danoAtaque = 2;
    public float taxaDeAtaque = 1f; // Ataques por segundo
    private float proximoAtaque = 0f;
    
    [Header("Quem eu posso bater?")]
    public LayerMask camadaInimigos; // Para não bater em paredes ou nele mesmo

    void Update()
    {
        // Se já passou o tempo de recarga e apertou o botão (Mouse esq)
        if (Time.time >= proximoAtaque && Input.GetButtonDown("Fire1"))
        {
            Atacar();
            proximoAtaque = Time.time + 1f / taxaDeAtaque;
        }
    }

    void Atacar()
    {
        // Verifica se tem inimigos na área e cria uma lista temporária de quem foi atingido
        Collider2D[] inimigosAtingidos = Physics2D.OverlapCircleAll(pontoDeAtaque.position, raioDeAtaque, camadaInimigos);

        // Dá dano em cada um deles
        foreach (Collider2D inimigo in inimigosAtingidos)
        {
            InimigoBase scriptInimigo = inimigo.GetComponent<InimigoBase>();
            if (scriptInimigo != null)
            {
                scriptInimigo.ReceberDano(danoAtaque);
                Debug.Log("Acertou o " + inimigo.name);
            }
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
