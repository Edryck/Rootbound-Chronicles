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
    private PlayerCombat combat;

    void Awake()
    {
        instance = this;
        int numSlots = System.Enum.GetNames(typeof(TipoEquipamento)).Length;
        equipamentosAtuais = new ItemData[numSlots];
    }

    void Start()
    {
        // Pega o número de slots baseado no Enum
        int numSlots = System.Enum.GetNames(typeof(TipoEquipamento)).Length;
        equipamentosAtuais = new ItemData[numSlots];

        stats = GetComponent<PlayerStats>();
        combat = GetComponent<PlayerCombat>();
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

    public (int danoFisico, int danoMagico) CalcularAtaqueTotal()
    {
        int fisico = 0;
        int magico = 0;

        foreach (ItemData item in equipamentosAtuais)
        {
            if (item != null)
            {
                fisico += item.danoFisico;
                magico += item.danoMagico;
            }
        }
        return (fisico, magico);
    }

    public void Equipar(ItemData novoItem)
    {
        if (novoItem.tipo == TipoEquipamento.Nenhum) return;

        // Para descobrir o índice (int) do tipo (Ex: Peito = 2)
        int slotIndex = (int)novoItem.tipo;
        // Se já tiver algo, desequipa primeiro
        ItemData itemAntigo = equipamentosAtuais[slotIndex];
        if (itemAntigo != null) { Desequipar(slotIndex); }
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

        // Atualiza os status de defesa/resistência
        if (stats == null) stats.RecalcularStatus();
        // Atualiza os status de dano físico/mágico
        if (combat != null) combat.RecalcularAtributosOfensivos();
        
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

            // Devolve pro inventário
            if (Inventario.instance != null)
                Inventario.instance.AdicionarItem(itemRemovido, 1);

            equipamentosAtuais[slotIndex] = null;

            // Atualiza os status de defesa/resistência
            if (stats == null) stats.RecalcularStatus();
            // Atualiza os status de dano físico/mágico
            if (combat != null) combat.RecalcularAtributosOfensivos();

            Debug.Log("Desequipou: " + itemRemovido.nomeDoItem);
        }
    }
}
