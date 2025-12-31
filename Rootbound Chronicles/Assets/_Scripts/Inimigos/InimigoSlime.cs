using UnityEngine;

public class InimigoSlime : InimigoBase
{
    protected override void TentarAtacar()
    {
        if (alvo != null)
        {
            PlayerStats playerScript = alvo.GetComponent<PlayerStats>();
            
            if (playerScript != null)
            {
                // Toca Animação
                if (animator != null)
                {
                    animator.SetTrigger("Atacar");
                }

                // Aplica Dano
                playerScript.ReceberDano(1, TipoDano.Fisico); 
                Debug.Log("Slime atacou o jogador!");
                
                // Adicionar Knockback aqui
            }
        }
    }
}
