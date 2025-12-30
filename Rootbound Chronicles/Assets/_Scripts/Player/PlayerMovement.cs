using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 5f;
    public Animator animator;
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

        // Passa os valores para o Animator
        if (movement != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }

        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
