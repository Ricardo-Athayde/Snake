using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.EventManager;

namespace Snake
{
    //Mostra a comida na tela
    public class FoodDisplay : Display
    {
        SpriteRenderer food; //Referencia para o sprite renderer que estamos utilizando para mostrar a comida
        public void UpdateDisplay(Vector2Int foodPosition)
        {
            //Caso o objeto de display da comida ainda nao tenha sido criado, instanciamos um novo
            if (food == null)
            {
                food = Instantiate(displayPrefab, transform).GetComponent<SpriteRenderer>();
                if (food == null) { Debug.LogWarning("No Sprite Render Found on object."); return; }
            }

            //Impede que tentemos acessar uma skin nao criada corretamente
            if(SkinController.instance.selectedSkin.foodSkin.foodSprites.Length == 0) { Debug.LogWarning("No food sprite on selected skin."); return; }

            //Colocamos o sprite na posicao correta e atualizamos o sprite
            food.sprite = SkinController.instance.selectedSkin.foodSkin.foodSprites[Random.Range(0, SkinController.instance.selectedSkin.foodSkin.foodSprites.Length)];
            food.transform.position = new Vector3(foodPosition.x - boardCenterDisplacement.x, foodPosition.y - boardCenterDisplacement.y, 0);
        }        
    }
}

