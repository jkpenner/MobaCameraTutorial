using UnityEngine;
using System.Collections;

public class MobaCameraMotor : MonoBehaviour {
    private Transform _pivot = null;
    private Transform _offset = null;
    private Camera _camera = null;

    public Transform Pivot {
        get { return _pivot; }
        set { _pivot = value; }
    }

    public Transform Offset {
        get { return _offset; }
        set { _offset = value; }
    }

    public Camera TargetCamera {
        get { return _camera; }
        set { _camera = value; }
    }

    private void Awake() {
        if (_pivot == null || _offset == null || _camera == null) {
            string missing = "";
            if (_pivot == null) {
                missing += "Pivot";
                this.enabled = false;
            }

            if (_offset == null) {
                missing += (missing == "" ? "" : ", ") + "Offset";
                this.enabled = false;
            }

            if (_camera == null) {
                missing += (missing == "" ? "" : ", ") + "Camera";
                this.enabled = false;
            }

            Debug.LogWarning("Moba_Camera Requirements Missing: " + missing +
                ". Add missing objects to the requirement tab under the Moba_camera script in the Inspector.");
        } else {
            if (Offset.parent != Pivot) {
                Offset.parent = Pivot;
            }

            if (TargetCamera.transform.parent != Offset) {
                TargetCamera.transform.parent = Offset;
            }
        }
    }
}
