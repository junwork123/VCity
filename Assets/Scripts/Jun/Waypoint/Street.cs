using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Street : MonoBehaviour
{
    // 도로명 주소 데이터 객체
    public string streetName;
    public List<Waypoint> ways;
    // Start is called before the first frame update

    public Waypoint src;
    public Waypoint dst;


    void Start()
    {
        ways = new List<Waypoint>();

        foreach (Waypoint item in GetComponentsInChildren<Waypoint>())
        {
            ways.Add(item);
        }

    }
    public List<Waypoint> GetShortestPath(Waypoint src, Waypoint dst)
    {
        Dictionary<List<Waypoint>, float> paths = GetPaths(src.GetComponent<Waypoint>(), dst.GetComponent<Waypoint>());
        float minLength = float.MaxValue;
        List<Waypoint> minPath = new List<Waypoint>();
        int i = 0;
        foreach (List<Waypoint> item in paths.Keys)
        {
            Debug.Log(i++ + "번째 경로 길이 : " + paths[item]);
            if (paths[item] < minLength)
            {
                minLength = paths[item];
                minPath = item;
            }
        }
        return minPath;
    }
    public Dictionary<List<Waypoint>, float> GetPaths(Waypoint src, Waypoint dst)
    {
        // 반환 형식은 경로 & 총 길이 
        Dictionary<List<Waypoint>, float> result = new Dictionary<List<Waypoint>, float>();
        List<string> visited = new List<string>();

        // 도착지부터 역으로 생각해서 
        //List<Waypoints> endChunk = 
        // 갈림길이 없으면 같은 도로니까 거기까지는 전처리 해두면 좋겠다
        List<Waypoint> path = new List<Waypoint>();
        path.Add(src);
        DFS(path, 0, src);
        if (result.Keys.Count == 0) Debug.Log("경로 탐색 결과가 없습니다");
        return result;

        void DFS(List<Waypoint> path, float length, Waypoint src)
        {
            // 이미 포함된 길이거나, 
            // 루트를 모두 돌아도 찾을 수 없다면
            // 종료한다.
            if (visited.Contains(src.uid)) return;

            // 경로 및 비용 업데이트
            length = length + src.GetLength();
            path.Add(src);
            visited.Add(src.uid);

            if (src.uid == dst.uid)
            {
                Debug.Log("dst founded : " + src.uid);
                result.Add(path, length);
                return;
            }
            else
            {
                Debug.Log("visited : " + src.uid);
                if (src.branches != null)
                {
                    foreach (Waypoint item in src.branches)
                    {
                        Debug.Log(src.uid + " Branch : " + item.uid);
                        DFS(new List<Waypoint>(path), length, item);
                    }
                }

                if (src.nextWaypoint != null)
                {
                    DFS(path, length, src.nextWaypoint);
                }
            }
        }
    }
}

