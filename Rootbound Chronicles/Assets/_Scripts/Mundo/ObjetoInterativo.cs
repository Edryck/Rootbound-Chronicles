using UnityEngine;

public class ObjetoInterativo : MonoBehaviour
{
    private bool jogadorPerto = false;
    private GameObject jogadorRef;

    [Header("Tipo de Interação")]
    public bool ehItemColetavel;
    public bool ehCheckpoint;
    public bool ehLore;

    [Header("Dados (Preencha o que for usar)")]
    public ItemData itemParaDar; // Se for item
    public CodexEntry paginaDoCodex; // Se for Lore/Inimigo/Estátua

    [Header("Configuração de Inspeção")]
    // Se true, só inspeciona. Se false, interage (pega/salva)
    public bool apenasInspecao = false; 

    void Update()
    {
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            Interagir();
        }
    }

    void Interagir()
    {
        // Lógica de Item (Coleta)
        if (ehItemColetavel && itemParaDar != null)
        {
            Inventario inv = jogadorRef.GetComponent<Inventario>();
            if (inv != null)
            {
                if(inv.AdicionarItem(itemParaDar, 1))
                {
                    // Se o item tiver uma página de lore vinculada, desbloqueia!
                    if (paginaDoCodex != null) CodexManager.instance.DesbloquearEntrada(paginaDoCodex);
                    Destroy(gameObject);
                }
            }
            return; // Para por aqui se for item
        }

        // Lógica de Checkpoint
        if (ehCheckpoint)
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.SalvarCheckpoint(GameManager.instance.player.transform.position);
                SistemaDialogo.instance.MostrarTexto("Vínculo neural estabelecido. (Jogo Salvo)");
            }
        }

        // Lógica de Lore / Inspeção (O Códice)
        if (ehLore || paginaDoCodex != null)
        {
            if (CodexManager.instance != null && paginaDoCodex != null)
            {
                // Desbloqueia no menu
                CodexManager.instance.DesbloquearEntrada(paginaDoCodex);

                // Mostra o texto na tela imediatamente (Feedback)
                // Se já tiver desbloqueado antes, mostra a lore oculta se tiver
                string textoParaMostrar = paginaDoCodex.loreOculta;
                
                if (paginaDoCodex.foiDescoberto && paginaDoCodex.loreRevelada)
                {
                    // TODO: Aqui entraria a checagem de "Nível de Percepção" do jogador no futuro
                    // Por enquanto, vamos liberar se inspecionar pela segunda vez
                    CodexManager.instance.RevelarLoreOculta(paginaDoCodex);
                    textoParaMostrar += "\n\n<color=red>[SINTONIA PROFUNDA]:</color> " + paginaDoCodex.loreOculta;
                }

                SistemaDialogo.instance.MostrarTexto(textoParaMostrar);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;
            jogadorRef = other.gameObject;
            // TODO: Mostrar alguma coisa pro jogador, para mostrar que dá para interagir
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
            jogadorRef = null;
        }
    }
}