using UnityEngine;

[CreateAssetMenu(fileName = "Novo Item", menuName = "Sistema/Item")]
public class ItemData : ScriptableObject
{
    public string nomeDoItem;
    [TextArea(3, 10)] // Para criar uma caixa de texto grande
    public string descricao; // Descrição do item (ex: Espada de Ferro Velha)
    [TextArea(3, 10)]
    public string loreOculta; // História do item (ex: Pertenceu ao capitão #####, que morreu de tétano)
    public Sprite icone; // Icone do item
}
