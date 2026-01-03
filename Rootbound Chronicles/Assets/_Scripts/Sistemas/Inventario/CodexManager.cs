using UnityEngine;
using System.Collections.Generic;

public class CodexManager : MonoBehaviour
{
    public static CodexManager instance;

    // Lista de tudo que existe no jogo (Arraste todas as páginas aqui no Inspector)
    public List<CodexEntry> todasAsPaginas; 

    // Evento para avisar a UI para atualizar
    public delegate void OnCodexUpdate();
    public OnCodexUpdate onCodexUpdateCallback;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void DesbloquearEntrada(CodexEntry pagina)
    {
        if (!pagina.foiDescoberto)
        {
            pagina.foiDescoberto = true;
            Debug.Log($"Códice Atualizado: {pagina.titulo} adicionado em {pagina.categoria}");

            // TODO: Toca som de "escrita orgânica" aqui
            // ex: SoundManager.instance.Play("CodexWrite");

            if (onCodexUpdateCallback != null) onCodexUpdateCallback.Invoke();
            
            // Mostra aviso na tela
            SistemaDialogo.instance.MostrarTexto($"Novo registro no Códice: {pagina.titulo}");
        }
    }

    public void RevelarLoreOculta(CodexEntry pagina)
    {
        if (pagina.foiDescoberto && !pagina.loreRevelada)
        {
            pagina.loreRevelada = true;
            Debug.Log($"Segredo revelado: {pagina.titulo}");
            
            if (onCodexUpdateCallback != null) onCodexUpdateCallback.Invoke();
        }
    }
}