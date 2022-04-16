using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class controller : MonoBehaviour
{
    private int range;
    private int maxHeight = 30;

    // TODO: add 3d terrain
    // TODO: redo how terrain is generated- think/google up a fancy system
    void Start()
    {
        cube center = new cube(0, 0, 0);
        List<cube> cubes = center.nearby(20);
        // generate ground
        foreach (cube c in cubes) {
            Vector2 v = c.position(1);
            float height = (float) NoiseS3D.Noise(v.x / 15f, v.y / 15f);

            if (height <= -0.35) {
                new hex(master.size, c, 0, new ocean());
            } else if (height >= 0.75) {
                new hex(master.size, c, 1, new grass());
                new hex(master.size, c, 5, new mountain());
            } else {
                new hex(master.size, c, 1, new grass());
            }
        }

        // generate features
        NoiseS3D.seed = UnityEngine.Random.Range(-1000, 1000);
        foreach (cube c in cubes) {
            Vector2 v = c.position(1);
            float height = (float) NoiseS3D.Noise(v.x / 10f, v.y / 10f);
            Vector3 v3 = new Vector3(c.q, c.r, 1);
            Vector3 _v3 = new Vector3(c.q, c.r, 5);

            if (height <= -0.15 && master.map[c].allowedToGenerate(5, 4)) {
                new hex(master.size, c, 5, new forest());
            }
        }
    }
}