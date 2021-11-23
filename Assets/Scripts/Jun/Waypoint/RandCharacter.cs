using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandCharacter : MonoBehaviour
{
    public GameObject[] characterModels;

    private void Start()
    {
        SetRandCharacter();
        Destroy(this);
    }
    void SetRandCharacter()
    {
        int idx = Random.Range(0, characterModels.Length);
        for (int i = 0; i < characterModels.Length; i++)
        {
            characterModels[i].SetActive(i == idx);
        }
    }
}
