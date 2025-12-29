using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Classes existentes
    public enum Classe {
        Guerreio,
        Mago, 
        Arqueiro,
        Ladino,
        Clérigo
    }

    [Header("Configuração da Classe")]
    public Classe classeAtual;

    [Header("Status Vitais")]
    public int vidaMaxima = 10;
    public int vidaAtual;
    // Futuramente public int mana;
    // Futuramento public int stamina;

    void Start()
    {
        vidaAtual = vidaMaxima;
    }

    public void ReceberDano(int danoRecebido)
    {
        vidaAtual -= danoRecebido;
        Debug.Log("Vida do Jogador: " + vidaAtual);

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    public void Curar(int quantidade)
    {
        vidaAtual += quantidade;
        if (vidaAtual > vidaMaxima) vidaAtual = vidaMaxima;
        Debug.Log("Player curado: " + vidaAtual);
    }

    protected virtual void Morrer()
    {
        Debug.Log("Morreu!"); // TODO: Depois tem que colocar alguma coisa pra puxar o motivo da morte
        Destroy(gameObject); // TODO: Já avisando que vai dar merda isso, vai bugar a câmera
        // Aqui pode dropar loot
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Lava")
        {
            ReceberDano(2); 
        }

        // Zona de Cura
        if (other.tag == "Cura")
        {
            if (vidaAtual == vidaMaxima)
            {
                Debug.Log("Vida cheia!");
            }
            else
            {
                Curar(2);
            }
        }

        // Poção fodástica
        if (other.tag == "Pocao")
        {
            if (vidaAtual < vidaMaxima)
            {
                Curar(vidaMaxima); // Cura tudo
                Debug.Log("Vida máxima restaurada!");
                Destroy(other.gameObject);
            }
        }
    }
}
