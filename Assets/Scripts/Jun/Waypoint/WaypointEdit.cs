using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaypointEdit : EditorWindow
{
    [MenuItem("Tools/Waypoint Editor")]

    public static void Open()
    {
        GetWindow<WaypointEdit>();
    }

    public Transform waypointRoot;

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);
        EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));

        if (waypointRoot == null)
        {
            EditorGUILayout.HelpBox("Root transform must be selected. Please assing a root transform.", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            DrawButtons();
            EditorGUILayout.EndVertical();
        }
        obj.ApplyModifiedProperties();
    }

    void DrawButtons()
    {
        if (GUILayout.Button("Create Waypoint"))
        {
            CreateWayPoint();
        }

        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Waypoint>())
        {
            if (GUILayout.Button("Add Branch Waypoint"))
            {
                CreateBranch();
            }

            if (GUILayout.Button("Create Waypoint Befor"))
            {
                CreateWaypointBefore();
            }
            if (GUILayout.Button("Create Waypoint After"))
            {
                CreateWaypointAfter();
            }
            if (GUILayout.Button("Remove Wapoint"))
            {
                RemoveWaypoint();
            }
            if (GUILayout.Button("Connect current to First"))
            {
                Selection.activeGameObject.GetComponent<Waypoint>().nextWaypoint = waypointRoot.gameObject.GetComponentInChildren<Waypoint>();
                Selection.activeGameObject.GetComponent<Waypoint>().nextWaypoint.previousWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();
            }
        }
    }

    void CreateWayPoint()
    {
        GameObject waypointObject = new GameObject("Waypoint" + waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
        if (waypointRoot.childCount > 1)
        {
            waypoint.previousWaypoint = waypointRoot.GetChild(waypointRoot.childCount - 2).GetComponent<Waypoint>();
            waypoint.previousWaypoint.nextWaypoint = waypoint;

            waypoint.transform.position = waypoint.previousWaypoint.transform.position;
            waypoint.transform.forward = waypoint.previousWaypoint.transform.forward;
        }

        Selection.activeObject = waypoint.gameObject;
    }

    void CreateWaypointBefore()
    {
        GameObject waypointObject = new GameObject("Waypoint" + waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();

        Waypoint selectWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

        waypointObject.transform.position = selectWaypoint.transform.position;
        waypointObject.transform.forward = selectWaypoint.transform.forward;

        if (selectWaypoint.previousWaypoint != null)
        {
            newWaypoint.previousWaypoint = selectWaypoint.previousWaypoint;
            selectWaypoint.previousWaypoint.nextWaypoint = newWaypoint;
        }

        newWaypoint.nextWaypoint = selectWaypoint;
        selectWaypoint.previousWaypoint = newWaypoint;
        newWaypoint.previousWaypoint = selectWaypoint;//중간추가 ;

        newWaypoint.transform.SetSiblingIndex(selectWaypoint.transform.GetSiblingIndex());

        Selection.activeGameObject = newWaypoint.gameObject;

    }

    void CreateWaypointAfter()
    {
        GameObject waypointObject = new GameObject("Waypoint" + waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();

        Waypoint selectWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

        waypointObject.transform.position = selectWaypoint.transform.position;
        waypointObject.transform.forward = selectWaypoint.transform.forward;

        newWaypoint.previousWaypoint = selectWaypoint;



    }

    void RemoveWaypoint()
    {
        Waypoint seletedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

        if (seletedWaypoint.nextWaypoint != null)
        {
            seletedWaypoint.nextWaypoint.previousWaypoint = seletedWaypoint.previousWaypoint;
        }

        if (seletedWaypoint.previousWaypoint != null)
        {
            seletedWaypoint.previousWaypoint.nextWaypoint = seletedWaypoint.nextWaypoint;
            Selection.activeGameObject = seletedWaypoint.previousWaypoint.gameObject;
        }

        DestroyImmediate(seletedWaypoint.gameObject);
    }

    void CreateBranch()
    {
        GameObject waypointObject = new GameObject("Waypoint" + waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();

        Waypoint branchedFrom = Selection.activeGameObject.GetComponent<Waypoint>();
        branchedFrom.branches.Add(waypoint);

        waypoint.transform.position = branchedFrom.transform.position;
        waypoint.transform.forward = branchedFrom.transform.forward;

        Selection.activeGameObject = waypoint.gameObject;
    }
}
