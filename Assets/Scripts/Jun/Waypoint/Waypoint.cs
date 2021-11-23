using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    #region 테스트 중
    public string uid;
    public bool isEnable = true;
    #endregion

    public Waypoint previousWaypoint;
    public Waypoint nextWaypoint;


    [Range(0f, 5f)]
    public float width = 2.5f;

    public List<Waypoint> branches;

    [Range(0f, 1f)]
    public float branchRatio = 0.5f;

    public Vector3 GetPosition()
    {
        //Waypoint 너비 기반으로 임의의 지점 반환
        Vector3 minBound = transform.position + transform.right * width / 2f;
        Vector3 maxBound = transform.position - transform.right * width / 2f;

        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }
    public float GetLength()
    {
        // Vector3 prev, next;
        // if (previousWaypoint == null)
        //     prev = transform.position;
        // else
        //     prev = previousWaypoint.transform.position;

        // if (nextWaypoint == null)
        //     next = transform.position;
        // else
        //     next = nextWaypoint.transform.position;
    
        //Vector3 prev = previousWaypoint == null ? GetPosition() : previousWaypoint.GetPosition();
        //Vector3 next = nextWaypoint == null ? GetPosition() : previousWaypoint.GetPosition();

        // return Vector3.Distance(prev, next);
        return 1;

    }
    public Waypoint DeepCopy()
    {
        Waypoint waypoints = new Waypoint();
        waypoints.uid = this.uid;
        waypoints.isEnable = this.previousWaypoint;
        waypoints.previousWaypoint = this.previousWaypoint;
        waypoints.nextWaypoint = this.nextWaypoint;
        waypoints.width = this.width;
        waypoints.branchRatio = this.branchRatio;
        waypoints.branches = new List<Waypoint>();
        foreach (Waypoint item in this.branches)
        {
            waypoints.branches.Add(item);
        }

        return waypoints;
    }
}
