using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private Rigidbody2D rb;
    [Header("Config")]
    private Transform playerAvatar;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float runSpeed = 2f;

    Vector2 movement;
    Animator animator;

    private void Start()
    {
        if (!IsOwner) return;

        rb = GetComponent<Rigidbody2D>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.TryGetComponent<Animator>(out animator))
            {
                playerAvatar = child.transform;
                break;
            }
        }
    }
    private void Update()
    {
        if (!IsOwner) return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }
    private void FixedUpdate()
    {
        if (!IsOwner) return;

        Movement();
    }
    private void Movement()
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", movement.sqrMagnitude >= 0.1f ? 1f : 0f);
        }
        if (movement.sqrMagnitude >= 0.1f)
        {
            if (playerAvatar != null)
            {
                playerAvatar.rotation = Quaternion.Euler(new(0f, movement.x < 0 ? 180f : 0f, 0f));
            }
            float speed = moveSpeed;
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed = runSpeed;
            }
            rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * movement.normalized);
        }
    }
}
