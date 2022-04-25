using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class keyboardController
{
    private static Dictionary<teamId, Color> teamColorOffset = new Dictionary<teamId, Color>() {
        {teamId.none, new Color(0, 0, 0, 0)},
        {teamId.red, new Color(0.35f, 0, 0, 0)},
        {teamId.blue, new Color(0, 0, 0.35f, 0)}};
    
    private static Dictionary<string, List<Color>> defaultColors = new Dictionary<string, List<Color>>();

    public static bool isTeamOverlayOn {get; private set;}

    public static void update()
    {
        // toggle map overlay
        if (Input.GetKeyUp("z")) {
            if (isTeamOverlayOn) {
                isTeamOverlayOn = false;
                foreach (KeyValuePair<cube, column> kvp in master.map) {
                    hex h = kvp.Value.levels.Values.First();
                    MeshRenderer mr = h.model.GetComponent<MeshRenderer>();
                    // clear color offset
                    for (int i = 0; i < mr.materials.Length; i++) {
                        mr.materials[i].color = defaultColors[h.name][i];
                    }
                }
            } else {
                isTeamOverlayOn = true;
                foreach (KeyValuePair<cube, column> kvp in master.map) {
                    hex h = kvp.Value.levels.Values.First();
                    MeshRenderer mr = h.model.GetComponent<MeshRenderer>();

                    // register the default colors if need be
                    if (!defaultColors.ContainsKey(h.name)) {
                        List<Color> dc = new List<Color>();
                        foreach (Material m in mr.materials) dc.Add(m.color);
                        defaultColors[h.name] = dc;
                    }

                    // apply color offset
                    for (int i = 0; i < mr.materials.Length; i++) {
                        mr.materials[i].color += teamColorOffset[teamController.tileKey[h.pos]];
                    }
                }
            }
        }
    }
}
