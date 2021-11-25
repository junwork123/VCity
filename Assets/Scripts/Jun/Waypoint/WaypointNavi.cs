using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavi : MonoBehaviour
{
    public CharacterNavigationController controller;
    public Waypoint currentWaypoint;
    public Waypoint dst;

    public Street street;
    public List<Waypoint> path;
    public int direction = 0;

    private void Awake()
    {
        direction = Random.Range(0, 2);
        controller = GetComponent<CharacterNavigationController>();
    }
    void Start()
    {

        //path = street.GetShorestPath(currentWaypoint, dst);
    }

    void Update()
    {
        MoveOrigin();
        //MovePath();
    }
    void MovePath()
    {
        Debug.Log("현재 주행중인 웨이포인트 : " + currentWaypoint.uid);
        if (controller.reachedDestination)
        {
            if (path.Count != 0)
            {
                currentWaypoint = path[0];
                path.RemoveAt(0);
                controller.SetDestination(currentWaypoint.GetPosition());
            }


        }
    }
    void MoveOrigin()
    {
        if (controller.reachedDestination)
        {
            bool shouldBranch = false;
            if (currentWaypoint == null)
            {
                currentWaypoint = street.ways[Random.Range(0, street.ways.Count)];
            }
            if (currentWaypoint.branches != null && currentWaypoint.branches.Count > 0)
                shouldBranch = Random.Range(0f, 1f) <= currentWaypoint.branchRatio ? true : false;

            if (shouldBranch)
            {
                currentWaypoint = currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Count - 1)];
            }
            else
            {
                if (direction == 0)
                {
                    if (currentWaypoint.nextWaypoint != null)
                        currentWaypoint = currentWaypoint.nextWaypoint;
                    else
                    {
                        currentWaypoint = currentWaypoint.previousWaypoint;
                        direction = 1;
                    }
                }

                else if (direction == 1)
                {
                    if (currentWaypoint.previousWaypoint != null)
                        currentWaypoint = currentWaypoint.previousWaypoint;
                    else
                    {
                        currentWaypoint = currentWaypoint.nextWaypoint;
                        direction = 0;
                    }
                }

                controller.SetDestination(currentWaypoint.transform.position);
            }
        }
    }
}
