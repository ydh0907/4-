using Unity.Netcode;
using UnityEngine;

public class MonoSingleton<T> : NetworkBehaviour where T : NetworkBehaviour
{
    private static bool destroyed = false;
    private static T instance = null;

    /*protected virtual void Awake() {
        if(instance != null) {
            return;
        }
        T _instance = this as T;
        SingletonManager.Register(instance);

        instance = _instance;
    }*/
    public static T Instance
    {
        get 
        {
            if (destroyed)
            {
                return null;
            }

            if(instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if(instance == null)
                {
                    instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                }
            }
            return instance; 
        }
    }

    private void OnApplicationQuit()
    {
        //destroyed = true;
    }
}
