using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager
{
    // initial pooling count
    public const int POOL_COUNT = 5;

    #region Pool
    class Pool
    {
        public GameObject Origin { get; private set; }
        public Transform Root { get; set; }

        Stack<PoolableObject> _poolStack = new Stack<PoolableObject>();

        public void Init(GameObject origin, int count = PoolManager.POOL_COUNT)
        {
            if (origin == null)
            {
                Debug.Log("GameObject cannot be null.");
                return;
            }

            Origin = origin;
            Root = new GameObject().transform;
            Root.name = $"{origin.name}_Root";

            for (int i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        PoolableObject Create()
        {
            if (Origin == null)
                return null;

            GameObject go = Object.Instantiate<GameObject>(Origin);
            go.name = Origin.name;
            return go.AddMissingComponent<PoolableObject>();
        }

        public void Push(PoolableObject poolable)
        {
            if (poolable == null)
                return;

            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            poolable.IsUsing = false;

            _poolStack.Push(poolable);
        }

        public PoolableObject Pop(Transform parent)
        {
            PoolableObject poolable;
            if (_poolStack.Count > 0)
                poolable = _poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);

            // DontDestroyOnLoad 하위로 가는 것을 막음
            if (parent == null)
                poolable.transform.parent = Managers.Scene.CurrentScene.transform;

            poolable.transform.parent = parent;
            poolable.IsUsing = true;

            return poolable;
        }
    }
    #endregion

    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root;

    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    public void Create(GameObject origin, int count = POOL_COUNT)
    {
        Pool pool = new Pool();
        pool.Init(origin, count);
        pool.Root.parent = _root.transform;

        _pool.Add(origin.name, pool);
    }

    public void Push(PoolableObject poolable)
    {
        string name = poolable.gameObject.name;
        if (_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
        }

        _pool[name].Push(poolable);
    }

    public PoolableObject Pop(GameObject origin, Transform parent = null)
    {
        if (_pool.ContainsKey(origin.name) == false)
            Create(origin);

        return _pool[origin.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if (_pool.ContainsKey(name) == false)
            return null;

        return _pool[name].Origin;
    }

    public void Clear()
    {
        foreach (Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }
}
