using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelReset : MonoBehaviour
{
    public ToggleGroup toggleGroup;
    public ToggleGroup toggleGroup2;
    public GameObject scrollview;
    // Start is called before the first frame update
    private void OnDisable()
    {
        if (toggleGroup != null){
            foreach (var item in toggleGroup.GetComponentsInChildren<Toggle>())
            {
                item.isOn = false;
            }
        }
        if (toggleGroup2 != null){
            foreach (var item in toggleGroup2.GetComponentsInChildren<Toggle>())
            {
                item.isOn = false;
            }
        }
        if (scrollview != null) scrollview.SetActive(false);
    }
}
