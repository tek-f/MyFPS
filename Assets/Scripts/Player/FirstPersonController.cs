using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using MyFPS.GameAdmin;

namespace MyFPS.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        /// <summary>
        /// The sensitivity of the mouse.
        /// </summary>
        public float mouseSensitivity = 100f;
        /// <summary>
        /// The players speed.
        /// </summary>
        public float speed = 10f;
        /// <summary>
        /// The gravity applied to players.
        /// </summary>
        public float gravity = -10f;
        /// <summary>
        /// The speed of the players jump.
        /// </summary>
        public float jumpSpeed = 5f;
        /// <summary>
        /// Tracks if the player is touching the ground.
        /// </summary>
        bool grounded;
        /// <summary>
        /// The players x rotation.
        /// </summary>
        float xRotation;
        /// <summary>
        /// The players velocity.
        /// </summary>
        public Vector3 velocity;
        /// <summary>
        /// Reference to the Transform of the players camera.
        /// </summary>
        [SerializeField] Transform cameraTransform;
        /// <summary>
        /// Reference to the CharacterController component on this game object.
        /// </summary>
        CharacterController charControl;
        /// <summary>
        /// Offset used in grouded check to account for the size of the player.
        /// </summary>
        float groundedOffset = 1.1f;
        /// <summary>
        /// Reference to the Ground layer.
        /// </summary>
        public LayerMask groundLayerMask = 8;
        /// <summary>
        /// Reference to the PlayerInput component on this game object.
        /// </summary>
        [SerializeField] PlayerInput playerInput;
        /// <summary>
        /// Look action on the player input map.
        /// </summary>
        InputAction lookAction;
        /// <summary>
        /// Move action on the player input map.
        /// </summary>
        InputAction moveAction;
        /// <summary>
        /// Jump action on the player input map.
        /// </summary>
        InputAction jumpAction;
        /// <summary>
        /// Reference to the Animator component on this game object.
        /// </summary>
        public Animator anim;
        /// <summary>
        /// Rotates the camera and player according to moveAction(Mouse movement).
        /// </summary>
        /// <param name="_inputVector">The movement of the mouse as a Vector2.</param>
        void MouseLook(Vector2 _inputVector)
        {

            float mouseX = _inputVector.x;
            float mouseY = _inputVector.y;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
        /// <summary>
        /// Moves the player according to movement input from moveAction buttons(WASD). Also handles jumping according to input from jumpAction button(Space).
        /// </summary>
        /// <param name="_inputVector">The input of moveAction buttons as a Vector2.</param>
        void PlayerMovement(Vector2 _inputVector)
        {
            grounded = Physics.Raycast(gameObject.transform.position, Vector3.down, groundedOffset/*, groundLayerMask*/);
            if (grounded && velocity.y < -2)
            {
                velocity.y = 0f;
            }

            float x = _inputVector.x;
            float z = _inputVector.y;

            if (anim != null)
            {
                if (z != 0 || x != 0)
                {
                    anim.SetBool("moving", true);
                }
                else if(anim.GetBool("moving"))
                {
                    anim.SetBool("moving", false);
                }
            }

            Vector3 move = (transform.right * x) + (transform.forward * z);
            charControl.Move(move * speed * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;
            charControl.Move(velocity * Time.deltaTime);

            if (jumpAction.ReadValue<float>() > 0 && grounded)
            {
                print("jump");
                velocity.y += jumpSpeed;
            }
        }
        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            cameraTransform = gameObject.GetComponentInChildren<Camera>().transform;
            cameraTransform.gameObject.SetActive(true);
            charControl = gameObject.GetComponent<CharacterController>();

            playerInput = gameObject.GetComponent<PlayerInput>();

            moveAction = playerInput.actions.FindAction("Move");
            moveAction.Enable();

            lookAction = playerInput.actions.FindAction("Look");
            lookAction.Enable();

            jumpAction = playerInput.actions.FindAction("Jump");
            jumpAction.Enable();
        }
        private void Update()
        {
            MouseLook(lookAction.ReadValue<Vector2>());
            PlayerMovement(moveAction.ReadValue<Vector2>());
        }

    }
}