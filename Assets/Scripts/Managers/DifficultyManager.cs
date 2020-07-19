using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
    public class DifficultyManager : MonoBehaviour
    {        enum E_DifficultyType { Progressive, Curve };
        [SerializeField] E_DifficultyType difficultyType = default;

        [SerializeField] float progressiveStart = default;
        [SerializeField] float progressiveIncrease = default;
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

