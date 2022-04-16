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
    private cube _pos;
    private float width, height, vertDist, horzDist;
    public int level {get; private set;}
    private ITile tile;
    public string name {get => tile.name;}
    public tileType type {get => tile.type;}
    public int levelHeight {get => tile.levelHeight;}

    public hex(float size, cube position, int level, ITile tile) {
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

    private void instTile(ITile tile) {
        this.tile = tile;
        tile.instTile();

        tile.model.transform.position = worldPos;
    }

    public void remove() {
        master.removeHex(this);

        tile.remove();
    }
}
