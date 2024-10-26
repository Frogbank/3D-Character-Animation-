using System.Collections;
using UnityEngine;
public class Player : MonoBehaviour
{
    [Header("Character Controller")]
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

    [Header("Dust Particles")]
    public GameObject dustParticles;
    public Transform[] particleChecks;
    private bool leftCheck = false;
    private bool rightCheck = false;
    public float dustDistance;

    [Header("Laser")]
    public LineRenderer lineRenderer;
    public float laserTime;
    public Transform laserPos;
    public GameObject laserSparks;

    public BoxCollider kickTrigger;


    void Start()
    {
        // gets animator and character controller
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;

        // ignores collision between the player and ragdoll
        Physics.IgnoreLayerCollision(6, 7);
        Physics.IgnoreLayerCollision(6, 6);

        // gets all rigidbodies in the ragdoll and sets them to kinematic
        rb = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody ragdoll in rb)
        {
            ragdoll.isKinematic = true;
        }
        
        lineRenderer.enabled = false;

        kickTrigger.enabled = false;
    }
    void Update()
    {
        // checks for the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // jumps - starts animation and adds velocity upwards
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            animator.ResetTrigger("jump");
            animator.SetTrigger("jump");
            velocity.y = 4f;
        }
        // adds gravity if not grounded
        velocity.y += gravity * Time.deltaTime;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        // adds vertical velocity
        characterController.Move(velocity * Time.deltaTime);


        // kick
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            canMove = false;

            animator.ResetTrigger("Kick");
            animator.SetTrigger("Kick");


            StartCoroutine(Kick());
        }



        // increases speed when running and turns animations to running animations
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


        if(canMove) // allows the player to move if not a ragdoll
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            characterController.Move(move.normalized * moveSpeed * Time.deltaTime);


            if(!running) // sets movement animations
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


        // laser
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            if(Physics.Raycast(laserPos.position, laserPos.forward, out hit))
            {
                StartCoroutine(Laser(hit.point, hit.normal));
                Enemies enemy = hit.transform.root.gameObject.GetComponent<Enemies>();
                if(enemy != null)
                {
                    enemy.Ragdoll();
                }
            }
        }


        // dust particles
        if (!leftCheck && Physics.CheckSphere(particleChecks[0].position, dustDistance, groundMask))
        {
            SpawnDustParticles(particleChecks[0].position);
            leftCheck = true;
        }

        if (!rightCheck && Physics.CheckSphere(particleChecks[1].position, dustDistance, groundMask))
        {
            SpawnDustParticles(particleChecks[1].position);
            rightCheck = true;
        }

        leftCheck = Physics.CheckSphere(particleChecks[0].position, dustDistance, groundMask);
        rightCheck = Physics.CheckSphere(particleChecks[1].position, dustDistance, groundMask);
    }
    public void Ragdoll() // turns the character to a ragdoll
    {
        canMove = false;
        animator.enabled = false;
        foreach (Rigidbody ragdoll in rb)
        {
            ragdoll.isKinematic = false;
        }
        Physics.IgnoreLayerCollision(6, 6, false);
    }

    private void SpawnDustParticles(Vector3 pos)
    {
        Instantiate(dustParticles, pos, Quaternion.Euler(-90, 0, 0)); 
    }

    private IEnumerator Laser(Vector3 hitPos, Vector3 hitNormal)
    {
        lineRenderer.enabled = true;

        lineRenderer.SetPosition(0, laserPos.position);
        lineRenderer.SetPosition(1, hitPos);

        GameObject sparks = Instantiate(laserSparks, hitPos + (hitNormal * 0.5f), Quaternion.identity);
        Quaternion sparkRotation = Quaternion.LookRotation(hitNormal);
        sparks.transform.rotation = sparkRotation;

        yield return new WaitForSeconds(laserTime);
        lineRenderer.enabled = false;
    }

    private IEnumerator Kick()
    {
        yield return new WaitForSeconds(1);

        kickTrigger.enabled = true;

        yield return new WaitForSeconds(1);
        kickTrigger.enabled = false;
        canMove = true;
    }
}
