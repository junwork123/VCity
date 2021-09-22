using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 씬마다 발생하는 긴 배경음악을 담당하는 매니저
public class AudioManagerBGM : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip menuCilp;
    public AudioClip forestCilp;
    public AudioClip introClip;

    public AudioSource speaker;
    void Start()
    {   
        speaker.Stop();
        speaker.volume = 0.5f;
        speaker.loop = true;
        switch (SceneManager.GetActiveScene().buildIndex)
        {   
            // 메인 메뉴
            case 0:
                speaker.clip = menuCilp;
                speaker.Play();
                break;
            // 포레스트 맵
            case 1:
                speaker.clip = forestCilp;
                speaker.Play();
                break;
            // 인트로
            case 3:
                speaker.clip = introClip;
                speaker.Play();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Mute(){
        if(speaker.mute == false){
            speaker.mute = true;
        }
        else{
            speaker.mute = false;
        }
            
    }

    public void ChangeBGM(int _num){

    }

    public void setVolume(float _amount){
        speaker.volume = _amount;
    }
}
