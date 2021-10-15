using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 오브젝트가 생성되었을 때 Button 오브젝트들을 균일한 각도로 배치
/// </summary>
public class SpreadObject : MonoBehaviour
{
    public List<GameObject> childObjects;
    public float radiusRange = 1.5f;
    float angle;
    [Range(1, 100)]
    public float paddingPercent = 10f;


    void OnEnable()
    {

        Spread();
    }

    void Spread()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>();
        for (int i = 0; i < transforms.Length; ++i)
        {
            if (transforms[i].gameObject.name.Contains("Button"))
            {
                transforms[i].localPosition = Vector3.up * radiusRange;
                childObjects.Add(transforms[i].gameObject);
            }
        }

        // atan(y/x) : 버튼이 인접했을 때 각의 절반
        float buttonRadius = childObjects[0].transform.localScale.x * 0.5f;
        // 10f padding 추가
        angle = Mathf.Atan2(buttonRadius, radiusRange) * Mathf.Rad2Deg * 2 * 1 + paddingPercent * 0.01f;

        int count = childObjects.Count;
        for (int i = 0; i < count; ++i)
        {
            // N개일 때 초기위치 (N - 1) * angle * 0.5
            // 중심을 기준으로 angle만큼 회전하면서 배치
            childObjects[i].transform.RotateAround(transform.position, Vector3.forward, (count - 1) * angle * 0.5f);
            childObjects[i].transform.RotateAround(transform.position, Vector3.back, angle * i);
            childObjects[i].transform.localEulerAngles = Vector3.zero;
        }
    }
}
