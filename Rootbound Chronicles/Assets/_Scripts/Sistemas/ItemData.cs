using UnityEngine;

public enum TipoEquipamento
{
    Nenhum, // Para item normais (moeda, poção, madeira, etc)
    Cabeca, // Capacetes
    Peito,  // Peitorais
    Pernas, // Calças/Botas
    Arma,   // Espadas/Machados/Cajados
    Escudo  // Escudos
}

public enum TipoDano 
{ 
    Fisico,
    Magico
}

[CreateAssetMenu(fileName = "Novo Item", menuName = "Sistema/Item")]
public class ItemData : ScriptableObject
{
    [Header("Informções Básicas")]
    public string nomeDoItem;
    [TextArea(3, 10)] // Para criar uma caixa de texto grande
    public string descricao; // Descrição do item (ex: Espada de Ferro Velha)
    public Sprite icone; // Icone do item

    [Header("Lore e História")]
    [TextArea(3, 10)]
    public string loreOculta; // História do item (ex: Pertenceu ao capitão #####, que morreu de tétano)

    [Header("Dados de Equipamento")]
    public TipoEquipamento tipo; 

    [Header("Combate Ofensivo")]
    public int danoFisico; // Dano de espada/flecha
    public int danoMagico; // Dano de cajado/feitiço
    public float alcance; // 0.5 (Adaga), 2.0 (Lança), 10.0 (Arco/Magia)

    [Header("Combate Defensivo")]
    public int defesaFisica; // Armadura contra porrada
    public int resistenciaMagica; // Resistência contra fogo/gelo

    [Header("Visual")]
    public Sprite spriteVisual;
}
