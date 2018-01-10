using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class AudioManagerMediator : EventMediator {

        [Inject]
        public AudioManagerView view { get; set; }

        [Inject]
        public ClickShapeSignal clickShapeSignal { get; set; }

        [Inject]
        public DropShapeSignal dropShapeSignal { get; set; }

        public override void OnRegister()
        {
            clickShapeSignal.AddListener(onClick);
            dropShapeSignal.AddListener(onDrop);
        }

        public override void OnRemove()
        {
            clickShapeSignal.RemoveListener(onClick);
            dropShapeSignal.RemoveListener(onDrop);
        }

        private void onClick()
        {
            view.ClickPlay();
        }

        private void onDrop()
        {
            view.DropPlay();
        }
    }
}