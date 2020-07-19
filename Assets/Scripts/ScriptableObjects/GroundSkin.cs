using UnityEngine;

namespace Snake
{
    [CreateAssetMenu(fileName = "Ground Skin", menuName = "Snake/Ground Skin", order = 4)]
    public class GroundSkin : ScriptableObject
    {
        public Sprite[] groundSprite;
    }
}
