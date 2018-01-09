using strange.extensions.context.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game {
    public class GameContextView : ContextView {


        void Awake()
        {
            context = new GameContext(this);
        }
    }
}