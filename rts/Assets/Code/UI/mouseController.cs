using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class mouseController : MonoBehaviour
{
    public Vector3 lastMousePosition, lastMousePanPosition;
    public Vector2 cameraDefaultPosition = new Vector2(40, 40);
    public tileGameObjectInfo currentHighlight;
    public List<hex> affectedHighlights = new List<hex>();
    public float highlightHeightIncrease = 0.5f;
    public int currentCameraAngleIndex = 0;
    private float[] angles = new float[12] {0, 30, 60, 90, 120, 150, 180, 210, 240, 270, 300, 330};
    public void Start() {
        setCameraAngle(0);
    }

    public void Update() {
        // mouse is hovering over a tile, raise it to "highlight" it
        // TODO: implement actual highlighting?

        if (lastMousePosition != Input.mousePosition) {
            if (!EventSystem.current.IsPointerOverGameObject()) {
                // check if we are hitting anything
                Ray r = uiHelper.camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(r, out hit)) {
                    tileGameObjectInfo ti = hit.transform.gameObject.GetComponent<tileGameObjectInfo>();
                    if (!(currentHighlight is null)) {
                        if (ti.position != currentHighlight.position || ti.level != currentHighlight.level) {
                            clearHighlight();
                            highlight(ti);
                        }
                    } else highlight(ti);
                }
            }

            lastMousePosition = Input.mousePosition;
        }

        // zoom
        if (Input.mouseScrollDelta.y != 0) {
            float newSize = uiHelper.camera.orthographicSize - Input.mouseScrollDelta.y * 2f;
            uiHelper.camera.orthographicSize = Mathf.Min(Mathf.Max(newSize, 5f), 50f);
        }

        // camera pan
        if (Input.GetMouseButtonDown(1)) {
            lastMousePanPosition = Input.mousePosition;
        } else if (Input.GetMouseButton(1)) {
            if (Input.mousePosition != lastMousePanPosition) {
                Vector3 difference = ((lastMousePanPosition - Input.mousePosition) / 15f) * (uiHelper.camera.orthographicSize / 20f); // TODO: dont hardcode this
                lastMousePanPosition = Input.mousePosition;

                uiHelper.camera.transform.position += uiHelper.camera.transform.up * difference.y;
                uiHelper.camera.transform.position += uiHelper.camera.transform.right * difference.x;
            }
        }

        // switch camera angle
        if (Input.GetKeyDown("d")) {
            currentCameraAngleIndex += 1;
            if (currentCameraAngleIndex >= angles.Length) currentCameraAngleIndex = 0;

            setCameraAngle(angles[currentCameraAngleIndex]);
        }
        if (Input.GetKeyDown("a")) {
            currentCameraAngleIndex -= 1;
            if (currentCameraAngleIndex < 0) currentCameraAngleIndex = angles.Length - 1;

            setCameraAngle(angles[currentCameraAngleIndex]);
        }
    }

    private void highlight(tileGameObjectInfo ti) {
        currentHighlight = ti;

        // if we highlight something, make sure we also highlight everything above to prevent meshes intersecting each other
        column c = master.map[ti.position];
        foreach (KeyValuePair<int, hex> level in c.levels) {
            if (level.Key >= ti.level) {
                affectedHighlights.Add(level.Value);
                level.Value.model.transform.position += new Vector3(0, highlightHeightIncrease, 0);
            }
        }
    }

    private void clearHighlight() {
        foreach (hex he in affectedHighlights) {
            he.model.transform.position -= new Vector3(0, highlightHeightIncrease, 0);
        }

        affectedHighlights = new List<hex>();
    }

    private void setCameraAngle(float angle) {
        uiHelper.camera.transform.position = new Vector3(
            Mathf.Cos(Mathf.Deg2Rad * angle) * cameraDefaultPosition.x,
            cameraDefaultPosition.y,
            Mathf.Sin(Mathf.Deg2Rad * angle) * cameraDefaultPosition.x);
        uiHelper.camera.transform.LookAt(Vector3.zero);
    }
}
