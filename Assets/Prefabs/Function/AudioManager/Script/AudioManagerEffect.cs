using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Sound 열거형 모음
public enum Sound
{   
    #region 
    HighScore, //
    MidScore, //
    LowScore, //
    Pause,
    SelectPlayer, // 
    GrabItem,
    DropItem,
    Loading,
    Die,
    PopingBtn,
    OverBtn,
    #endregion
}

// 상황마다 발생하는 짧은 효과음을 관리하는 매니저 
public class AudioManagerEffect : MonoBehaviour
{
    // AudioCilp 멤버 변수 모음
    #region 
    public AudioClip highScore;
    public AudioClip midScore;
    public AudioClip lowScore;
    public AudioClip pause;
    public AudioClip selectPlayer;
    public AudioClip grabItem;
    public AudioClip dropItem;
    public AudioClip loading;
    public AudioClip die;

    public AudioClip popingBtn;
    public AudioClip overBtn;
    #endregion
    public AudioSource speaker;
    void Start()
    {   
        DontDestroyOnLoad(gameObject);
        speaker.Stop();
        speaker.volume = 1f;
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

    public void setVolume(int _amount){
        speaker.volume = _amount;
    }
    public void playEffect(Sound sound){
        switch (sound)
        {
            case Sound.HighScore:
                speaker.PlayOneShot(highScore);
                break;
            case Sound.MidScore:
                speaker.PlayOneShot(midScore);
                break;
            case Sound.LowScore:
                speaker.PlayOneShot(lowScore);
                break;
            case Sound.Pause:
                speaker.PlayOneShot(pause);
                break;
            case Sound.SelectPlayer:
                speaker.PlayOneShot(selectPlayer);
                break;
            case Sound.GrabItem:
                speaker.PlayOneShot(grabItem);
                break;
            case Sound.DropItem:
                speaker.PlayOneShot(dropItem);
                break;
            case Sound.Loading:
                speaker.PlayOneShot(loading);
                break;
            case Sound.Die:
                speaker.PlayOneShot(die);
                break;
            case Sound.PopingBtn:
                speaker.PlayOneShot(popingBtn);
                break;
            case Sound.OverBtn:
                speaker.PlayOneShot(overBtn);
                break;
        }
    }
    public void onClickPause(){
        speaker.PlayOneShot(pause);
    }

    public void setVolume(float _amount){
        speaker.volume = _amount;
    }
}
