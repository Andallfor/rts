using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public static class mouseController
{
    private static Vector3 lastMousePosition, lastMousePanPosition;
    private static Vector2 cameraDefaultPosition = new Vector2(40, 40);
    private static hex currentHighlight;
    private static List<hex> affectedHighlights = new List<hex>();
    public static UITileInfo uiti;
    public static hex selectedHex;
    private static float highlightHeightIncrease = 0.5f;
    private static int currentCameraAngleIndex = 0;
    private static float[] angles = new float[12] {0, 30, 60, 90, 120, 150, 180, 210, 240, 270, 300, 330};

    public static void init() {
        uiti = GameObject.FindGameObjectWithTag("ui/right/tileInfo").GetComponent<UITileInfo>();
    }

    public static void update() {
        // mouse is hovering over a tile, raise it to "highlight" it
        // TODO: implement actual highlighting?

        if (lastMousePosition != Input.mousePosition && selectedHex is null) {
            if (!EventSystem.current.IsPointerOverGameObject()) {
                // check if we are hitting anything
                Ray r = uiHelper.camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(r, out hit)) {
                    tileGameObjectInfo ti = hit.transform.gameObject.GetComponent<tileGameObjectInfo>();
                    if (!(currentHighlight is null)) {
                        if (ti.position != currentHighlight.pos || ti.level != currentHighlight.level) {
                            clearHighlight();
                            highlight(ti.parent);
                        }
                    } else highlight(ti.parent);
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

        // selecting tile
        if (Input.GetMouseButtonUp(0) && currentHighlight is hex && !EventSystem.current.IsPointerOverGameObject()) {
            // check if were still mousing over the same tile, if not then deselect
            Ray r = uiHelper.camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool stillSelecting = false;
            if (Physics.Raycast(r, out hit)) {
                tileGameObjectInfo ti = hit.transform.gameObject.GetComponent<tileGameObjectInfo>();
                if (ti.level == currentHighlight.level && ti.position == currentHighlight.pos) stillSelecting = true;
            }

            if (!stillSelecting) closeTileInfoMenu();
            else {
                // if we have yet to select anything
                if (selectedHex is null) {
                    // select the tile
                    selectedHex = currentHighlight;
                    uiti.openMenu(selectedHex);
                } else { // we have already selected something
                    // if there is a tile below the current tile, select it
                    // if there is not, deselect it
                    hex below = master.map[selectedHex.pos].levels.FirstOrDefault(x => x.Key < selectedHex.level).Value;
                    if (below is null) closeTileInfoMenu();
                    else {
                        selectedHex = below;
                        uiti.openMenu(selectedHex);
                    }
                }

                if (selectedHex is hex) {
                    clearHighlight();
                    highlight(selectedHex);
                }
            }
        }
    }

    public static void closeTileInfoMenu() {
        selectedHex = null;
        uiti.closeMenu();
        clearHighlight();
    }

    public static void highlight(hex h) {
        currentHighlight = h;

        // if we highlight something, make sure we also highlight everything above to prevent meshes intersecting each other
        column c = master.map[h.pos];
        foreach (KeyValuePair<int, hex> level in c.levels) {
            if (level.Key >= h.level) {
                affectedHighlights.Add(level.Value);
                level.Value.model.transform.position += new Vector3(0, highlightHeightIncrease, 0);
            }
        }
    }

    public static void clearHighlight() {
        foreach (hex he in affectedHighlights) {
            he.model.transform.position -= new Vector3(0, highlightHeightIncrease, 0);
        }

        affectedHighlights = new List<hex>();
    }

    public static void setCameraAngle(float angle) {
        uiHelper.camera.transform.position = new Vector3(
            Mathf.Cos(Mathf.Deg2Rad * angle) * cameraDefaultPosition.x,
            cameraDefaultPosition.y,
            Mathf.Sin(Mathf.Deg2Rad * angle) * cameraDefaultPosition.x);
        uiHelper.camera.transform.LookAt(Vector3.zero);
    }
}
