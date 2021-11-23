using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    public GameObject PedestrianPrefab;
    public int pedestrianToSpawn;
    public Street street;
    Waypoint[] childs;

    // Start is called before the first frame update
    void Start()
    {
        childs = street.GetComponentsInChildren<Waypoint>();

        GameObject obj = Instantiate(PedestrianPrefab);
        int idx = Random.Range(0, transform.childCount - 1);
        Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);
        obj.transform.position = pos;
        obj.transform.SetParent(this.transform);
        obj.GetComponent<WaypointNavi>().currentWaypoint = childs[idx];

        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        int count = 1;
        while (count < pedestrianToSpawn)
        {
            GameObject obj = Instantiate(PedestrianPrefab);
            int idx = Random.Range(0, childs.Length - 1);
            Vector3 pos = new Vector3(childs[idx].transform.position.x, 0, childs[idx].transform.position.z);
            obj.transform.position = pos;
            obj.transform.SetParent(this.transform);
            obj.GetComponent<WaypointNavi>().currentWaypoint = childs[idx];
            yield return new WaitForEndOfFrame();
            count++;
        }
    }

}
