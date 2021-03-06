﻿using System.Collections;
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
        [SerializeField] UIManager ui = default;
        [SerializeField] ScoreManager score = default;

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
                    ui.RemoveMsg();
                    ui.UpdateScores(score.CurrentScore, score.Highscore);
                    break;
                case E_GameState.GameStart:
                    ResetGame();
                    ui.DisplayMsg("Press a movement key to start the game.\n(use the arrow keys to move)");
                    ui.UpdateScores(score.CurrentScore, score.Highscore);
                    break;
                case E_GameState.Death:
                    snake.StopMoving();
                    ui.DisplayMsg("You Died!\n\nPress a movement key to try again.", Color.red);
                    ui.UpdateScores(score.CurrentScore, score.Highscore);
                    break;
                case E_GameState.Win:
                    snake.StopMoving();
                    ui.DisplayMsg("You Won!\n\nPress a movement key to try again.", Color.green);
                    ui.UpdateScores(score.CurrentScore, score.Highscore);
                    break;
            }
        }

        //Reinicia o jogo
        void ResetGame()
        {
            score.CurrentScore = 0;
            ui.UpdateScores(score.CurrentScore, score.Highscore);
            EventManager.BroadcastEvent("BoardSize", new Hashtable { { "Size", board.boardSize } });
            groundDisplay.CreateMap(true);
            snake.ResetSnake(board.boardSize / 2);
            snake.speed = difficulty.GetDifficultyValue(score.CurrentScore);
            board.ResetSnakeBodyMatrix();
            board.UpdateSnakeBodyMatrix();
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

        //Verifica se a cobra bateu em algum lugar ou comeu uma fruta
        void SnakeMoved(Hashtable eventParam)
        {
            if (eventParam == null) { return; }
            if (eventParam.ContainsKey("Pos"))
            {
                board.UpdateSnakeBodyMatrix();
                switch (board.CheckIfHitObject((Vector2Int)eventParam["Pos"]))
                {
                    case Board.E_HitObject.Food:
                        EventManager.BroadcastEvent("SnakeGrow", null);
                        score.CurrentScore++;
                        ui.UpdateScores(score.CurrentScore, score.Highscore);
                        snake.speed = difficulty.GetDifficultyValue(score.CurrentScore);
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
            score.CurrentScore++;
            ui.UpdateScores(score.CurrentScore, score.Highscore);
            snake.speed = difficulty.GetDifficultyValue(score.CurrentScore);
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

