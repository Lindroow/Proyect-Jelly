using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    Vector3 movement;
    Vector3 moveDir;
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

    [Header("Dash")]
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;

    private float turnSmoothVelocity;

    private Rigidbody rb;
    private CharacterController characterController;
    [SerializeField] ParticleSystem[] particleSystem;
    [SerializeField] particlesControl particlesControl;

    [Header("Particulas esteticas")]
    [SerializeField] ParticleSystem particlesGrass;
    [SerializeField] ParticleSystem particlesDash;
    [SerializeField] TrailRenderer trailIce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Jump();
        //Dash
        if (Input.GetButtonDown("Fire3") && fireHability == true)
        {
            StartCoroutine(Dash());
        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
        Movement();

    }

    public void Jump()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.55f)) // Comprueba si el objeto está en el suelo usando un Raycast
        {
            Debug.Log("Suelo");
            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Aplica una fuerza hacia arriba para simular el salto
                if (airHability)
                {
                    doubleJump = true;
                }
            }
        }
        else
        {
            rb.AddForce(Vector3.down * -gravity, ForceMode.Force);
            if (Input.GetButtonDown("Jump") && airHability && doubleJump)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Aplica una fuerza hacia arriba para simular el doble salto
                particlesControl.StartCoroutine("airHability");
                doubleJump = false;
            }
        }

        //characterController.Move(velocity * Time.deltaTime);

        //// Aplicar gravedad
        //if (characterController.isGrounded)
        //{
        //    Debug.Log("Suelo");
        //    velocity.y = -2;

        //    if (Input.GetButtonDown("Jump"))
        //    {
        //        velocity.y = jumpForce;
        //        if (airHability == true)
        //        {
        //            doubleJump = true;
        //        }
        //    }

        //}
        //else
        //{
        //    velocity.y -= gravity * -3 * Time.deltaTime;


        //    //Doble salto si tiene determinada cantidad de partículas de aire
        //    if (Input.GetButtonDown("Jump") && airHability == true && doubleJump == true)
        //    {
        //        velocity.y = jumpForce;
        //        particlesControl.StartCoroutine("airHability");
        //        doubleJump = false;
        //    }
        //}

    }

    public void Movement()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        // Movement
        Vector3 movement = new Vector3(inputX, 0f, inputZ).normalized;
        //Vector3 moveDir = Vector3.zero;

        if (movement.magnitude > 0.1f)
        {
            // Rotation
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Movement
            //moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Apply movement using Rigidbody
            //rb.velocity = moveDir.normalized * speed * Time.deltaTime;
            rb.MovePosition(rb.position + moveDir.normalized * speed * Time.deltaTime);
        }

        
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;
        particlesDash.Play();
        particlesControl.StartCoroutine("FireHability");
        while(Time.time < startTime + dashTime)
        {
            rb.MovePosition(rb.position + moveDir.normalized * dashSpeed * Time.deltaTime);
            yield return null;
        }
        particlesDash.Stop();
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
            //Agranda el tamaño el focus cuanto máyor sea la cantidad de particulas;
            particlesControl.FocusSize();

            //Cada tipo de particula se setea con un número y el total de particulas
            particlesControl.numParticlesSet();
            particlesControl.ActiveHability(ref fireHability,ref waterHability,ref airHability,ref plantHability);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Water") && waterHability == true)
        {
            Debug.Log("Colision con agua");
            particlesControl.WaterHabilityActive();
            trailIce.emitting = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water") && waterHability == true)
        {
            particlesControl.WaterHabilityInactive();
            trailIce.emitting = false;
        }
    }

}
