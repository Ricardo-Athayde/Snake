using UnityEngine;

namespace Snake
{
    [CreateAssetMenu(fileName = "Skin", menuName = "Snake/Skin", order = 1)]
    public class Skin : ScriptableObject
    {
        public SnakeSkin snakeSkin;
        public FoodSkin foodSkin;
        public GroundSkin groundSkin;
        public WallSkin wallSkin;
    }
}

