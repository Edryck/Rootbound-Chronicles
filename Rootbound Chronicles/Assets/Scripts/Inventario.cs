using UnityEngine;
using System.Collections.Generic;

public class Inventario : MonoBehaviour
{
    [System.Serializable]
    public class SlotDeItem
    {
        public ItemData dadosDoItem;
        public int quantidade;
        // Mais pra frente tem que adicionar o public Sprite icone;
    }

    public List<SlotDeItem> mochila = new List<SlotDeItem>();

    public void AdicionarItem(ItemData itemParaAdicionar, int qtd)
    {
        // Verifica se já tem esse item na mochila, se já tiver então empilha ele
        foreach (SlotDeItem item in mochila)
        {
            if (item.dadosDoItem == itemParaAdicionar)
            {
                item.quantidade += qtd;
                Debug.Log("Item acumulado: " + itemParaAdicionar.nomeDoItem);
                return;
            }
        }

        // Se não tiver, então cria um novo slot
        SlotDeItem novoItem = new SlotDeItem();
        novoItem.dadosDoItem = itemParaAdicionar;
        novoItem.quantidade = qtd;

        // Adiciona na lista Mochila
        mochila.Add(novoItem);
        Debug.Log("Novo item criado: " + itemParaAdicionar.nomeDoItem);
    }
}