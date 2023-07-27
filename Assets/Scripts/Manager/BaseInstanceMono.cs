using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInstanceMono<T> : MonoBehaviour where T : BaseInstanceMono<T>
{
    public static T Instance;
    public virtual void Awake()
    {
        if (Instance != null)
        {
            if (Instance != this)
                Destroy(this.gameObject);
            return;
        }
        Instance = this as T;
    }
}
