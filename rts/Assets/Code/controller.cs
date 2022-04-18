using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    public void Awake() {
        uiHelper.camera = Camera.main;
    }
}
