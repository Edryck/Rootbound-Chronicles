using UnityEngine;
using UnityEngine.UI;

public class SlotInventarioUI : MonoBehaviour
{
    [Header("Configuração")]
    public Image iconDisplay;
    public Sprite iconePadrao;
    public TipoEquipamento tipoDeSlot;

    ItemData itemAtual;

    void Start()
    {
        // Inicia com o item vazio (a silhueta)
        AtualizarSlot(null);
    }

    public void AtualizarSlot(ItemData item)
    {
        itemAtual = item;

        if (item != null)
        {
            iconDisplay.sprite = item.icone;
            iconDisplay.enabled = true;
            // iconDisplay.color = Color.white; // Caso tenha escurecido antes
        }
        else
        {
            // Se tiver um sprite de silhueta, mostra ele
            if (iconePadrao != null)
            {
                iconDisplay.sprite = iconePadrao;
                iconDisplay.enabled = true;
            }
            else
            {
                iconDisplay.enabled = false; // Ou esconde tudo
            }
        }
    }

    // Função para o Botão (Unequip)
    public void DesequiparItem()
    {
        if (itemAtual != null)
        {
            EquipmentManager.instance.Desequipar((int)tipoDeSlot);
            AtualizarSlot(null); // Limpa visualmente
        }
    }
}
