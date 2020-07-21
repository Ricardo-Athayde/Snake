using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.SaveSystem;

namespace Snake
{
    public class ScoreManager : MonoBehaviour
    {
        int currentScore;
        int highScore;
        public int Highscore
        {
            get
            {
                CheckHighScore();
                return highScore;
            }
        }
        public int CurrentScore
        {
            get
            {
                return currentScore;
            }
            set
            {
                currentScore = value;
                CheckHighScore();
            }
        }

        void CheckHighScore()
        {
            object highScoreValue = null;
            SaveManager.instance.GetSavedInfo(0, out highScoreValue);
            if (highScoreValue == null)
            {
                highScore = currentScore;
                SaveManager.instance.SetInfo(0, currentScore);
            }
            else if (currentScore > (int)highScoreValue)
            {
                highScore = currentScore;
                SaveManager.instance.SetInfo(0, currentScore);
                SaveManager.instance.SaveFile();
            }
            else
            {
                highScore = (int)highScoreValue;
            }
        }
    }
}