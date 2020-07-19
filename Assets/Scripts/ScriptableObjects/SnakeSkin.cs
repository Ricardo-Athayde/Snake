using UnityEngine;

namespace Snake
{
    [CreateAssetMenu(fileName = "Snake Skin", menuName = "Snake/Snake Skin", order = 2)]
    public class SnakeSkin : ScriptableObject
    {
        public Sprite headSprite;
        public Sprite bodySprite;
        public Sprite bodyTurnSprite;
        public Sprite tailSprite;
    }
}
