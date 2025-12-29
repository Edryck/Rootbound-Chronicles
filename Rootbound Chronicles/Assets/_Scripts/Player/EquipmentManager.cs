using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    // Singleton simples para facilitar o acesso
    public static EquipmentManager instance;

    [Header("Cabides Visuais (Arraste os SpriteRenderers aqui)")]
    public SpriteRenderer slotCabeca;
    public SpriteRenderer slotPeito;
    public SpriteRenderer slotPernas;
    public SpriteRenderer slotArma;
    public SpriteRenderer slotEscudo;

    // Para guardar o que está equipado atualmente
    private ItemData[] equipamentosAtuais;
    private PlayerStats stats; // Para poder aumentar defesa/dano

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Pega o número de slots baseado no Enum
        int numSlots = System.Enum.GetNames(typeof(TipoEquipamento)).Length;
        equipamentosAtuais = new ItemData[numSlots];

        stats = GetComponent<PlayerStats>();
    }

    public (int defesaFisica, int resistenciaMagica) CalcularBonusTotal()
    {
        int def = 0;
        int res = 0;

        foreach (ItemData item in equipamentosAtuais)
        {
            if (item != null)
            {
                def += item.defesaFisica;
                res += item.resistenciaMagica;
            }
        }
        return (def, res);
    }

    public void Equipar(ItemData novoItem)
    {
        if (novoItem.tipo == TipoEquipamento.Nenhum) return;

        // Para descobrir o índice (int) do tipo (Ex: Peito = 2)
        int slotIndex = (int)novoItem.tipo;

        // Se já tiver algo, desequipa primeiro (troca)
        // ItemData itemAntigo = equipamentosAtuais[slotIndex];
        // if (itemAntigo != null) { Desequipar(slotIndex); }

        // Salva o novo item
        equipamentosAtuais[slotIndex] = novoItem;

        // Atualiza o visual (Paper Doll)
        switch (novoItem.tipo)
        {
            case TipoEquipamento.Cabeca:
                slotCabeca.sprite = novoItem.spriteVisual;
                break;
            case TipoEquipamento.Peito:
                slotPeito.sprite = novoItem.spriteVisual;
                break;
            case TipoEquipamento.Pernas:
                slotPernas.sprite = novoItem.spriteVisual;
                break;
            case TipoEquipamento.Arma:
                slotArma.sprite = novoItem.spriteVisual;
                break;
            case TipoEquipamento.Escudo:
                slotEscudo.sprite = novoItem.spriteVisual;
                break;
        }

        // TODO: Aplica os Status (Lógica RPG)
        // stats.aumentarDefesa(novoItem.modificadorArmadura);
        stats.RecalcularStatus();
        
        Debug.Log("Equipou: " + novoItem.nomeDoItem);
    }

    public void Desequipar(int slotIndex)
    {
        if (equipamentosAtuais[slotIndex] != null)
        {
            ItemData itemRemovido = equipamentosAtuais[slotIndex];

            // Remove visual
            switch (itemRemovido.tipo)
            {
                case TipoEquipamento.Cabeca:
                    slotCabeca.sprite = null;
                    break;
                case TipoEquipamento.Peito:
                    slotPeito.sprite = null;
                    break;
                case TipoEquipamento.Pernas:
                    slotPernas.sprite = null;
                    break;
                case TipoEquipamento.Arma:
                    slotArma.sprite = null;
                    break;
                case TipoEquipamento.Escudo:
                    slotEscudo.sprite = null;
                    break;
            }

            // TODO: Remove dos status
            // stats.diminuirDefesa(itemRemovido.modificadorArmadura);

            // TODO: Devolve pro inventário (Importante!)
            Inventario.instance.AdicionarItem(itemRemovido, 1);

            equipamentosAtuais[slotIndex] = null;
            stats.RecalcularStatus();
            Debug.Log("Desequipou: " + itemRemovido.nomeDoItem);
        }
    }
}
