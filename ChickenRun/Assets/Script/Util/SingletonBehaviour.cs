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
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
