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

        // Override Start so that we can fire the StartSignal 
        override public IContext Start()
        {
            base.Start();
            StartSignal startSignal = (StartSignal)injectionBinder.GetInstance<StartSignal>();
            startSignal.Dispatch();
            return this;
        }

        protected override void mapBindings()
        {
            injectionBinder.Bind<IGrid>().To<GridModel>().ToSingleton();

            injectionBinder.Bind<PutShapeSignal>().ToSingleton();
            injectionBinder.Bind<ResetGameSignal>().ToSingleton();
            injectionBinder.Bind<GameOverSignal>().ToSingleton();
            injectionBinder.Bind<AfterUpdateSignal>().ToSingleton();

            commandBinder.Bind<StartSignal>().To<StartCommand>().Once();
            commandBinder.Bind<CheckGameOverSignal>().To<CheckGameOverCommand>();

            mediationBinder.Bind<GridView>().To<GridMediator>();
            mediationBinder.Bind<ShapeCreatorView>().To<ShapeCreatorMediator>();


        }
    }

}