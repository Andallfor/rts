using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ocean : ITile {
    public ocean() {
        name = "ocean";
        modelPrefab = Resources.Load("Tiles/hex_water") as GameObject;
        type = tileType.water;

        maxHealth = 0;
        maxDefense = 0;
        maxShield = 0;

        whitelist = new List<generationRestrictionData>() {"", tileType.water};
        blacklist = new List<generationRestrictionData>();
        possibleActions = new List<ITileAction>();
        levelHeight = 4;

        base.init();
    }

    public override void action(tileActionData data) {
        return; // water does nothing
    }

    public override float hit(tileHitData data) {
        return 0; // water takes no damage
    }
}
