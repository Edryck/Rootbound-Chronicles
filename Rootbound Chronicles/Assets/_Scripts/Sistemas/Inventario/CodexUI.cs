using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CodexUI : MonoBehaviour
{
    [Header("Grupos Principais (Arrastar da Hierarquia)")]
    public GameObject grupoLore;
    public GameObject grupoBestiario;
    public GameObject grupoInventario;
    public GameObject grupoPersonagem;

    [Header("UI Lore (Página 1)")]
    public Transform listaLoreContainer;
    public Image imgLoreEsq;
    public TextMeshProUGUI txtLoreTituloDir;
    public TextMeshProUGUI txtLoreDescDir;
    public TextMeshProUGUI txtLoreOcultaDir;

    [Header("UI Bestiário (Página 2)")]
    public Transform listaBestiarioContainer;
    public Image imgMonstroEsq;
    public TextMeshProUGUI txtMonstroTituloDir;
    public TextMeshProUGUI txtMonstroLoreDir;
    public TextMeshProUGUI txtMonstroFraquezasDir; // Novo

    [Header("UI Personagem (Página 4)")]
    public TextMeshProUGUI txtStatsDireita; // Vamos preencher isso com PlayerStats

    [Header("Prefabs")]
    public GameObject prefabBotaoLista; // Botãozinho com nome para as listas

    // Estado
    private CategoriaCodex abaAtual = CategoriaCodex.Lore;

    void Start()
    {
        // Começa fechado
        gameObject.SetActive(false);
        if (CodexManager.instance != null) 
            CodexManager.instance.onCodexUpdateCallback += AtualizarAbaAtual;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Tab))
        {
            AlternarJanela();
        }
    }

    public void AlternarJanela()
    {
        bool estado = !gameObject.activeSelf;
        gameObject.SetActive(estado);
        
        if (estado) MudarAba((int)abaAtual);
    }

    public void MudarAba(int index)
    {
        abaAtual = (CategoriaCodex)index;

        // Desliga tudo
        grupoLore.SetActive(false);
        grupoBestiario.SetActive(false);
        grupoInventario.SetActive(false);
        grupoPersonagem.SetActive(false);

        // Liga e Configura a Aba Certa
        switch (abaAtual)
        {
            case CategoriaCodex.Lore:
                grupoLore.SetActive(true);
                PreencherLista(listaLoreContainer, CategoriaCodex.Lore);
                LimparDireitaLore();
                break;

            case CategoriaCodex.Bestiario:
                grupoBestiario.SetActive(true);
                PreencherLista(listaBestiarioContainer, CategoriaCodex.Bestiario);
                LimparDireitaBestiario();
                break;

            case CategoriaCodex.Inventario:
                grupoInventario.SetActive(true);
                // O script InventoryUI no Grid cuida do resto
                break;

            case CategoriaCodex.Personagem:
                grupoPersonagem.SetActive(true);
                AtualizarStatsPersonagem();
                break;
        }
    }

    // Lógica Genérica de Listas
    void PreencherLista(Transform container, CategoriaCodex filtro)
    {
        // Limpa botões antigos
        foreach (Transform child in container) Destroy(child.gameObject);

        // Cria novos botões para itens descobertos
        foreach (var entry in CodexManager.instance.todasAsPaginas)
        {
            if (entry.categoria == filtro && entry.foiDescoberto)
            {
                GameObject btn = Instantiate(prefabBotaoLista, container);
                btn.GetComponentInChildren<TextMeshProUGUI>().text = entry.titulo;
                
                // Configura o clique
                btn.GetComponent<Button>().onClick.AddListener(() => {
                    if (filtro == CategoriaCodex.Lore) MostrarDetalhesLore(entry);
                    else if (filtro == CategoriaCodex.Bestiario) MostrarDetalhesBestiario(entry);
                });
            }
        }
    }

    // Exibição de Detalhes
    void MostrarDetalhesLore(CodexEntry entry)
    {
        imgLoreEsq.sprite = entry.icone;
        imgLoreEsq.enabled = (entry.icone != null);
        txtLoreTituloDir.text = entry.titulo;
        txtLoreDescDir.text = entry.descricaoBasica + "\n\n" + entry.historiaDetalhada;
        
        if (entry.loreRevelada && !string.IsNullOrEmpty(entry.loreOculta))
            txtLoreOcultaDir.text = "<color=purple>SINTONIA:</color> " + entry.loreOculta;
        else
            txtLoreOcultaDir.text = "";
    }

    void MostrarDetalhesBestiario(CodexEntry entry)
    {
        imgMonstroEsq.sprite = entry.icone;
        imgMonstroEsq.enabled = (entry.icone != null);
        txtMonstroTituloDir.text = entry.titulo;
        txtMonstroLoreDir.text = entry.descricaoBasica;

        // Se tiver info extra, mostra
        if (!string.IsNullOrEmpty(entry.fraquezas))
            txtMonstroFraquezasDir.text = $"Fraquezas: {entry.fraquezas}\nResistências: {entry.resistencias}";
        else
            txtMonstroFraquezasDir.text = "???";
    }

    void AtualizarStatsPersonagem()
    {
        if (PlayerStats.instance != null)
        {
            // Pega os dados direto do Singleton do PlayerStats
            string stats = $"Vida: {PlayerStats.instance.vidaAtual}/{PlayerStats.instance.vidaMaxima}\n";
            stats += $"Defesa Física: {PlayerStats.instance.defesaFisicaTotal}\n";
            stats += $"Resistência Mágica: {PlayerStats.instance.resistenciaMagicaTotal}\n";
            stats += $"Dano Base: {PlayerCombat.instance.danoBase}\n";
            stats += $"Dano Físico: {PlayerCombat.instance.danoFisicoTotal}\n";
            stats += $"Dano Mágico: {PlayerCombat.instance.danoMagicoTotal}\n";
            
            txtStatsDireita.text = stats;
        }
    }

    // Limpezas visuais
    void LimparDireitaLore() { txtLoreTituloDir.text = ""; txtLoreDescDir.text = "Selecione um registro..."; imgLoreEsq.enabled = false; }
    void LimparDireitaBestiario() { txtMonstroTituloDir.text = ""; txtMonstroLoreDir.text = "Selecione uma criatura..."; imgMonstroEsq.enabled = false; }

    void AtualizarAbaAtual() { if (gameObject.activeSelf) MudarAba((int)abaAtual); }
}