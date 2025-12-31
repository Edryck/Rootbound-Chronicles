using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerE2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.SalvarCheckpoint(transform.position);
                // TODO: Feedback visual ou sonoro
                Debug.Log("Ponto de respawn atualizado!");
                // TODO: SistemaDialogo.instance.MostrarTexto("Progresso salvo.");
            }
        }
    }
}
