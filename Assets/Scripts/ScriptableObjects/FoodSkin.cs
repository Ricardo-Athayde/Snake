using UnityEngine;

namespace Snake
{
    [CreateAssetMenu(fileName = "Food Skin", menuName = "Snake/Food Skin", order = 3)]
    public class FoodSkin : ScriptableObject
    {
        public Sprite[] foodSprites;
    }
}
