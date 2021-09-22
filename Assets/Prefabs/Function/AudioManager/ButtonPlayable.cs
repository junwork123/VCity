using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPlayable : MonoBehaviour
{   
    AudioManagerEffect AMeffect;
    private void Awake() {
        AMeffect = FindObjectOfType<AudioManagerEffect>();

        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((eventData) => { 
            OnMouseOver(); 
        });
        trigger.triggers.Add(entryEnter); 

        EventTrigger.Entry entryDown = new EventTrigger.Entry();
        entryDown.eventID = EventTriggerType.PointerDown;
        entryDown.callback.AddListener((eventData) => { 
            OnMouseDown(); 
        });

        trigger.triggers.Add(entryDown); 
    }
    // Start is called before the first frame update
    private void OnMouseDown() {
        AMeffect.playEffect(Sound.PopingBtn);
 
    }
    private void OnMouseOver() {
        AMeffect.playEffect(Sound.OverBtn);
    }
}
