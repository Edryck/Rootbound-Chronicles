using UnityEngine;

public class InventarioUI : MonoBehaviour
{
    public Transform itensPai; 
    public GameObject inventarioUI;

    SlotInventario[] slots;

    void Start()
    {
        // Pega o inventário e se inscreve no evento
        Inventario.instance.onItemChangedCallback += AtualizarUI;

        // Pega todos os slots filhos do Grid
        slots = itensPai.GetComponentsInChildren<SlotInventario>();

        // Começa fechado
        inventarioUI.SetActive(false);
    }

    void Update()
    {
        // Abre e fecha com a tecla I ou Tab
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
        {
            inventarioUI.SetActive(!inventarioUI.activeSelf);
        }
    }

    void AtualizarUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // Agora verificamos a lista 'mochila'
            if (i < Inventario.instance.mochila.Count)
            {
                // Pegamos o slot inteiro (dados + quantidade)
                Inventario.SlotDeItem slotAtual = Inventario.instance.mochila[i];
                
                // Passamos os dados E a quantidade para o visual
                slots[i].AddItem(slotAtual.dadosDoItem, slotAtual.quantidade);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
