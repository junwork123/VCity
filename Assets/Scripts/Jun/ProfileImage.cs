using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileImage : MonoBehaviour
{
    public static ProfileImage Instance;
    public Sprite[] profileImg;
    private void Awake()
    {
        Instance = this;
    }
    public Sprite GetProfileImg(int _profileNum)
    {
        return profileImg[_profileNum];
    }
}
