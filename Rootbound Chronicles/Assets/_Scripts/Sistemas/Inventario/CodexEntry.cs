using UnityEngine;
using System.Collections.Generic;

public enum CategoriaCodex { Lore, Bestiario, Inventario, Personagem }

[CreateAssetMenu(fileName = "NovaPagina", menuName = "Sistema/Pagina do Codex")]
public class CodexEntry : ScriptableObject
{
    [Header("Geral")]
    public string id;
    public string titulo;
    public CategoriaCodex categoria;
    public Sprite icone; // Foto do Monstro/Item

    [Header("Textos")]
    [TextArea(3, 5)] public string descricaoBasica;
    [TextArea(3, 5)] public string historiaDetalhada; // Direita
    [TextArea(3, 5)] public string loreOculta; // Direita (Se tiver inspeção)

    [Header("Exclusivo Bestiário")]
    public string fraquezas; // Ex: "Fogo, Corte"
    public string resistencias; // Ex: "Veneno"
    
    [Header("Controle")]
    public bool foiDescoberto;
    public bool loreRevelada;
}