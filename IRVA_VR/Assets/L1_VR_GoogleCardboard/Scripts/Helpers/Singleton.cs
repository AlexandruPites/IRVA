using UnityEngine;

namespace L1_VR_GoogleCardboard.Scripts.Helpers
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static bool HasInstance => _instance != null;
    
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();
                }
                return _instance;
            }
        }
    
        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this as T;
        }
    }
}