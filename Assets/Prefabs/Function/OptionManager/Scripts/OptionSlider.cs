using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OptionSlider : MonoBehaviour
{
    // Start is called before the first frame update
    private Slider slider;
    OptionManager optionManager;
    AudioManagerBGM audioManagerBGM;
    AudioManagerEffect audioManagerEffect;
    void Start()
    {
        slider = gameObject.GetComponent<Slider>();

        optionManager = FindObjectOfType<OptionManager>();
        audioManagerBGM = FindObjectOfType<AudioManagerBGM>();
        audioManagerEffect = FindObjectOfType<AudioManagerEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        print("speed : " + optionManager.speed);
        print("BGM : " + audioManagerBGM.speaker.volume);
        print("EffectVolume : " + audioManagerEffect.speaker.volume);
    }
    
    public void SetMusicVolume(){
        if(audioManagerBGM != null) audioManagerBGM.setVolume(slider.value);
    }
    public void SetEffectVolume(){
        if(audioManagerEffect != null) audioManagerEffect.setVolume(slider.value);
    }
    public void SetTimeScale(){
        if(slider != null && optionManager != null)  optionManager.speed = slider.value;
    }
    
}
