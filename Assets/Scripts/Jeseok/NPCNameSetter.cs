using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using TMPro;

public class NPCNameSetter : MonoBehaviour
{
    TextMeshPro nameTMP;
    Interactionable interactionable;

    // Start is called before the first frame update
    void Start()
    {
        nameTMP = GetComponent<TextMeshPro>();
        interactionable = GetComponentInParent<Interactionable>();

        StringBuilder sb = new StringBuilder();
        switch (interactionable.objectType)
        {
            case NPCType.LIFE:
                sb.Append("생활");
                break;
            case NPCType.STUDY:
                sb.Append("학업");
                break;
            case NPCType.JOB:
                sb.Append("취업");
                break;
            case NPCType.ETC:
                sb.Append("기타");
                break;
        }
        nameTMP.SetText(sb.ToString());
    }
}
