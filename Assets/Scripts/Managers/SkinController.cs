using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
    //Carrega e mantem a referencia par as akins do jogo
    public class SkinController : MonoBehaviour
    {
        enum E_Skins { Basic, Normal }
        [SerializeField] E_Skins skin = default;

        [HideInInspector]
        public Skin selectedSkin;

        //Singleton
        public static SkinController instance;
        private void Awake()
        {
            if (instance == null) 
            {
                instance = this;
                LoadSkinResources();
            }
            else
            {
                Debug.LogWarning("Duplicate singleton found.");
                DestroyImmediate(this);
            }
        }

        //Carrega a skin desejada na pasta resources. Isso e feito para que apenas a skin desejada seja carregada na memoria
        void LoadSkinResources()
        {
            selectedSkin = Resources.Load<Skin>("Skins/" + skin.ToString());
        }
    }
}
