using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class UIManagerMediator : EventMediator
    {
        [Inject]
        public UIManagerView view { get; set; }

        [Inject]
        public IScore score { get; set; }

        [Inject]
        public ResetGameSignal resetSignal { get; set; }

        [Inject]
        public GameOverSignal gameOverSignal { get; set; }

        [Inject]
        public AddScoreSignal addScoreSignal { get; set; }

        private readonly string SCORE_TEXT = "Score: ";

        public override void OnRegister()
        {
            resetSignal.AddListener(reset);
            gameOverSignal.AddListener(gameOver);
            addScoreSignal.AddListener(addScore);
        }

        public override void OnRemove()
        {
            resetSignal.RemoveListener(reset);
            gameOverSignal.RemoveListener(gameOver);
            addScoreSignal.RemoveListener(addScore);
        }

        private void addScore(int value)
        {
            score.AddToScore(value);
            view.UpdateScore(SCORE_TEXT + score.Score);
        }

        private void reset()
        {
            score.Reset();
            view.UpdateScore(SCORE_TEXT + score.Score);
            view.DisableGameOverPanel();
        }

        private void gameOver()
        {
            view.EnableGameOverPanel();
        }
    }
}