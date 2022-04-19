using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class controller : MonoBehaviour
{
    public void Awake() {
        uiHelper.camera = Camera.main;

        researchTree.addNode("1", new List<string>() {});
        researchTree.addNode("2", new List<string>() {"1"});
        researchTree.addNode("3", new List<string>() {"1"});

        researchTree.processNodes();
    }
}
