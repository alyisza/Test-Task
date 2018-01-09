using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ShapeCreatorMediator : EventMediator
    {
        [Inject]
        public ShapeCreatorView view { get; set; }

        [Inject]
        public ResetGameSignal resetGameSignal { get; set; }

        [Inject]
        public PutShapeSignal putShapeSignal { get; set; }

        [Inject]
        public CheckGameOverSignal checkGameOverSignal { get; set; }

        [Inject]
        public AfterUpdateSignal afterUpdateSignal { get; set; }

        public override void OnRegister()
        {
            resetGameSignal.AddListener(resetShapes);
            putShapeSignal.AddListener(upgradeShapes);
            afterUpdateSignal.AddListener(checkGameOver);
        }

        public override void OnRemove()
        {
            resetGameSignal.RemoveListener(resetShapes);
            putShapeSignal.RemoveListener(upgradeShapes);
            afterUpdateSignal.RemoveListener(checkGameOver);
        }

        private void checkGameOver()
        {
            checkGameOverSignal.Dispatch(view.GetShapes());
        }

        private void resetShapes()
        {
            view.Reset();
        }

        private void upgradeShapes(ShapeView shape)
        {
            view.UpdateShapes();
        }
    }
}