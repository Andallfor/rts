using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;
using System.Text;

public class player : NetworkBehaviour
{
    public teamId team {get; private set;}
    private bool alreadyInit = false;
    public void Update() {
        if (isLocalPlayer && isClient) {
            mouseController.update();
            keyboardController.update();
        }

        if (isServer) {
            // testing
            if (Input.GetKeyUp("space") && !alreadyInit) {
                alreadyInit = true;
                Debug.Log("INIT GAME");
                mouseController.clearHighlight();
                // red
                List<cube> area = new cube(-14, 0, 14).nearby(6);
                teamController.registerArea(area, teamId.red);

                // blue
                area = new cube(14, 0, -14).nearby(6);
                teamController.registerArea(area, teamId.blue);
            }
        }
    }

    public override void OnStartClient() {
        if (isLocalPlayer) {
            mouseController.init();
            mouseController.setCameraAngle(0);
            master.localPlayer = this;
            this.team = (teamId) (GameObject.FindGameObjectsWithTag("Player").Length);
        }

        if (isLocalPlayer && isClientOnly) CmdRequestMapFromServer(this.gameObject);

        teamController.registerArea(new cube(0, 0, 0).nearby(20), teamId.none);
    }

    public void syncAction(hex h, string name, string args) {
        CmdSyncAction(h.pos.ToString(), h.level, name, args);
    }

    [Command]
    private void CmdSyncAction(string pos, int level, string name, string args) {
        RpcSyncAction(pos, level, name, args);
    }

    [ClientRpc]
    private void RpcSyncAction(string pos, int level, string name, string args) {
        cube c = new cube(pos);
        hex h = master.map[c][level];
        ITileAction ta = h.possibleActions.Find(x => x.name == name);
        ta.actuallyRunAction(h, ta.deserialzeArgs(args));
    }

    [Command]
    private void CmdRequestMapFromServer(GameObject sender) {
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
    private void TargetGenerateMap(NetworkConnection target, string map) {
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

            if (height <= -0.15 && master.registeredTiles.Find(x => x.name == "forest").canTileGenerate(c, 5)) {
                new hex(master.size, c, 5, new forest(), 0);
            }
        }
    }
}
