using strange.extensions.command.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game
{
    public class StartCommand : Command
    {
        [Inject]
        public ResetGameSignal resetGameSignal { get; set; }

        public override void Execute()
        {
            resetGameSignal.Dispatch();
        }
        
    }
}