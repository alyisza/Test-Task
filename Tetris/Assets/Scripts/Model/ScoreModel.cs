using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreModel : IScore {

    private int score;
    public int Score
    {
        get
        {
            return score;
        }
    }

    public ScoreModel()
    {
        Reset();
    }

    public void AddToScore(int value)
    {
        score += value;
    }

    public void Reset()
    {
        score = 0;
    }


}
