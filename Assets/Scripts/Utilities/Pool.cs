using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class Pool : MonoBehaviour
    {
        [SerializeField] private List<PoolItem> poolItems;
        private Dictionary<BlockType, Stack<GameObject>> pool;
        private Dictionary<BlockType, GameObject> prefabLookup;

        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            pool = new Dictionary<BlockType, Stack<GameObject>>();
            prefabLookup = new Dictionary<BlockType, GameObject>();
            foreach (var item in poolItems)
            {
                var stack = new Stack<GameObject>();
                for (var i = 0; i < item.size; i++)
                {
                    var obj = Instantiate(item.prefab, transform);
                    var bc = obj.GetComponent<BlockController>();
                    if (bc == null) bc = obj.AddComponent<BlockController>();
                    bc.SetTypeBlock(item.blockType);
                    obj.name = item.prefab.name;
                    obj.SetActive(false);
                    stack.Push(obj);
                }

                pool[item.blockType] = stack;
                prefabLookup[item.blockType] = item.prefab;
            }
        }

        public GameObject GetPooledObject(BlockType blockType)
        {
            GameObject obj = null;
            if (pool.TryGetValue(blockType, out var pools))
            {
                if (pools.Count == 0)
                {
                    obj = Instantiate(prefabLookup[blockType], transform);
                    var bc = obj.GetComponent<BlockController>();
                    if (bc == null) bc = obj.AddComponent<BlockController>();
                    bc.SetTypeBlock(blockType);
                    return obj;
                }

                obj = pools.Pop();
                obj.SetActive(true);
            }
            else
            {
                return null;
            }

            return obj;
        }

        public void ReturnToPool(BlockType type, GameObject pooledObject)
        {
            if (!pool.ContainsKey(type))
            {
                Destroy(pooledObject);
                return;
            }

            pooledObject.SetActive(false);
            pool[type].Push(pooledObject);
        }

        public void ReturnListToPool(List<GameObject> list)
        {
            foreach (var pooledObject in list)
            {
                if (pooledObject == null) continue;
                var bc = pooledObject.GetComponent<BlockController>();
                if (bc == null)
                {
                    Destroy(pooledObject);
                    continue;
                }

                ReturnToPool(bc.blockType, pooledObject);
            }
        }

        [Serializable]
        public class PoolItem
        {
            public BlockType blockType;
            public GameObject prefab;
            public int size;
        }
    }
}