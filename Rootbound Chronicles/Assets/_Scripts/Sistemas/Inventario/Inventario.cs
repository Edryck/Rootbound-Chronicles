using UnityEngine;
using System.Collections.Generic;

public class Inventario : MonoBehaviour
{
    public static Inventario instance;

    // O Evento que avisa a UI para atualizar
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    [System.Serializable]
    public class SlotDeItem
    {
        public ItemData dadosDoItem;
        public int quantidade;
    }

    public int espaco = 20; // Limite de slots
    public List<SlotDeItem> mochila = new List<SlotDeItem>();

    void Awake()
    {
        if (instance != null) return;
        instance = this;
    }

    public bool AdicionarItem(ItemData itemParaAdicionar, int qtd)
    {
        // Tenta empilhar
        foreach (SlotDeItem slot in mochila)
        {
            if (slot.dadosDoItem == itemParaAdicionar)
            {
                slot.quantidade += qtd;
                
                // Avisa a UI
                if (onItemChangedCallback != null) onItemChangedCallback.Invoke();
                return true;
            }
        }

        // Se não empilhou, verifica se tem espaço
        if (mochila.Count >= espaco)
        {
            Debug.Log("Mochila Cheia!");
            return false;
        }

        // Cria novo slot
        SlotDeItem novoSlot = new SlotDeItem();
        novoSlot.dadosDoItem = itemParaAdicionar;
        novoSlot.quantidade = qtd;
        mochila.Add(novoSlot);

        // Avisa a UI
        Debug.Log("Novo item: " + itemParaAdicionar.nomeDoItem);
        if (onItemChangedCallback != null) onItemChangedCallback.Invoke();
        
        return true;
    }

    // Função para remover ou gastar itens
    public void RemoverItem(ItemData itemParaRemover, int qtd = 1)
    {
        // Procura o item na mochila
        SlotDeItem slot = mochila.Find(x => x.dadosDoItem == itemParaRemover);

        if (slot != null)
        {
            slot.quantidade -= qtd;

            // Se zerou, remove o slot da lista
            if (slot.quantidade <= 0)
            {
                mochila.Remove(slot);
            }

            // Avisa a UI que mudou
            if (onItemChangedCallback != null) onItemChangedCallback.Invoke();
        }
    }
}