using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Service : MonoBehaviour
{
    public static Service Instance;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
