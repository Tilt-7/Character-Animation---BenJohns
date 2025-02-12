using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    // Variables
    public CharacterController controller;
    public Animator anim;
    public AudioClip runningSound;
    private AudioSource audioSource;

    public float runningSpeed = 4.0f;
    public float rotationSpeed = 100.0f;
    public float jumpHeight = 6.0f;
    public float gravity = -9.8f; // Gravity value to pull the player down

    private float runInput;
    private float rotateInput;
    private Vector3 velocity;
    private bool isJumping = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get the CharacterController, Animator, and AudioSource components
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get input values for movement and rotation
        runInput = Input.GetAxis("Vertical");  // Forward/Backward input
        rotateInput = Input.GetAxis("Horizontal");  // Left/Right input

        // Call jump checking function
        CheckJump();

        // Set moveDir to new Vector3 based on input values
        Vector3 moveDir = new Vector3(0, velocity.y, runInput * runningSpeed);

        // Transform the direction to match the world space
        moveDir = transform.TransformDirection(moveDir);

        // Move the character based on direction and deltaTime for frame-independent movement
        controller.Move(moveDir * Time.deltaTime);

        // Rotate the character based on Horizontal input
        transform.Rotate(0f, rotateInput * rotationSpeed * Time.deltaTime, 0f);

        // Call function to handle animations and sound effects
        Effects();
    }

    // Check if the player is pressing space to jump
    void CheckJump()
    {
        if (controller.isGrounded)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Apply jump force
                isJumping = true;
            }
            else
            {
                velocity.y = -2f; // Small downward force to ensure the player stays grounded
                isJumping = false;
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; // Apply gravity when in the air
        }
    }

    // Manage animations and sound effects
    void Effects()
    {
        // Set jump animation state
        if (isJumping)
        {
            anim.SetBool("Jump", true);
            anim.SetBool("Run", false); // Prevent running animation while jumping
        }
        else
        {
            anim.SetBool("Jump", false);

            // Running animation and sound effect only if not jumping
            if (runInput != 0)
            {
                anim.SetBool("Run", true);

                if (!audioSource.isPlaying) // Prevent sound from overlapping
                {
                    audioSource.clip = runningSound;
                    audioSource.Play();
                }
            }
            else
            {
                anim.SetBool("Run", false);
                audioSource.Stop();  // Stop sound when not running
            }
        }
    }
}