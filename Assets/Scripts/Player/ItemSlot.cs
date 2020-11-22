using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyFPS.Player
{
    public class ItemSlot : MonoBehaviour, IDropHandler
    {
        /// <summary>
        /// The weapon in this ItemSlot. 
        /// </summary>
        public GameObject weapon = null;
        /// <summary>
        /// Reference to the player handler for this class. Must be set in inspector on Prefab player.
        /// </summary>
        [SerializeField] PlayerHandler playerHandler;
        /// <summary>
        /// Determines if this instance of ItemSlot is the slot that represents the players equip slot.
        /// </summary>
        public bool equipSlot = false;
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                if(weapon != null)//If there is a weapon in this item slot
                {
                    weapon.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<DragDrop>().currentItemSlot.GetComponent<RectTransform>().anchoredPosition;//set this items slots weapon's position to new weapons olditem slot position
                    weapon.GetComponent<DragDrop>().currentItemSlot = eventData.pointerDrag.GetComponent<DragDrop>().currentItemSlot.gameObject;//set this items currentItemSlot to new weapons currentItemSlot
                }
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;//set new weapons position
                eventData.pointerDrag.GetComponent<DragDrop>().currentItemSlot = gameObject;//set new weapons currentItemSlot
                eventData.pointerDrag.GetComponent<CanvasGroup>().blocksRaycasts = false;
                weapon = eventData.pointerDrag.gameObject;//set weapon to new weapon
                if (equipSlot)
                {
                    playerHandler.SwitchWeapon(eventData.pointerDrag.GetComponent<DragDrop>().weaponIndex);//calls players switch weapon function with new weapons weaponIndex
                }
            }
        }
    }
}