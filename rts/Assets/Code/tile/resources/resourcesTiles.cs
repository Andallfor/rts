using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forest : ITile {
    public forest() {
        name = "forest";
        modelPrefab = Resources.Load("Objects/forest") as GameObject;
        type = tileType.resource;

        maxHealth = 15;
        maxDefense = 0;
        maxShield = 0;

        whitelist = new List<generationRestrictionData>() {"grass"};
        blacklist = new List<generationRestrictionData>() {};
        possibleActions = new List<ITileAction>();
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

public class castle : ITile {
    public castle() {
        name = "castle";
        modelPrefab = Resources.Load("Objects/castle") as GameObject;
        type = tileType.resource;

        maxHealth = 30;
        maxDefense = 5;
        maxShield = 20;

        whitelist = new List<generationRestrictionData>() {tileType.ground};
        blacklist = new List<generationRestrictionData>() {};
        possibleActions = new List<ITileAction>();
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