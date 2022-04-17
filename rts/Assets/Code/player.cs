using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Text;

public class player : NetworkBehaviour
{
    public void Update() {
        if (!isLocalPlayer) return;

        
    }

    public override void OnStartClient() {
        if (!isLocalPlayer || !isClientOnly) return;

        CmdRequestMapFromServer(this.gameObject);
    }

    [Command]
    public void CmdRequestMapFromServer(GameObject sender) {
        // unable to send such a large arg at once so split into multi calls
        string data = master.serializeMap();
        int size = ASCIIEncoding.Unicode.GetByteCount(data);

        // send 16kib per call (this is arb, idk the max amount)
        int index = 0;
        string[] tiles = data.Split('\n');
        for (int i = 0; i <= size; i += 16384) {
            StringBuilder sb = new StringBuilder();
            int stringSize = 0;
            // TODO: dont do this
            // fill up string to send up to 16 kib
            while (stringSize < 16384 && index != tiles.Length - 1) {
                string toAdd = tiles[index];
                stringSize += ASCIIEncoding.Unicode.GetByteCount(toAdd);
                sb.AppendLine(toAdd);

                index++;
            }
            TargetGenerateMap(sender.GetComponent<NetworkIdentity>().connectionToClient, sb.ToString());
        }
    }

    [TargetRpc]
    public void TargetGenerateMap(NetworkConnection target, string map) {
        master.deserializeMap(map);
    }

    // TODO: add 3d terrain
    // TODO: redo how terrain is generated- think/google up a fancy system
    public override void OnStartServer() {
        if (master.map.Count != 0) return; // dont regen terrain if a new client connects
        // generate terrain
        cube center = new cube(0, 0, 0);
        List<cube> cubes = center.nearby(20);
        // generate ground
        foreach (cube c in cubes) {
            Vector2 v = c.position(1);
            float height = (float) NoiseS3D.Noise(v.x / 15f, v.y / 15f);

            if (height <= -0.35) {
                new hex(master.size, c, 0, new ocean(), 0);
            } else if (height >= 0.75) {
                new hex(master.size, c, 1, new grass(), 0);
                new hex(master.size, c, 5, new mountain(), 0);
            } else {
                new hex(master.size, c, 1, new grass(), 0);
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
                new hex(master.size, c, 5, new forest(), 0);
            }
        }
    }
}
