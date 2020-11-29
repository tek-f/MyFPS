using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFPS.GameAdmin
{
    public class GameModeDM : GameMode
    {
        protected override void Start()
        {
            base.Start();
            gameType = "DM";
            gameScoreLimit = 3;
        }
    }
}