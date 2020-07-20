using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.EventManager;
using Unity.Jobs;
using Unity.Collections;

namespace Snake
{
    //Contem e controla as informacoes do tabuleiro
    public class Board : MonoBehaviour
    {
        public enum E_HitObject { None, SnakeBody, SnakeHead, Wall, Food }
        [Header("Settings")]
        public Vector2Int boardSize;

        [Header("References")]
        [SerializeField] Snake snake = default;
        [SerializeField] Food food = default;

        bool[,] snakeBodyMatrix;
        Vector2Int lastTailPosition;


        //Verifica se a posicao passada corresponde a algum elemento interativo do jogo
        public E_HitObject CheckIfHitObject(Vector2Int pos)
        {
            //Comida
            if (pos == food.foodPosition)
            {
                return E_HitObject.Food;
            }

            //Bordas
            if (pos.y == 0 || pos.y == boardSize.y - 1 || pos.x == 0 || pos.x == boardSize.x - 1)
            {
                return E_HitObject.Wall;
            }

            //Corpo da Cobra
            //Verifica se a posicao pos na matriz de corpo da cobra e verdadeira
            if (snakeBodyMatrix[pos.x, pos.y] == true)
            {
                return E_HitObject.SnakeBody;
            }

            //Cabeca da cobra
            if (pos == snake.snakeBody[0])
            {
                return E_HitObject.SnakeHead;
            }

            return E_HitObject.None;
        }

        //Encontra uma posicao vazia no tabuleiro e se nao encontrar retorna um vetor 0
        public Vector2Int GetEmptyPosition()
        {
            //Pega um valor aleatorio no mapa
            Vector2Int pos = new Vector2Int(Random.Range(1, boardSize.x - 1), Random.Range(1, boardSize.y - 1));

            //Partindo dessa posicao percorremos a linha tentando encontrar um espaco vazio. 
            //Se nao encontramos na linha, buscamos na linha de cima e repetimos ate que um valor tenha sido encontrado ou tenhamos varrido todo o mapa e nao encontrado nenhum espaco vazio.
            //Como o X e Y iniciais sao aleatorios, quando chegamos no fim do tabuleiro, damos a volta.
            for (int i = pos.y; i < boardSize.y + pos.y; i++)
            {
                for (int j = pos.x; j < boardSize.x + pos.x; j++)
                {
                    Vector2Int testPos = new Vector2Int(j < boardSize.x ? j : j - boardSize.x, i < boardSize.y ? i : i - boardSize.y);
                    if (CheckIfHitObject(testPos) == E_HitObject.None)
                    {
                        return testPos;
                    }
                }
            }
            return Vector2Int.zero;
        }

        //Atualiza a matriz que contem o corpo da cobra
        public void UpdateSnakeBodyMatrix()
        {
            if (snakeBodyMatrix == null)
            {
                ResetSnakeBodyMatrix();
            }
            if (lastTailPosition != snake.snakeBody[snake.snakeBody.Count - 1])
            {
                snakeBodyMatrix[lastTailPosition.x, lastTailPosition.y] = false;
                lastTailPosition = snake.snakeBody[snake.snakeBody.Count - 1];
            }
            snakeBodyMatrix[snake.snakeBody[1].x, snake.snakeBody[1].y] = true;
        }

        //Reseta a matriz que contem os elemetos do corpo da cobra
        public void ResetSnakeBodyMatrix()
        {
            snakeBodyMatrix = new bool[boardSize.x, boardSize.y];
            for (int i = 1; i < snake.snakeBody.Count; i++)
            {
                snakeBodyMatrix[snake.snakeBody[i].x, snake.snakeBody[i].y] = true;
            }
            lastTailPosition = snake.snakeBody[snake.snakeBody.Count - 1];
        }
    }
}
