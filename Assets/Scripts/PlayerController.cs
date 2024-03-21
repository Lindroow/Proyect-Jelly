using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float gravity;
    [SerializeField] float jumpForce;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] bool doubleJump =false;
    Vector3 velocity;

    [SerializeField] Transform camera;

    [Header("Habilidades")]
    [SerializeField] bool airHability;
    [SerializeField] bool fireHability;
    [SerializeField] bool waterHability;
    [SerializeField] bool plantHability;

    private float turnSmoothVelocity;

    private CharacterController characterController;
    [SerializeField] ParticleSystem[] particleSystem;
    [SerializeField] particlesControl particlesControl;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    // Update is called once per frame
    void Update()
    {
        Jump();
        Movement();
        

    }

    public void Jump()
    {
        // Aplicar gravedad
        if (characterController.isGrounded)
        {
            velocity.y = -1;

            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = jumpForce;
            }
            if (airHability == true)
            {
                doubleJump = true;
            }
        }
        else
        {
            velocity.y -= gravity * -2 * Time.deltaTime;

            //Doble salto si tiene determinada cantidad de partículas de aire
            if (Input.GetButtonDown("Jump") && airHability == true)
            {
                velocity.y = jumpForce;
                particlesControl.StartCoroutine("airHability");
                doubleJump = false;
            }
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    public void Movement()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        //Movement
        Vector3 movement = new Vector3(inputX, 0f, inputZ).normalized;

        if (movement.magnitude > 0.1f)
        {
            //Rotation
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + camera.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Movement
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Al colisionar se setea en el personaje el tipo de particula que juntó y la cantidad
        if (other.CompareTag("ParticlesLot"))
        {
            

            switch (other.GetComponent<ParticleLot>().setType())
            {
                case "Fuego":
                    particleSystem[0].maxParticles += other.GetComponent<ParticleLot>().SetParticles();
                    break;
                case "Agua":
                    particleSystem[1].maxParticles += other.GetComponent<ParticleLot>().SetParticles();
                    break;
                case "Aire":
                    particleSystem[2].maxParticles += other.GetComponent<ParticleLot>().SetParticles();
                    break;
                case "Planta":
                    particleSystem[3].maxParticles += other.GetComponent<ParticleLot>().SetParticles();
                    break;
            }
            //Cada tipo de particula se setea con un número y el total de particulas
            particlesControl.numParticlesSet();
            particlesControl.ActiveHability(ref fireHability,ref waterHability,ref airHability,ref plantHability);
        }
        
    }
}
