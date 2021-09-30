using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadObject : MonoBehaviour
{
    public List<GameObject> childObjects;
    public float radiusRange = 1.5f;


    // Start is called before the first frame update
    void Start()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>();
        for (int i = 0; i < transforms.Length; ++i)
        {
            if (transforms[i].gameObject.name.Contains("Button"))
            {
                transforms[i].localPosition = transform.up * radiusRange;
                childObjects.Add(transforms[i].gameObject);
            }
        }

        // Spread();
    }

    private void Update()
    {
        float halfAngle = Mathf.Atan2(1f, radiusRange);
        int count = childObjects.Count;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            childObjects[0].transform.RotateAround(transform.position, Vector3.back, (count - 1) * halfAngle);
    }

    void Spread()
    {
        /*
        범위 : spreadRange
        중심 축 : (x, y, z) 중 1, 기본 z축
        */

        int count = childObjects.Count;
        // 0.5f : 버튼 반지름
        float halfAngle = Mathf.Atan2(0.5f, radiusRange);
        for (int i = 0; i < count; ++i)
        {
            childObjects[i].transform.RotateAround(transform.position, Vector3.back, (count - 1) * halfAngle);
            // childObjects[i].transform.RotateAround(transform.position, Vector3.back, halfAngle * 2);
            childObjects[i].transform.localEulerAngles = Vector3.zero;
        }
    }
}
