using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float speed = 2f;
    public float gravity = -10f;
    public float jumpSpeed = 50f;
    bool grounded;
    float xRotation;
    public Vector3 velocity;
    //Camera playerCamera;
    Transform cameraTransform;
    CharacterController charControl;
    float groundedMOE = 1.1f;
    public LayerMask groundLayerMask;
    public Animator anim;
    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    void PlayerMovement()
    {
        /*if(charControl.isGrounded && velocity.y < -2)
        {
            velocity.y = 0f;
        }*/
        grounded = Physics.Raycast(gameObject.transform.position, Vector3.down, groundedMOE, groundLayerMask);
        if(grounded && velocity.y < -2)
        {
            velocity.y = 0f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if(z != 0 || x != 0)
        {
            anim.SetBool("moving", true);
        }
        else
        {
            anim.SetBool("moving", false);
        }


        Vector3 move = (transform.right * x) + (transform.forward * z);
        charControl.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        charControl.Move(velocity * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && grounded)
        {
            Debug.Log("jump");
            velocity.y += Mathf.Sqrt(jumpSpeed * -1 * gravity);
        }
    }
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //playerCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        cameraTransform = GameObject.FindWithTag("MainCamera").GetComponent<Camera>().transform;
        charControl = gameObject.GetComponent<CharacterController>();
    }
    private void Update()
    {
        MouseLook();
        PlayerMovement();
    }
}
