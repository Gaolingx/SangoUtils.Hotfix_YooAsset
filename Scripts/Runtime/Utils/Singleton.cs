using UnityEngine;

namespace SangoUtils.Patchs_YooAsset
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static readonly object syslock = new object(); // 线程锁
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syslock) //锁一下，避免多线程出问题
                    {
                        _instance = FindObjectOfType(typeof(T)) as T;

                        if (_instance == null)
                        {
                            GameObject obj = new GameObject("[" + typeof(T).FullName + "]");
                            obj.hideFlags = HideFlags.DontSave;
                            // obj.hideFlags = HideFlags.HideAndDontSave;
                            _instance = (T)obj.AddComponent(typeof(T));
                        }
                    }
                }

                return _instance;
            }
        }


        protected virtual void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            if (_instance == null)
            {
                _instance = this as T;
            }
            else
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }

}