using UnityEngine;

namespace Snake
{
    [CreateAssetMenu(fileName = "Wall Skin", menuName = "Snake/Wall Skin", order = 5)]
    public class WallSkin : ScriptableObject
    {
        public Sprite straightWallSprite;
        public Sprite curveWallSprite;
    }
}
