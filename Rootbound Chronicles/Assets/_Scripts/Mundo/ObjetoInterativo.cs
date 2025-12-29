using UnityEngine;

public class ObjetoInterativo : MonoBehaviour
{
    private bool jogadorPerto = false;
    private GameObject jogadorRef; // Referência para lembrar que é o jogador
    public ItemData itemParaDar;

    void Update()
    {
        if (jogadorPerto == true && Input.GetKeyDown(KeyCode.E))
        {
            Interagir();
        }
    }

    // Função que faz a ação acontecer
    void Interagir()
    {
        if (gameObject.tag == "Item")
        {
            // Acha o invetário do jogador
            Inventario inv = jogadorRef.GetComponent<Inventario>();

            if (inv != null && itemParaDar != null)
            {
                // Aqui passa o nome do objeto como nome do item
                inv.AdicionarItem(itemParaDar, 1);
                Destroy(gameObject);
            }
        }
        if (gameObject.tag == "NPC")
        {
            Debug.Log("Você interagiu com o NPC: " + gameObject.name);
        }
        if (gameObject.tag == "Bau")
        {
            Debug.Log("Você interagiu com: " + gameObject.name);
        }
        // Aqui vai uma lógica específica
        // Como dar item, abrir dialogo, salvar jogo
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            jogadorPerto = true;
            jogadorRef = other.gameObject;
            Debug.Log("Tecle E para interagir");
            // Aqui liga o batão de interagir
        }
    }

    // Para detectar quando o jogador sai de perto
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            jogadorPerto = false;
            jogadorRef = null;
            // Aqui que vai desligar o balãozinho de interação
        }
    }
}