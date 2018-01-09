using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game
{
    public class GameContext : MVCSContext
    {

        public GameContext(MonoBehaviour view) : base(view)
        {
        }


        public GameContext(MonoBehaviour view, bool autoStartup) : base(view, autoStartup)
        { }

        protected override void mapBindings()
        {
            injectionBinder.Bind<IGrid>().To<GridModel>().ToSingleton();
          
            mediationBinder.Bind<GridView>().To<GridMediator>();

            injectionBinder.Bind<PutShapeSignal>().ToSingleton();

        }
    }

}