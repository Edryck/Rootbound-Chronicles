using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Para facilitar o acesso

    [Header("UI")]
    public GameObject telaGameOver;

    [Header("Referências")]
    public GameObject player;
    public PlayerStats playerStats;

    [Header("Estado do Jogo")]
    public bool jogoPausado = false;
    public Vector3 ultimoCheckpoint;

    void Awake()
    {
        if (instance == null) 
            instance = this;
        else 
            Destroy(gameObject);
    } 
   
    void Start()
    {
        // Define o checkpoint inicial como a posição onde o jogador começou o jogo
        if (player != null)
        {
            ultimoCheckpoint = player.transform.position;
        }
    }

    public void SalvarCheckpoint(Vector3 novaPosicao)
    {
        ultimoCheckpoint = novaPosicao;
        Debug.Log("Checkpoint salve em: " + novaPosicao);
        // TODO: Adicionar o aviso de "Progresso salvo"
    }

    public void GameOver()
    {
        if (jogoPausado) return;

        jogoPausado = true;
        Time.timeScale = 0f;
        telaGameOver.SetActive(true);
    }

    // Função do Renascer
    public void RenascerJogador()
    {
        Time.timeScale = 1f;
        jogoPausado = false;

        // Esconde a tela de game over
        telaGameOver.SetActive(false);

        // Teletransporta o jogador
        if (player != null)
        {
            player.transform.position = ultimoCheckpoint;

            // Reativa os controles que foram desligados no PlayerStats.Morrer()
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<Collider2D>().enabled = true;
            // Se desligar os sprites, tem que ativar aqui
        }

        // Cura totalmente o jogador
        if (playerStats != null)
        {
            playerStats.vidaAtual = playerStats.vidaMaxima;
            // Quando tiver mana, encher aqui
            Debug.Log("Jogador renascido!");
        }
    }
}
