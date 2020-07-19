using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.EventManager;

namespace Snake
{
    //Classe que controla o fluxo do jogo
    public class GameManager : MonoBehaviour
    {
        public enum E_GameState { Playing, Death, Win, GameStart }
        E_GameState gState;
        E_GameState gameState
        {
            get
            {
                return gState;
            }
            set
            {
                gState = value;
                ChangeGameState(value);
            }
        }

        [SerializeField] DisplayMap groundDisplay= default;
        [SerializeField] Snake snake = default;
        [SerializeField] Board board = default;
        [SerializeField] Food food = default;
        [SerializeField] DifficultyManager difficulty = default;
        [SerializeField] ScreenMsg screenMesseges = default;
        int eatenFoodNumber;

        // Start is called before the first frame update
        void Start()
        {
            gameState = E_GameState.GameStart;
        }

        //Muda o estado do jogo tomando tomando as decisoes necessarias
        void ChangeGameState(E_GameState state)
        {
            switch(state)
            {
                case E_GameState.Playing:
                    snake.StartMoving();
                    screenMesseges.RemoveMsg();
                    break;
                case E_GameState.GameStart:
                    ResetGame();
                    screenMesseges.DisplayMsg("Press a movement key to start the game.\n(use the arrow keys to move)");
                    break;
                case E_GameState.Death:
                    snake.StopMoving();
                    screenMesseges.DisplayMsg("You Died!\n\nPress a movement key to try again.", Color.red);
                    break;
                case E_GameState.Win:
                    snake.StopMoving();
                    screenMesseges.DisplayMsg("You Won!\n\nPress a movement key to try again.", Color.green);
                    break;
            }
        }

        //Reinicia o jogo
        void ResetGame()
        {
            eatenFoodNumber = 0;
            EventManager.BroadcastEvent("BoardSize", new Hashtable { { "Size", board.boardSize } });
            groundDisplay.CreateMap(true);
            snake.ResetSnake(board.boardSize / 2);
            snake.speed = difficulty.GetDifficultyValue(eatenFoodNumber);
            food.PlaceFood(board.GetEmptyPosition());            
        }

        //Reposiciona a nova comida e verifica se vencemos o jogo
        public void FooodEaten()
        {            
            Vector2Int newFoodPos = board.GetEmptyPosition();
            if (newFoodPos != Vector2Int.zero)
            {
                food.PlaceFood(newFoodPos);
            }
            else
            {
                gameState = E_GameState.Win;
            }
        }

        //Verifica se a aobra bateu em algum lugar ou comeu uma fruta
        void SnakeMoved(Hashtable eventParam)
        {
            if (eventParam == null) { return; }
            if (eventParam.ContainsKey("Pos"))
            {
                switch (board.CheckIfHitObject((Vector2Int)eventParam["Pos"]))
                {
                    case Board.E_HitObject.Food:
                        EventManager.BroadcastEvent("SnakeGrow", null);
                        eatenFoodNumber++;
                        snake.speed = difficulty.GetDifficultyValue(eatenFoodNumber);
                        FooodEaten();
                        break;
                    case Board.E_HitObject.SnakeBody:
                        gameState = E_GameState.Death;
                        break;
                    case Board.E_HitObject.Wall:
                        gameState = E_GameState.Death;
                        break;
                }
            }
        }

        //Recebe a mensagem se uma tecla foi pressionada
        void MovementKeyPressed(Hashtable eventParam)
        {
            if (eventParam == null) { return; }
            if (eventParam.ContainsKey("Code"))
            {
                switch (gameState)
                {
                    case E_GameState.GameStart:
                        gameState = E_GameState.Playing;
                        break;
                    case E_GameState.Win:
                        gameState = E_GameState.GameStart;
                        break;
                    case E_GameState.Death:
                        gameState = E_GameState.GameStart;
                        break;
                }
            }
        }

        void SpecialKeyPressed(Hashtable eventParam)
        {
            eatenFoodNumber++;
            snake.speed = difficulty.GetDifficultyValue(eatenFoodNumber);
        }

        private void OnEnable()
        {
            EventManager.AddListener("SnakeMoved", SnakeMoved);
            EventManager.AddListener("MovementKeyPressed", MovementKeyPressed);
            EventManager.AddListener("SpecialKeyPressed", SpecialKeyPressed);
        }

        private void OnDisable()
        {
            EventManager.RemoveListner("SnakeMoved", SnakeMoved);
            EventManager.RemoveListner("MovementKeyPressed", MovementKeyPressed);
            EventManager.AddListener("SpecialKeyPressed", SpecialKeyPressed);

        }

        private void OnDestroy()
        {
            EventManager.RemoveListner("SnakeMoved", SnakeMoved);
            EventManager.RemoveListner("MovementKeyPressed", MovementKeyPressed);
            EventManager.AddListener("SpecialKeyPressed", SpecialKeyPressed);
        }
    }
}

