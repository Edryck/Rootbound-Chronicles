using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Classes existentes
    public enum Classe {
        Guerreiro,
        Mago, 
        Arqueiro,
        Ladino,
        Clérigo
    }

    [Header("Configuração da Classe")]
    public Classe classeAtual;

    [Header("Status Vitais")]
    public int vidaMaxima = 100;
    public int vidaAtual;
    // Futuramente public int mana;
    // Futuramento public int stamina;

    [Header("Defesa e Resistência")]
    public int defesaFisicaTotal = 0;
    public int resistenciaMagicaTotal = 0;

    void Start()
    {
        vidaAtual = vidaMaxima;
        RecalcularStatus(); // Para calcular os status iniciais
    }

    public void RecalcularStatus()
    {
        // Reseta para o base
        defesaFisicaTotal = 0; 
        resistenciaMagicaTotal = 0;

        // Pergunta pro EquipmentManager o que o jogador está vestindo
        if (EquipmentManager.instance != null)
        {
            // Pega os bônus do equipamento
            var bonus = EquipmentManager.instance.CalcularBonusTotal();
            
            defesaFisicaTotal += bonus.defesaFisica;
            resistenciaMagicaTotal += bonus.resistenciaMagica;
        }
        
        Debug.Log($"Stats Atualizados: Def {defesaFisicaTotal} | Res {resistenciaMagicaTotal}");
    }

    public void ReceberDano(int danoBruto, TipoDano tipoDoAtaque)
    {
        int danoFinal = danoBruto;

        // Lógica de Redução
        if (tipoDoAtaque == TipoDano.Fisico)
        {
            danoFinal -= defesaFisicaTotal;
        }
        else if (tipoDoAtaque == TipoDano.Magico)
        {
            danoFinal -= resistenciaMagicaTotal;
        }

        // O dano nunca pode ser menor que zero (vai curar o jogador)
        // Vai ficar definido como dano mínimo = 1
        if (danoFinal <= 0) danoFinal = 1;

        vidaAtual -= danoFinal;
        Debug.Log($"Tomou {danoFinal} de dano ({tipoDoAtaque}). Vida: {vidaAtual}");

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    public void Curar(int quantidade)
    {
        vidaAtual += quantidade;
        if (vidaAtual > vidaMaxima) vidaAtual = vidaMaxima;
        Debug.Log("Player curado: " + vidaAtual);
    }

    protected virtual void Morrer()
    {
        Debug.Log("Morreu!"); // TODO: Depois tem que colocar alguma coisa pra puxar o motivo da morte
        Destroy(gameObject); // TODO: Já avisando que vai dar merda isso, vai bugar a câmera
        // Aqui pode dropar loot
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Zona de Cura
        if (other.tag == "Cura")
        {
            if (vidaAtual == vidaMaxima)
            {
                Debug.Log("Vida cheia!");
            }
            else
            {
                Curar(2);
            }
        }

        // Poção fodástica
        if (other.tag == "Pocao")
        {
            if (vidaAtual < vidaMaxima)
            {
                Curar(vidaMaxima); // Cura tudo
                Debug.Log("Vida máxima restaurada!");
                Destroy(other.gameObject);
            }
        }
    }
}
