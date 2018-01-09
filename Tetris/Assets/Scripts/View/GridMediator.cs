using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GridMediator : EventMediator
    {
        [Inject]
        public GridView view { get; set; }

        [Inject]
        public IGrid grid { get; set; }

        [Inject]
        public PutShapeSignal putShapeSignal { get; set; }

        public override void OnRegister()
        {
            view.Init();
            grid.Reset(view.size);
            putShapeSignal.AddListener(updateGridModel);
        }

        
        public override void OnRemove()
        {
            //view.dispatcher.UpdateListener(false, GameEvent.PUT_ON_BOARD, Show);
        }

        public void updateGridModel(ShapeView shape)
        {
            List<Vector3> shapeBlockPosition = shape.GetLastWorldPosition();
            Color shapeColor = shape.GetShapeColor();
            view.ShowBlocks(shapeBlockPosition, shapeColor);
        }
    }
}