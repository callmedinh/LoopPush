using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class Pool : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int size;
        //Dung stack(first in last out) hoac queue (first in first out) deu duoc
        private Stack<GameObject> pool = new Stack<GameObject>();
        private void Awake()
        {
            for (int i = 0; i < size; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                pool.Push(obj);
            }
        }

        public GameObject GetPool()
        {
            GameObject obj;
            if (pool.Count < 0)
            {
                obj = Instantiate(prefab);
            }
            obj = pool.Pop();
            obj.SetActive(true);
            return obj;
        }

        public void ReturnPool(GameObject obj)
        {
            obj.SetActive(false);
            pool.Push(obj);
        }
    }
}