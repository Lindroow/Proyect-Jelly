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
    [SerializeField] Transform camera;


    private float turnSmoothVelocity;

    private CharacterController characterController;
    [SerializeField] ParticleSystem[] particleSystem;

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
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        //Movement
        Vector3 movement = new Vector3(inputX,0f, inputZ).normalized;

        // Aplicar gravedad
        if (characterController.isGrounded)
        {
            movement.y -= gravity * Time.deltaTime;
        }

        // Salto
        if (characterController.isGrounded && Input.GetButtonDown("Jump"))
        {
            movement.y = jumpForce;
        }

        if (movement.magnitude > 0.1f )
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
             
        }
    }
}
