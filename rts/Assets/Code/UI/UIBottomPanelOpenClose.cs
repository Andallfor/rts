using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBottomPanelOpenClose : MonoBehaviour
{
    public static bool open = false;
    public RawImage arrow;
    public RectTransform panel, arrowR;

    public void openPanel() {
        panel.anchoredPosition3D = new Vector3(0, 40, 0);
        arrow.gameObject.transform.rotation = Quaternion.Euler(0, 0, 180);
        arrowR.anchoredPosition3D = new Vector3(0, 0, 0);
        open = true;
    }

    public void closePanel() {
        panel.anchoredPosition3D = new Vector3(0, -45, 0);
        arrow.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        arrowR.anchoredPosition3D = new Vector3(0, 2.5f, 0);
        open = false;
    }

    public void togglePanel() {
        if (open) closePanel();
        else openPanel();
    }
}
