using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
    //Classe responsavel pelo controle da dificuldade do jogo
    public class DifficultyManager : MonoBehaviour
    {                
        enum E_DifficultyType { Progressive, Curve };

        [Header("Type")]
        [Tooltip("How shoud the difficulty be calculated")]
        [SerializeField] E_DifficultyType difficultyType = default;

        [Header("Progressive")]
        [Tooltip("Initial value for the progressive difficulty")]
        [SerializeField] float progressiveStart = default;

        [Tooltip("Incremental value for the progressive difficulty")]
        [SerializeField] float progressiveIncrease = default;

        [Header("Curve")]
        [Tooltip("Difficulty curve")]
        [SerializeField] AnimationCurve curveIncrease = default;
        
        public float GetDifficultyValue(float value)
        {
            switch (difficultyType)
            {
                case E_DifficultyType.Progressive:
                    return progressiveIncrease * value + progressiveStart;

                case E_DifficultyType.Curve:
                    return curveIncrease.Evaluate(value);

                default:
                    return 0;
            }
        }
    }
}

