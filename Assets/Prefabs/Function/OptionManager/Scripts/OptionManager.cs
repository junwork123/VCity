using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    private void Awake()
    {
        speed = Time.timeScale;
        gameObject.transform.localScale = new Vector3(0, 0, 0);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }


    public void Pause()
    {
        AudioManagerEffect AMeffect = FindObjectOfType<AudioManagerEffect>();
        AMeffect.playEffect(Sound.Pause);

        if (Time.timeScale != 0)
        {
            // iTween.ScaleTo(gameObject, iTween.Hash(
            //     "vecter", new Vector3(1,1,1),
            //     "time", 0.01f,
            //     "easetype", iTween.EaseType.easeOutBounce
            // ));
            //Invoke("Freeze",0.5f);
            OpenPanel();
        }
        else
        {
            // iTween.ScaleTo(gameObject, iTween.Hash(
            //     "vecter", new Vector3(0,0,0),
            //     "time", 0.01f,
            //     "easetype", iTween.EaseType.easeOutBounce
            // ));
            ClosePanel();
        }

    }
    public void ClosePanel()
    {
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        Time.timeScale = speed;
    }
    public void OpenPanel()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        Time.timeScale = 0;
    }
}
