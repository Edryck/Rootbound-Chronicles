using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    // Referência opcional caso a velocidade mude baseada na classe
    // private PlayerStats stats;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // stats = GetComponent<PlayerStats>();
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
}
