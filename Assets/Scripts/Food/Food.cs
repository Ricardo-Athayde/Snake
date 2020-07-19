using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
    //Classe mae da comida
    [RequireComponent(typeof(FoodDisplay))]
    public class Food : MonoBehaviour
    {
        public Vector2Int foodPosition { get; private set; } //Posicao da comida no tabuleiro
        FoodDisplay display; //O display da comida

        private void Awake()
        {
            display = GetComponent<FoodDisplay>();
        }

        //Coloca a comida no local especificado e atualiza o display
        public void PlaceFood(Vector2Int pos)
        {
            foodPosition = pos;
            display.UpdateDisplay(foodPosition);
        }
    }
}
