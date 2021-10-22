using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트가 생성되었을 때 Button 오브젝트들을 균일한 각도로 배치
/// </summary>
public class SpreadObject : MonoBehaviour
{
    public List<Transform> childTransforms;
    public float radiusRange = 1.5f;
    float angle;
    [Range(1, 100)]
    public float paddingPercent = 10f;


    private void Awake()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            childTransforms.Add(transform.GetChild(i));
        }
    }

    void OnEnable()
    {
        InitPosition();
        Spread();
    }

    void InitPosition()
    {
        // 위치 초기화 (0, radiusRange)
        for (int i = 0; i < childTransforms.Count; ++i)
        {
            childTransforms[i].localPosition = Vector3.up * radiusRange;
        }

    }

    void Spread()
    {
        // atan(y/x) : 버튼이 인접했을 때 각의 절반
        float buttonRadius = childTransforms[0].transform.localScale.x * 0.5f;
        // 10f padding 추가
        angle = Mathf.Atan2(buttonRadius, radiusRange) * Mathf.Rad2Deg * 2 * (1 + paddingPercent * 0.01f);

        int count = childTransforms.Count;
        for (int i = 0; i < count; ++i)
        {
            // N개일 때 초기위치 (N - 1) * angle * 0.5
            // 중심을 기준으로 angle만큼 회전하면서 배치
            childTransforms[i].transform.RotateAround(transform.position, Camera.main.transform.forward, (count - 1) * angle * 0.5f);
            childTransforms[i].transform.RotateAround(transform.position, Camera.main.transform.forward * (-1), angle * i);
            childTransforms[i].transform.localEulerAngles = Vector3.zero;
        }
    }
}
