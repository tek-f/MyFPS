﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyFPS.GameAdmin
{
    public class GameModeCTF : GameMode
    {
        protected override void Start()
        {
            base.Start();
            gameType = "CTF";
        }
    }
}