using UnityEngine;
using UnityEngine.UI;

public class SlotInventario : MonoBehaviour
{
    public Image icon;
    public Button removeButton; // Para jogar o item fora

    ItemData item;

    public void AddItem(ItemData newItem, int quantidade)
    {
        item = newItem;
        icon.sprite = item.icone;
        icon.enabled = true;
        
        // Futuro: Se tiver um texto de quantidade, atualize aqui:
        // textoQuantidade.text = quantidade.ToString();
    }

    public void ClearSlot()
    {
        item = null;
        item.spriteVisual = null;
        // item.enabled = false;
    }

    public void ItemEmUso()
    {
        if (item != null)
        {
            if (item.tipo != TipoEquipamento.Nenhum)
            {
                EquipmentManager.instance.Equipar(item);
            }
            else
            {
                Debug.Log("Usando item: " + item.nomeDoItem);
                // TODO: Usar item (item.Usar();)
            }
        }
    }
    
    // TODO: Remove do Inventario se for consumivel
    // Inventario.instance.RemoverItem(item);
}
