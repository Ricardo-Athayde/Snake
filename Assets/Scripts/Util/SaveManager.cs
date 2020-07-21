using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Util.SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        SaveObject saveObj;

        //Singleton
        public static SaveManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogWarning("Duplicate singleton found.");
                DestroyImmediate(this);
            }
        }

        //Carrega o save do persistent data path
        public void LoadSave()
        {
            string path = Path.Combine(Application.persistentDataPath, "save.sav");
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream reader = new FileStream(path, FileMode.Open);
                saveObj = formatter.Deserialize(reader) as SaveObject;
                reader.Close();
            }
            else
            {
                saveObj = new SaveObject();
                saveObj.fields = new Dictionary<int, object>();
                SaveFile();
            }
        }


        //salva as informacoes guardadas em disco
        public void SaveFile()
        {
            string path = Path.Combine(Application.persistentDataPath, "save.sav");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream writer = new FileStream(path, FileMode.Create);
            formatter.Serialize(writer, saveObj);
            writer.Close();
        }

        //Retorna uma informacao salva baseado no id
        public void GetSavedInfo(int infoId, out object value)
        {
            if (saveObj == null)
            {
                LoadSave();
            }
            if (saveObj.fields.ContainsKey(infoId))
            {
                value = saveObj.fields[infoId];
            }
            else
            {
                value = null;
            }
        }

        //Retorna uma informacao salva baseado no id
        public object GetSavedInfo(int infoId)
        {
            object value = null;
            GetSavedInfo(infoId, out value);
            return value;
        }

        //Guanda uma informacao para salvar
        public void SetInfo(int infoId, object value)
        {
            if (value.GetType().IsSerializable)
            {
                if (saveObj == null)
                {
                    LoadSave();
                }
                if (saveObj.fields == null) { saveObj.fields = new Dictionary<int, object>(); }

                if (saveObj.fields.ContainsKey(infoId))
                {
                    saveObj.fields[infoId] = value;
                }
                else
                {
                    saveObj.fields.Add(infoId, value);
                }
            }
            else
            {
                Debug.LogWarning("infoId: " + infoId + " value type is not serializable and will not be saved");
            }
        }
    }
}
