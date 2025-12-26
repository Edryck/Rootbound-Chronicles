using UnityEngine;

public class ObjetoInterativo : MonoBehaviour
{
    private bool jogadorPerto = false;

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
            Destroy(gameObject);
            Debug.Log("Você pegou o item: " + gameObject.name);
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
            // Aqui que vai desligar o balãozinho de interação
        }
    }
}