using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.EventManager;
using Util;

namespace Snake
{
    //Classe mae da cobra
    [RequireComponent(typeof(DisplaySnake))]
    public class Snake : MonoBehaviour
    {
        #region Variables
        public float speed; //Velocidade da cobra

        [HideInInspector] public List<Vector2Int> snakeBody { get; private set; }

        GlobalEnums.E_Direction desiredDirection = GlobalEnums.E_Direction.None;
        GlobalEnums.E_Direction lastDirection = GlobalEnums.E_Direction.None;

        int bodyPartsAdded;
        bool canMove;
        float moveTimer;

        DisplaySnake display;
        #endregion
        private void Awake()
        {
            display = GetComponent<DisplaySnake>();
        }

        private void Update()
        {
            if(canMove && speed > 0)
            {
                moveTimer += Time.deltaTime;
                if (moveTimer >= 1 / speed)
                {
                    moveTimer = 0;
                    Move();
                }
            }                
        }

        //Move a cobra
        void Move()
        {
            if (desiredDirection != GlobalEnums.E_Direction.None)
            {
                Vector2Int newSnakeHeadPos = Vector2Int.zero;

                newSnakeHeadPos = snakeBody[0] + GetVectorDirection(desiredDirection);

                Vector2Int tailPos = snakeBody[snakeBody.Count - 1];

                for (int i = snakeBody.Count - 1; i > 0; i--)
                {
                    snakeBody[i] = snakeBody[i - 1];
                }
                if (bodyPartsAdded > 0)
                {
                    snakeBody.Add(tailPos);
                    bodyPartsAdded--;
                }
                snakeBody[0] = newSnakeHeadPos;
                EventManager.BroadcastEvent("SnakeMoved", new Hashtable { { "Pos", newSnakeHeadPos } });
            }
            display.UpdateSprites(snakeBody, desiredDirection);
            lastDirection = desiredDirection;
        }        


        public void StartMoving()
        {
            canMove = true;
        }

        public void StopMoving()
        {
            canMove = false;
        }

        //Aumenta o numero de partes do corpo que devemos crescer
        public void Grow(Hashtable eventParam)
        {
            bodyPartsAdded++;
        }

        //Reinicia a cobra
        public void ResetSnake(Vector2Int pos)
        {
            canMove = false;
            if (snakeBody != null)
            {
                snakeBody.Clear();
            }
            snakeBody = new List<Vector2Int>();
            snakeBody.Add(pos);
            snakeBody.Add(new Vector2Int(pos.x, pos.y - 1));
            desiredDirection = GlobalEnums.E_Direction.None;
            lastDirection = GlobalEnums.E_Direction.None;
            display.UpdateSprites(snakeBody, GlobalEnums.E_Direction.Up);
        }

        #region Utility
        //Pega uma direcao e retorna o vetor da direcao
        Vector2Int GetVectorDirection(GlobalEnums.E_Direction direction)
        {
            switch (direction)
            {
                case GlobalEnums.E_Direction.Left:
                    return Vector2Int.left;

                case GlobalEnums.E_Direction.Right:
                    return Vector2Int.right;

                case GlobalEnums.E_Direction.Up:
                    return Vector2Int.up;

                case GlobalEnums.E_Direction.Down:
                    return Vector2Int.down;

                default:
                    return Vector2Int.zero;
            }
        }
        #endregion

        #region Messege Handlers
        //Recebe o evento de botao de movimentacao pressionado
        void MovementKeyPressed(Hashtable eventParam)
        {
            if (eventParam == null) { return; }
            if (eventParam.ContainsKey("Code"))
            {
                switch (eventParam["Code"])
                {
                    case "Left":
                        if (lastDirection != GlobalEnums.E_Direction.Right) { desiredDirection = GlobalEnums.E_Direction.Left; }
                        break;

                    case "Right":
                        if (lastDirection != GlobalEnums.E_Direction.Left) { desiredDirection = GlobalEnums.E_Direction.Right; }
                        break;

                    case "Up":
                        if (lastDirection != GlobalEnums.E_Direction.Down) { desiredDirection = GlobalEnums.E_Direction.Up; }
                        break;

                    case "Down":
                        if (lastDirection != GlobalEnums.E_Direction.Up) { desiredDirection = GlobalEnums.E_Direction.Down; }
                        break;
                }
            }
        }

        private void OnEnable()
        {
            EventManager.AddListener("MovementKeyPressed", MovementKeyPressed);
            EventManager.AddListener("SnakeGrow", Grow);
            EventManager.AddListener("SpecialKeyPressed", Grow);
        }

        private void OnDisable()
        {
            EventManager.RemoveListner("MovementKeyPressed", MovementKeyPressed);
            EventManager.RemoveListner("SnakeGrow", Grow);
            EventManager.RemoveListner("SpecialKeyPressed", Grow);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListner("MovementKeyPressed", MovementKeyPressed);
            EventManager.RemoveListner("SnakeGrow", Grow);
            EventManager.RemoveListner("SpecialKeyPressed", Grow);
        }
        #endregion
    }
}
