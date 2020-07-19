using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.EventManager;

namespace Snake
{
    //Classe mae dos displays. Contem as funcionalidades comuns dos displays
    public class Display : MonoBehaviour
    {
        [SerializeField] protected GameObject displayPrefab;
        protected Vector2Int boardSize;
        protected Vector2 boardCenterDisplacement;

        private void SetBoardSize(Hashtable eventParam)
        {
            if (eventParam == null) { return; }
            if (eventParam.ContainsKey("Size"))
            {
                boardSize = (Vector2Int)eventParam["Size"];
                boardCenterDisplacement = boardSize / 2;
            }
        }

        private void OnEnable()
        {
            EventManager.AddListener("BoardSize", SetBoardSize);
        }

        private void OnDisable()
        {
            EventManager.RemoveListner("BoardSize", SetBoardSize);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListner("BoardSize", SetBoardSize);
        }
    }
}

