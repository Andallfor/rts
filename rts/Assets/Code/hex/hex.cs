using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// flat top
public class hex {
    public static readonly float[] angles = new float[6] {0, 60, 120, 180, 240, 300};
    public float size = 1;
    public cube pos {
        get => _pos;
        set {
            _pos = value;
            Vector2 v = value.position(size);
            worldPos = new Vector3(v.x, (float) level / 4f, v.y);
        }
    }

    public Vector3 worldPos {get; private set;}
    public int level {get; private set;}
    public teamId team {get; private set;}
    public resources inventory {get; private set;}

    private cube _pos;
    private float width, height, vertDist, horzDist;
    /* #region values to retrieve from tile */
    private ITile tile;
    public List<ITileAction> possibleActions {get => tile.possibleActions;}
    public string name {get => tile.name;}
    public tileType type {get => tile.type;}
    public int levelHeight {get => tile.levelHeight;}
    public float health {get => tile.health;}
    public float defense {get => tile.defense;}
    public float shield {get => tile.shield;}
    public float maxHealth {get => tile.maxHealth;}
    public float maxDefense {get => tile.maxDefense;}
    public float maxShield {get => tile.maxShield;}
    /* #endregion */
    public GameObject model;
    
    public hex(float size, cube position, int level, ITile tile, teamId team) {
        this.size = size;
        this.level = level;
        width = 2f * size;
        height = Mathf.Sqrt(3) * size;
        vertDist = height;
        horzDist = width * (3f / 4f);
        pos = position;

        this.instTile(tile);

        master.addHex(this, false);
    }

    public hex(string name, teamId team, int level, float health, float defense, float shield, float size, cube pos) {
        this.size = size;
        this.level = level;
        width = 2f * size;
        height = Mathf.Sqrt(3) * size;
        vertDist = height;
        horzDist = width * (3f / 4f);
        this.pos = pos;

        this.instTile(master.registeredTiles.Find(x => x.name == name));

        master.addHex(this, false);

        tile.forceSetHealth(health);
        tile.forceSetDefense(defense);
        tile.forceSetShield(shield);
    }

    private void instTile(ITile tile) {
        this.tile = tile;
        this.model = GameObject.Instantiate(tile.modelPrefab);

        model.transform.position = worldPos;
        model.transform.parent = GameObject.FindGameObjectWithTag("generatedObjects/map").transform;
        MeshCollider mc = model.AddComponent(typeof(MeshCollider)) as MeshCollider;
        tileGameObjectInfo ti = model.AddComponent(typeof(tileGameObjectInfo)) as tileGameObjectInfo;
        ti.position = this.pos;
        ti.level = this.level;
        ti.parent = this;
        ti.vp = new Vector3(this.pos.q, this.pos.r, this.pos.s);

        this.setTeam(0);
    }

    public void remove() {
        master.removeHex(this);

        GameObject.Destroy(model);
    }

    public void setTeam(teamId t) {
        this.team = t;
    }

    public void setInventory(resources r) {
        this.inventory = r;
    }
}
