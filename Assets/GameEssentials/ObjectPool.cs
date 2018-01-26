using System.Collections.Generic;
using UnityEngine;
namespace Pool
{
    public class ObjectPool<T> where T : PooledObject
    {
        List<T> objectPool = new List<T>();
        T pooledObject;
        Transform parent = null;

        public ObjectPool(T _type, Transform _parent, int _amount = 10)
        {
            pooledObject = _type;
            parent = _parent;
            for (int i = 0; i < _amount; i++)
            {      
                NewPooledObject(parent);
            }
        }

        public T GetPooledObject(Transform _trans)
        {
            foreach (T t in objectPool)
            {
                if (!t.isActiveAndEnabled)
                {
                    t.Activate(_trans);
                    return t;
                }
            }

            T newObj = NewPooledObject(parent);
            newObj.Activate(_trans);
            return newObj;
        }

        public T GetPooledObject(Vector3 _pos, Quaternion _rot)
        {
            foreach (T t in objectPool)
            {
                if (!t.isActiveAndEnabled)
                {
                    t.Activate(_pos, _rot);
                    return t;
                }
            }

            T newObj = NewPooledObject(parent);
            newObj.Activate(_pos, _rot);
            return newObj;
        }

        T NewPooledObject(Transform _parent)
        {
            T o = Object.Instantiate(pooledObject);
            o.gameObject.SetActive(false);
            o.transform.parent = _parent;
            objectPool.Add(o);
            return o;
        }

        public T RemoveFromPool(T _object)
        {
            for (int i = 0; i < objectPool.Count; i++)
            {
                if (objectPool[i] == _object)
                {
                    objectPool.RemoveAt(i);
                    return _object;
                }
            }
            return null;
        }

        public void ClearPool()
        {
            for (int i = objectPool.Count; i >= 0; i--)
            {
                Object.Destroy(objectPool[i]);
            }
            objectPool.Clear();
        }

    }
    public class PooledObject : MonoBehaviour, IPooledObject
    {
        public virtual void Activate(Transform _trans)
        {
            gameObject.SetActive(true);
            transform.position = _trans.position;
            transform.rotation = _trans.rotation;
        }
        public virtual void Activate(Vector3 _pos, Quaternion _rot)
        {
            gameObject.SetActive(true);
            transform.position = _pos;
            transform.rotation = _rot;
        }
        public virtual void ReturnToPool()
        {
            gameObject.SetActive(false);
        }
    }
    public interface IPooledObject 
    {
        void Activate(Transform _trans);
        void Activate(Vector3 _pos, Quaternion _rot);
        void ReturnToPool();
    }
}