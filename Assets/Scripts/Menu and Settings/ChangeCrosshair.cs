using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyFPS.Player
{
    public class ChangeCrosshair : MonoBehaviour
    {
        public List<Sprite> crosshairOptions = new List<Sprite>();
        public PlayerHandler playerHandler;

        public void CrosshairChange(int crossHairOptionsReference)
        {
            playerHandler.crosshair.sprite = crosshairOptions[crossHairOptionsReference];
        }
    }
}