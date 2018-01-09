using strange.extensions.mediation.impl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
    public class UIManagerView : EventView {

        public Text ScoreText;
        public GameObject GameOverPanel;
        public Button RestartButton;

        [Inject]
        public ResetGameSignal resetSignal { get; set; }

        public void OnRestartClick()
        {
            resetSignal.Dispatch();
        }

        public void UpdateScore(string scoreText)
        {
            ScoreText.text = scoreText;
        }

        public void EnableGameOverPanel()
        {
            GameOverPanel.SetActive(true);
        }

        public void DisableGameOverPanel()
        {
            GameOverPanel.SetActive(false);
        }


    }
}