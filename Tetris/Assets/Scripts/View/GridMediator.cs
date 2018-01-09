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

        [Inject]
        public ResetGameSignal resetGameSignal { get; set; }

        [Inject]
        public AfterUpdateSignal afterUpdateSignal { get; set; }

        public override void OnRegister()
        {
            view.Init();
            resetGameSignal.AddListener(resetGrid);
            putShapeSignal.AddListener(updateGridModel);
        }
                
        public override void OnRemove()
        {
            resetGameSignal.RemoveListener(resetGrid);
            putShapeSignal.RemoveListener(updateGridModel);
        }

        private void updateGridModel(ShapeView shape)
        {
            List<Vector3> shapeBlockPosition = shape.GetLastWorldPosition();
            Color shapeColor = shape.GetShapeColor();
            view.ShowBlocks(shapeBlockPosition, shapeColor);
            List<Index> indexes = view.ConvertWorldPosToIndex(shapeBlockPosition);
            grid.AddShape(indexes);

            StartCoroutine(CheckGrid());
        }

        private void resetGrid()
        {
            view.Reset();
            grid.Reset(view.size);
        }

        private IEnumerator CheckGrid()
        {
            if (grid.CheckRowsAndColumns() > 0)
            {
                yield return new WaitForSeconds(.1f);
                view.UpdateGrid(grid.GridArray);           
            }
            afterUpdateSignal.Dispatch();
        }
    }
}