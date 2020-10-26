using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

namespace MyFPS
{
    public class MPPlayerCameraController : NetworkBehaviour
    {
        [SerializeField] Transform cameraTransform;
        bool grounded;
        float xRotation;

        Controls controls;
        Controls Controls
        {
            get
            {
                if (controls != null)
                { 
                    return controls; 
                }
                return controls = new Controls();
            }
        }
        public override void OnStartAuthority()
        {
            cameraTransform.gameObject.SetActive(true);

            enabled = true;

            Controls.Player.Look.performed += ctx => MouseLook(ctx.ReadValue<Vector2>());
        }

        [ClientCallback]
        private void OnEnable() => Controls.Enable();
        [ClientCallback]
        private void OnDisable() => Controls.Disable();
        void MouseLook(Vector2 inputVector)
        {
            //float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            //float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            float mouseX = inputVector.x;
            float mouseY = inputVector.y;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
    }
}