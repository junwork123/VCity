using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Service : MonoBehaviour
{
    public static Service Instance;
    public int CharacterNum;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void SetCharcterNum(int _num) { CharacterNum = _num; }

    public void SetUserCharacter()
    {
        DataManager.Instance.userCache.Character = CharacterNum;
        GameManager.instance.SetPlayerModel((PlayerModelType)CharacterNum);
    }
}
