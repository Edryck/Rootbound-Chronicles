using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Status")]
    public int vidaMaxima = 10;
    private int vidaAtual;

    [Header("Configurações")]
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        vidaAtual = vidaMaxima;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Lava")
        {
            vidaAtual--;
            Debug.Log("Vida restante: " + vidaAtual);

            if (vidaAtual <= 0)
            {
                Debug.Log("Você morreu!");
                Destroy(gameObject);
            }
        }
        if (other.tag == "Cura")
        {
            if (vidaAtual == vidaMaxima)
            {
                Debug.Log("Vida cheia!");
            }
            else
            {
                vidaAtual++;
                Debug.Log("Curando, vida atual: " + vidaAtual);
            }
        }
        if (other.tag == "Pocao")
        {
            if (vidaAtual != vidaMaxima)
            {
                vidaAtual = vidaMaxima;
                Debug.Log("Vida máxima restaurada!");
                Destroy(other.gameObject);
            }
        }
    }
}
