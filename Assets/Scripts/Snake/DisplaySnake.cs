using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Snake
{
    public class DisplaySnake : Display
    {
        List<SpriteRenderer> displayedSprites = new List<SpriteRenderer>(); //Referencia para todos os sprites da cobra

        [SerializeField] Pool spritesPool = default; //Referencia para a piscina de objetos

        //Atualiza a cobra na tela
        public void UpdateSprites(List<Vector2Int> bodyParts, GlobalEnums.E_Direction headDirection)
        {
            if (bodyParts.Count == 0) { return; }

            //Garantindo que temos objetos suficientes
            while (bodyParts.Count > displayedSprites.Count)
            {
                GameObject obj = spritesPool.GetObjFromPool();
                SpriteRenderer render = obj.GetComponent<SpriteRenderer>();
                if (render == null) { Debug.LogWarning("No Sprite Render Found on pool object."); return; }
                displayedSprites.Add(render);
                obj.SetActive(true);
                obj.transform.SetParent(transform);
            }

            //Garantindo que nao temos mais objetos que precisamos
            while (bodyParts.Count < displayedSprites.Count)
            {
                spritesPool.ReturnObjToPool(displayedSprites[displayedSprites.Count - 1].gameObject);
                displayedSprites.RemoveAt(displayedSprites.Count - 1);
            }

            //Cabeca
            displayedSprites[0].transform.position = new Vector3(bodyParts[0].x - boardCenterDisplacement.x, bodyParts[0].y - boardCenterDisplacement.y, 0);
            displayedSprites[0].transform.rotation = RotationFromDirection(headDirection);
            if (displayedSprites[0].sprite == null)
            {
                displayedSprites[0].sprite = SkinController.instance.selectedSkin.snakeSkin.headSprite;
            }

            //Corpo e rabo
            for (int i = 1; i < bodyParts.Count; i++)
            {
                displayedSprites[i].transform.position = new Vector3(bodyParts[i].x - boardCenterDisplacement.x, bodyParts[i].y - boardCenterDisplacement.y, 0);
                SetBodySpriteAndRotation(bodyParts, i);
            }
        }

        void SetBodySpriteAndRotation(List<Vector2Int> bodyParts, int i)
        {
            GlobalEnums.E_Direction direction = GlobalEnums.E_Direction.None;

            //Calcula a direcao do sprite
            if(bodyParts[i].x < bodyParts[i-1].x)
            {
                direction = GlobalEnums.E_Direction.Right;
            }
            else if(bodyParts[i].x > bodyParts[i - 1].x)
            {
                direction = GlobalEnums.E_Direction.Left;
            }
            else if(bodyParts[i].y > bodyParts[i - 1].y)
            {
                direction = GlobalEnums.E_Direction.Down;
            }
            else if (bodyParts[i].y < bodyParts[i - 1].y)
            {
                direction = GlobalEnums.E_Direction.Up;
            }

            if (i != bodyParts.Count - 1)
            {
                //Seguindo Reto
                if ((bodyParts[i - 1].x < bodyParts[i].x && bodyParts[i + 1].x > bodyParts[i].x) || 
                    (bodyParts[i + 1].x < bodyParts[i].x && bodyParts[i - 1].x > bodyParts[i].x) || 
                    (bodyParts[i - 1].y < bodyParts[i].y && bodyParts[i + 1].y > bodyParts[i].y) || 
                    (bodyParts[i + 1].y < bodyParts[i].y && bodyParts[i - 1].y > bodyParts[i].y))
                {
                    displayedSprites[i].sprite = SkinController.instance.selectedSkin.snakeSkin.bodySprite;
                    displayedSprites[i].transform.rotation = RotationFromDirection(direction);
                }

                //Cima Esquerda ou Direita Baixo
                else if ((bodyParts[i - 1].y < bodyParts[i].y && bodyParts[i + 1].x < bodyParts[i].x) || (bodyParts[i + 1].y < bodyParts[i].y && bodyParts[i - 1].x < bodyParts[i].x))
                {
                    displayedSprites[i].sprite = SkinController.instance.selectedSkin.snakeSkin.bodyTurnSprite;
                    displayedSprites[i].transform.rotation = Quaternion.Euler(0, 0, -90);
                }

                //Cima - Direita ou Esquerda - Baixo
                else if ((bodyParts[i - 1].x > bodyParts[i].x && bodyParts[i + 1].y < bodyParts[i].y) || (bodyParts[i + 1].x > bodyParts[i].x && bodyParts[i - 1].y < bodyParts[i].y))
                {
                    displayedSprites[i].sprite = SkinController.instance.selectedSkin.snakeSkin.bodyTurnSprite;
                    displayedSprites[i].transform.rotation = Quaternion.Euler(0, 0, 0);
                }

                //Baixo - Esquerda ou Direita - Cima
                else if ((bodyParts[i - 1].x < bodyParts[i].x && bodyParts[i + 1].y > bodyParts[i].y) || (bodyParts[i + 1].x < bodyParts[i].x && bodyParts[i - 1].y > bodyParts[i].y))
                {
                    displayedSprites[i].sprite = SkinController.instance.selectedSkin.snakeSkin.bodyTurnSprite;
                    displayedSprites[i].transform.rotation = Quaternion.Euler(0, 0, -180);
                }

                //Baixo - Direita ou Esquerda - Cima
                else if ((bodyParts[i - 1].y > bodyParts[i].y && bodyParts[i + 1].x > bodyParts[i].x) || (bodyParts[i + 1].y > bodyParts[i].y && bodyParts[i - 1].x > bodyParts[i].x))
                {
                    displayedSprites[i].sprite = SkinController.instance.selectedSkin.snakeSkin.bodyTurnSprite;
                    displayedSprites[i].transform.rotation = Quaternion.Euler(0, 0, -270);
                }
            }
            //Rabo
            else
            {
                displayedSprites[i].sprite = SkinController.instance.selectedSkin.snakeSkin.tailSprite;
                displayedSprites[i].transform.rotation = RotationFromDirection(direction);
            }
        }

        Quaternion RotationFromDirection(GlobalEnums.E_Direction direction)
        {
            switch (direction)
            {
                case GlobalEnums.E_Direction.Up:
                    return Quaternion.Euler(0, 0, 0);

                case GlobalEnums.E_Direction.Down:
                    return Quaternion.Euler(0, 0, 180);

                case GlobalEnums.E_Direction.Left:
                    return Quaternion.Euler(0, 0, 90);

                case GlobalEnums.E_Direction.Right:
                    return Quaternion.Euler(0, 0, -90);

                default:
                    return Quaternion.Euler(0, 0, 0);
            }
        }
    }
}
