using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MouseDrag : MonoBehaviour
{
    /// <summary>
    /// The force to be added.
    /// </summary>
    public float forceAmount = 500;
    /// <summary>
    /// The selected rigidbody.
    /// </summary>
    Rigidbody selectedRigidbody;
    /// <summary>
    /// The target camera.
    /// </summary>
    Camera targetCamera;
    /// <summary>
    /// The original target position in Screen Space.
    /// </summary>
    Vector3 originalScreenTargetPosition;
    /// <summary>
    /// selectedRigidbody's original position.
    /// </summary>
    Vector3 originalRigidbodyPos;
    /// <summary>
    /// The distance that the ray will travel.
    /// </summary>
    float selectionDistance;
    void Start()
    {
        targetCamera = GetComponent<Camera>();
    }
    void Update()
    {
        if (!targetCamera)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            //Check if we are hovering over Rigidbody, if so, select it
            selectedRigidbody = GetRigidbodyFromMouseClick();
        }

        if (Input.GetMouseButtonUp(0) && selectedRigidbody)
        {
            //Release selected Rigidbody if there any
            selectedRigidbody = null;
        }
    }
    void FixedUpdate()
    {
        if (selectedRigidbody)
        {
            Vector3 mousePositionOffset = targetCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectionDistance)) - originalScreenTargetPosition;
            selectedRigidbody.velocity = (originalRigidbodyPos + mousePositionOffset - selectedRigidbody.transform.position) * forceAmount * Time.deltaTime;
        }
        GetRigidbodyFromMouseClick();
    }
    /// <summary>
    /// Returns a rigidbody that has been clicked on.
    /// </summary>
    /// <returns>Rigidbody.</returns>
    Rigidbody GetRigidbodyFromMouseClick()
    {
        RaycastHit hitInfo = new RaycastHit();
        Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);
        bool hit = Physics.Raycast(ray, out hitInfo);
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.blue);
        if (hit)
        {
            if (hitInfo.collider.gameObject.GetComponent<Rigidbody>())
            {
                selectionDistance = Vector3.Distance(ray.origin, hitInfo.point);
                originalScreenTargetPosition = targetCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectionDistance));
                originalRigidbodyPos = hitInfo.collider.transform.position;
                return hitInfo.collider.gameObject.GetComponent<Rigidbody>();
            }
        }
        return null;
    }
}