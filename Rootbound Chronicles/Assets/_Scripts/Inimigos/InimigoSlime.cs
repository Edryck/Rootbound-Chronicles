using UnityEngine;

public class InimigoSlime : InimigoBase
{
    protected override void TentarAtacar()
    {
        if (alvo != null)
        {
            // Caso futuramente mude o gerenciador do player, só tem que mudar o "PlayerStats" pelo novo nome
            PlayerStats playerScript = alvo.GetComponent<PlayerStats>();
            
            if (playerScript != null)
            {
                if (Time.time >= proximoAtaque) {
                    playerScript.ReceberDano(1); // O Slime tira 1 de vida, para mudar isso é só troca onde tá o 1
                    proximoAtaque = Time.time + 1f / taxaDeAtaque;
                    Debug.Log("Slime atacou o jogador!");
                }
                // Futuramente é bom adicionar algumas coisas legal
                // Como empurrar o jogador ou dar um debuff de velocidade
            }
        }
    }
}
