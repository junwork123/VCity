using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class ScreenshotM : MonoBehaviour {

    string path;
    
    private void Start() {
         //path = "./ScreenShot/";
         //path = Application.dataPath+"/ScreenShot/";
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.F12)) Snapshot();
    }

    public void Snapshot() {
        StartCoroutine(ScreenshotAsFile());
    }

    // public IEnumerator ScreenshotAsTexture() {
    //     yield return new WaitForEndOfFrame();
    //     Texture2D img = ScreenCapture.CaptureScreenshotAsTexture();
    //     Rect rect = new Rect(0, 0, img.width, img.height);
    //     ui_Image.sprite = Sprite.Create(img, rect, Vector2.one * .5f);
    // }

    public IEnumerator ScreenshotAsFile() {
        yield return new WaitForEndOfFrame();
        path = "./ScreenShot/";
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            Directory.CreateDirectory(path);
        }
        string name = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        print("screenshot saved");
        print(path + name);
        ScreenCapture.CaptureScreenshot(path + name);
    }
    // void ScreenshotTaken(Texture2D image) { 
    //     console.text += "\nScreenshot has been taken and is now saving..."; 
    //     screenshot.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(.5f, .5f)); screenshot.color = Color.white; ui.alpha = 1; 
    // }
    public void openSNS(){
         Application.OpenURL("https://www.instagram.com/");
    }
}
