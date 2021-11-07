using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSet : MonoBehaviour
{
    public static ImageSet Instance;
    public Sprite[] servicesImg;
    public Sprite[] profileImg;
    private void Awake()
    {
        Instance = this;
    }
    public Sprite GetServiceImg(int _serviceNum)
    {
        return servicesImg[_serviceNum];
    }
    public Sprite GetProfileImg(int _profileNum)
    {
        return profileImg[_profileNum];
    }
    public Sprite GetUserProfileImg()
    {
        return profileImg[DataManager.Instance.userCache.Character];
    }
}
