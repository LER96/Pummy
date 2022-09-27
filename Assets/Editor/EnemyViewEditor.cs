using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyView))]
public class EnemyViewEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyView fow = (EnemyView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);

        Vector3 pointA = fow.DirFromAngle(-fow.viewAngle, false);
        Vector3 pointB = fow.DirFromAngle(fow.viewAngle, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + pointA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + pointB * fow.viewRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in fow.visibleTargets)
        {
            Handles.DrawLine(fow.transform.position, visibleTarget.position);
        }
    }
}
