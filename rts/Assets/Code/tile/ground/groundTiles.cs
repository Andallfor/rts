using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dirt : ITile {
    public dirt() {
        name = "dirt";
        modelPrefab = Resources.Load("Tiles/hex_rock") as GameObject;
        type = tileType.ground;

        maxHealth = 150;
        maxDefense = 25;
        maxShield = 0;

        whitelist = new List<generationRestrictionData>() {tileType.ground};
        blacklist = new List<generationRestrictionData>() {};
        possibleActions = new List<ITileAction>() {growTree.instance};
        levelHeight = 4;

        base.init();
    }

    public override void action(tileActionData data) {
        return; // ground does nothing
    }

    public override float hit(tileHitData data) {
        return 0; // ground takes no damage
    }
}

public class grass : ITile {
    public grass() {
        name = "grass";
        modelPrefab = Resources.Load("Tiles/hex_forest") as GameObject;
        type = tileType.ground;

        maxHealth = 0;
        maxDefense = 0;
        maxShield = 0;

        whitelist = new List<generationRestrictionData>() {tileType.ground};
        blacklist = new List<generationRestrictionData>() {};
        possibleActions = new List<ITileAction>() {growTree.instance};
        levelHeight = 4;

        base.init();
    }

    public override void action(tileActionData data) {
        return; // ground does nothing
    }

    public override float hit(tileHitData data) {
        return 0; // ground takes no damage
    }
}