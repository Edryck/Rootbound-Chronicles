using UnityEngine;
using System.Collections.Generic;

public class Inventario : MonoBehaviour
{
    [System.Serializable]
    public class SlotDeItem
    {
        public string nomeDoItem;
        public int quantidade;
        // Mais pra frente tem que adicionar o public Sprite icone;
    }

    public List<SlotDeItem> mochila = new List<SlotDeItem>();

    public void AdicionarItem(string nome, int qtd)
    {
        // Verifica se já tem esse item na mochila, se já tiver então empilha ele
        foreach (SlotDeItem item in mochila)
        {
            if (item.nomeDoItem == nome)
            {
                item.quantidade += qtd;
                Debug.Log("Item acumulado: " + nome + " | Total: " + item.quantidade);
                return;
            }
        }

        // Se não tiver, então cria um novo slot
        SlotDeItem novoItem = new SlotDeItem();
        novoItem.nomeDoItem = nome;
        novoItem.quantidade = qtd;

        // Adiciona na lista Mochila
        mochila.Add(novoItem);
        Debug.Log("Novo item criado: " + nome);
    }

    // Função auxiliar para verificar se tem o item, para quando incrementar o sistema de crafting
    public bool TemItem(string nome, int qtdNecessaria)
    {
        foreach (SlotDeItem item in mochila)
        {
            if (item.nomeDoItem == nome && item.quantidade >= qtdNecessaria)
            {
                return true;
            }
        }
        return false;
    }
}