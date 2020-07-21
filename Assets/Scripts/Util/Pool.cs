using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util.PoolSystem
{
    //Sistema simples de object pooling que pode ser utilizado em diversas situações
    public class Pool : MonoBehaviour
    {
        [Tooltip("Object that will be Instantiated")]
        [SerializeField] GameObject poolObj = default;

        [Tooltip("How many objects will be instantiated at start")]
        [SerializeField] int inicialPoolSize;

        [Tooltip("How many objects will be instantiated when the pool is empty")]
        [SerializeField] int poolIncrement;

        List<GameObject> pool;

        public Pool(int poolStartSize, int increment)
        {
            inicialPoolSize = poolStartSize;
            poolIncrement = increment;
        }

        // Start is called before the first frame update
        void Start()
        {
            AddPoolObjects(inicialPoolSize);
        }

        //Recebe um objeto e adicona na piscina
        public void ReturnObjToPool(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            pool.Add(obj);
        }

        //Pega um objeto da piscina
        public GameObject GetObjFromPool()
        {
            if (pool == null) { AddPoolObjects(inicialPoolSize); }
            if (pool.Count <= 0)
            {
                AddPoolObjects(poolIncrement);
            }
            GameObject obj = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
            return obj;
        }

        //Adiciona elementos a pisicna de objetos
        void AddPoolObjects(int number)
        {
            if (pool == null) { pool = new List<GameObject>(); }
            for (int i = 0; i < number; i++)
            {
                GameObject obj = Instantiate(poolObj, transform);
                obj.SetActive(false);
                pool.Add(obj);
            }
        }

    }
}