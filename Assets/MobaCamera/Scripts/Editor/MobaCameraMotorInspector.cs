using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(MobaCameraMotor))]
public class MobaCameraMotorInspector : Editor {
    static public bool RequiredFoldout {
        get { return EditorPrefs.GetBool("MobaCameraInspector_RequiredFoldout", true); }
        set { EditorPrefs.SetBool("MobaCameraInspector_RequiredFoldout", value); }
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.Space();

        RequiredFoldout = GUILayout.Toggle(RequiredFoldout, "Required", EditorStyles.toolbarButton);
        if (RequiredFoldout) {
            DisplayRequiredGUI((MobaCameraMotor)target);
        }

        EditorGUILayout.Space();
    }

    static public void DisplayRequiredGUI(MobaCameraMotor target) {
        EditorGUI.BeginChangeCheck();

        var newPivot = EditorGUILayout.ObjectField("Pivot", target.Pivot, typeof(Transform), true) as Transform;
        var newOffset = EditorGUILayout.ObjectField("Offset", target.Offset, typeof(Transform), true) as Transform;
        var newCamera = EditorGUILayout.ObjectField("Camera", target.TargetCamera, typeof(Camera), true) as Camera;

        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(target, "Modify MobaCamera Requirements");
            target.Pivot = newPivot;
            target.Offset = newOffset;
            target.TargetCamera = newCamera;
            EditorUtility.SetDirty(target);
        }

        if (GUILayout.Button("Auto Generate", EditorStyles.miniButton)) {
            GenerateAuto(target);
        }

        if (GUILayout.Button("Generate from Main Camera", EditorStyles.miniButton)) {
            GenerateFromMainCamera(target);
        }
    }

    static public void GenerateAuto(MobaCameraMotor target) {
        if (target.Pivot == null) {
            target.Pivot = target.gameObject.transform;
        }

        if (target.Offset == null) {
            target.Offset = new GameObject("Offset").transform;
        }

        if (target.TargetCamera == null) {
            GameObject tempGO = new GameObject("MobaCamera");
            target.TargetCamera = tempGO.AddComponent<Camera>();
        }

        target.TargetCamera.transform.parent = target.Offset;
        target.Offset.parent = target.Pivot;

        target.TargetCamera.transform.localPosition = Vector3.zero;
        target.TargetCamera.transform.localRotation = Quaternion.identity;

        target.Offset.localPosition = Vector3.zero;
        target.Offset.localRotation = Quaternion.identity;
    }

    static public void GenerateFromMainCamera(MobaCameraMotor target) {
        target.TargetCamera = Camera.main;
        GenerateAuto(target);
    }
}
