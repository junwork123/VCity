using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OptionToggle : MonoBehaviour
{
    // Start is called before the first frame update
    private Toggle toggle;
    OptionManager optionManager;
    AudioManagerBGM audioManagerBGM;
    AudioManagerEffect audioManagerEffect;
    float volumeBGM;
    float volumeEffect;

    void Start()
    {
        toggle = GetComponent<Toggle>();

        optionManager = FindObjectOfType<OptionManager>();
        audioManagerBGM = FindObjectOfType<AudioManagerBGM>();
        audioManagerEffect = FindObjectOfType<AudioManagerEffect>();

        volumeBGM = audioManagerBGM.speaker.volume;
        volumeEffect = audioManagerEffect.speaker.volume;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void PauseMusic(){
        audioManagerBGM.speaker.Pause();
    }
    public void PauseEffectMusic(){
        audioManagerEffect.speaker.Pause();
    }
    public void MuteMusic(){
        if(audioManagerBGM != null) {
            if(audioManagerBGM.speaker.isPlaying == true) audioManagerBGM.speaker.Pause();
            else audioManagerBGM.speaker.Play();
        }
    }
    public void MuteEffectSound(){
        if(audioManagerEffect != null) {
            if(audioManagerEffect.speaker.volume != 0) audioManagerEffect.speaker.volume = 0;
            else audioManagerEffect.speaker.volume = 100;
        }
    }
}
