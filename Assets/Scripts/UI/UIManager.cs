using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Snake
{
    //Classe simples que controla mensagens na tela
    public class UIManager : MonoBehaviour
    {
        [SerializeField] Text txt = default;
        [SerializeField] Image panel = default;
        [SerializeField] Text scoreText = default;
        [SerializeField] Text highscoreText = default;

        public void DisplayMsg(string msg, Color color)
        {
            panel.gameObject.SetActive(true);
            txt.gameObject.SetActive(true);
            txt.text = msg;
            txt.color = color;
        }
        public void DisplayMsg(string msg)
        {
            DisplayMsg(msg, Color.white);
        }

        public void RemoveMsg()
        {
            panel.gameObject.SetActive(false);
            txt.gameObject.SetActive(false);
        }

        public void UpdateScores(int score, int highScore)
        {
            scoreText.text = "Score: " + score;
            highscoreText.text = "Highscore: " + highScore;
        }
    }
}