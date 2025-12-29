using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform alvo;
    public float suavidade = 0.125f; // Entre 0 e 1. Quanto menor, mais suave e lento
    public Vector3 offset; // Distância entre a câmera e o jogador

    void LateUpdate()
    {
        // Caso o jogador morra
        if (alvo == null) {
            Debug.Log("ERRO: O Alvo da câmera está vazio!"); // Vai avisar no console
            return;
        }

        // Descobre para onde quer ir (Posição do Jogador + Distância)
        Vector3 posicao = alvo.position + offset;
        // Lerp significa interpolação linear. É o que faz o movimento "macio"
        Vector3 posicaoSuave = Vector3.Lerp(transform.position, posicao, suavidade);

        // Aplica a posição
        transform.position= posicaoSuave;
    }
}