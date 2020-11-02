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

            Controls.Player.Fire.started += ctx => Shoot();
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
        void Shoot() => CMDShoot(cameraTransform.position, cameraTransform.forward);
        [Command]
        void CMDShoot(Vector3 cameraForward, Vector3 cameraPosition)
        {
            Ray ray = new Ray(cameraPosition, cameraForward * 500);
            RaycastHit hit;
            Debug.DrawRay(cameraPosition, cameraForward * 500, Color.red, 2f);
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.CompareTag("Player"))
                {
                    Debug.Log("player hit: " + hit.collider.GetComponent<NetworkIdentity>().netId);
                    RpcPlayerFiredEntity(GetComponent<NetworkIdentity>().netId, hit.collider.GetComponent<NetworkIdentity>().netId, hit.point, hit.normal);
                }
            }
        }
        [ClientRpc]
        void RpcPlayerFiredEntity(uint shootID, uint targetID, Vector3 impactPos, Vector3 impactRot)
        {

        }
    }
}