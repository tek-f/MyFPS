using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MyFPS.Mirror
{
    public class MPPlayerMovementController : NetworkBehaviour
    {
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private CharacterController charController = null;

        Vector2 previousInput;

        private Controls controls;
        private Controls Controls
        {
            get
            {
                if(controls != null)
                {
                    return controls;
                }
                return controls = new Controls();
            }
        }

        public override void OnStartAuthority()
        {
            enabled = true;

            Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
            Controls.Player.Move.canceled += ctx => ResetMovement();
        }
        [ClientCallback]
        private void OnEnable() => Controls.Enable();
        [ClientCallback]
        private void OnDisable() => Controls.Disable();
        [ClientCallback]
        private void Update() => Movement();
        [Client]
        private void SetMovement(Vector2 movement) => previousInput = movement;
        [Client]
        private void ResetMovement() => previousInput = Vector2.zero;
        private void Movement()
        {
            Vector3 right = charController.transform.right;
            Vector3 forward = charController.transform.forward;
            right.y = 0f;
            forward.y = 0f;

            Vector3 movement = right.normalized * previousInput.x + forward.normalized * previousInput.y;

            charController.Move(movement * movementSpeed * Time.deltaTime);
        }
    }
}