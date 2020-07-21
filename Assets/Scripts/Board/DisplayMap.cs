using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
    //Mostra o mapa na tela
    public class DisplayMap : Display
    {
        List<SpriteRenderer> groundObjects = new List<SpriteRenderer>(); //Objetos que formam o mapa

        //Cria o mapa dizendo se o mapa anterior deve ser deletado
        public void CreateMap(bool rebuildIfCreated)
        {
            bool rebuild = false;
            if (rebuildIfCreated)
            {
                if(boardSize.x * boardSize.y != groundObjects.Count)
                {
                    while (groundObjects.Count > 0)
                    {
                        Destroy(groundObjects[groundObjects.Count - 1].gameObject);
                        groundObjects.RemoveAt(groundObjects.Count - 1);
                    }
                    rebuild = true;
                }   
            }
            else
            {
                if (groundObjects.Count > 0) //Caso ja exista um mapa criado e nao precisamos recrialo
                {
                    return;
                }
            }

            
            //Impede que tentemos acessar uma skin nao criada corretamente
            if (SkinController.instance.selectedSkin.groundSkin.groundSprite.Length == 0) { Debug.LogWarning("No ground sprite on selected skin."); return; }
            int index = 0;
            //Varrendo todo o tabuleiro
            for (int i = 0; i < boardSize.y; i++)
            {
                for (int j = 0; j < boardSize.x; j++)
                {
                    SpriteRenderer obj =  rebuild? CreateGroundObj() : groundObjects[index];
                    index++;
                    if (obj == null) { return; } //O objeto nao contem um sprite renderer entao nao devemos prosseguir

                    //Quinas
                    if ((i == 0 && j == 0) || (i == 0 && j == boardSize.x - 1) || (i == boardSize.y - 1 && j == 0) || (i == boardSize.y - 1 && j == boardSize.x - 1))
                    {
                        obj.sprite = SkinController.instance.selectedSkin.wallSkin.curveWallSprite;
                    }

                    //Bordas
                    else if (i == 0 || i == boardSize.y - 1 || j == 0 || j == boardSize.x - 1)
                    {
                        obj.sprite = SkinController.instance.selectedSkin.wallSkin.straightWallSprite;
                    }

                    //Chao
                    else
                    {
                        obj.sprite = SkinController.instance.selectedSkin.groundSkin.groundSprite[Random.Range(0, SkinController.instance.selectedSkin.groundSkin.groundSprite.Length)];
                    }
                    obj.transform.position = new Vector3(j - boardCenterDisplacement.x, i - boardCenterDisplacement.y, 0);
                }
            }

        }

        //Cria um novo objeto de cenario
        SpriteRenderer CreateGroundObj()
        {
            SpriteRenderer render = Instantiate(displayPrefab, transform).GetComponent<SpriteRenderer>();
            if (render == null) { Destroy(render.gameObject); Debug.LogWarning("No Sprite Render Found on object."); return null; }
            groundObjects.Add(render);
            return render;
        }
    }
}
