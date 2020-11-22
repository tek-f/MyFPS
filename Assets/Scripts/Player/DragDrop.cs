using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyFPS.Player
{
    public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler,IDragHandler
    {
        /// <summary>
        /// Referencce to the canvas the dragable UI element is on.
        /// </summary>
        [SerializeField] private Canvas canvas;
        /// <summary>
        /// Reference to the Rect Transform of the dragable UI element. Functions as the objects position on the screen.
        /// </summary>
        private RectTransform rectTransform;
        /// <summary>
        /// Reference to the Canvas Group componenet on the dragable UI element.
        /// </summary>
        private CanvasGroup canvasGroup;
        /// <summary>
        /// Index of the weapon this UI element represents in playerHandler.weapons<>. Must be set in the inspector on Prefab player.
        /// </summary>
        public int weaponIndex;
        /// <summary>
        /// The game object of the current ItemSlot class that is displaying this weapon.
        /// </summary>
        public GameObject currentItemSlot;
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("on begin drag.");
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.65f;
        }
        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("on drag.");
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("on end drag.");
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("on pointer click down.");
        }
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }
}