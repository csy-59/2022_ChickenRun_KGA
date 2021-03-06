using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<T>();
                if(instance == null)
                {
                    GameObject go = new GameObject();
                    instance = go.AddComponent<T>();
                }

                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    private void Awake()
    {
        if(instance != null)
        {
            if(instance.gameObject != gameObject)
            {
                Destroy(gameObject);
            }
        }

        instance = GetComponent<T>();
        DontDestroyOnLoad(instance.gameObject);
    }
}
