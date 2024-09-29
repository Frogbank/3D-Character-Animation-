using UnityEngine;
public class Player : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;
    private Rigidbody[] rb;

    public float moveSpeed = 5f;
    private bool running;
    private bool canMove = true;

    public float gravity = -9.8f;
    private Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;
    private bool isGrounded;
    void Start()
    {



        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;

        Physics.IgnoreLayerCollision(6, 7);
        Physics.IgnoreLayerCollision(6, 6);

        rb = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody ragdoll in rb)
        {
            ragdoll.isKinematic = true;
        }
    }
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            animator.ResetTrigger("jump");
            animator.SetTrigger("jump");
            velocity.y = 4f;
        }

        velocity.y += gravity * Time.deltaTime;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        characterController.Move(velocity * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            running = true;
            moveSpeed = 10f;
        }
        else
        {
            running = false;
            moveSpeed = 5f;
        }
        if(canMove)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            characterController.Move(move.normalized * moveSpeed * Time.deltaTime);
            if(!running)
            {
                animator.SetFloat("velocityX", x * 0.5f);
                animator.SetFloat("velocityY", z * 0.5f);
            }
            else
            {
                animator.SetFloat("velocityX", x);
                animator.SetFloat("velocityY", z);
            }
        }


    }
    public void Ragdoll()
    {
        canMove = false;
        animator.enabled = false;
        foreach (Rigidbody ragdoll in rb)
        {
            ragdoll.isKinematic = false;
        }
        Physics.IgnoreLayerCollision(6, 6, false);
    }

}
