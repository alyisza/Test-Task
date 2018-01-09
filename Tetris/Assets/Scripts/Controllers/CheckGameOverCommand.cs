using strange.extensions.command.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CheckGameOverCommand : Command
    {
        [Inject]
        public List<ShapeView> shapeList { get; set; }

        [Inject]
        public IGrid grid { get; set; }

        [Inject]
        public GameOverSignal gameOverSignal { get; set; }

        public override void Execute()
        {
            foreach(ShapeView shape in shapeList)
            {
                if (grid.CheckAvailabilityForShape(shape.GetShapeInBoolArrray()))
                    return;
            }
            gameOverSignal.Dispatch();
        }
    }
}