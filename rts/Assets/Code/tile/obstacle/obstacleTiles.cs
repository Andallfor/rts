using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class mountain : ITile {
    public mountain() {
        name = "mountain";
        modelPrefab = Resources.Load("Objects/mountain") as GameObject;
        type = tileType.obstacle;

        maxHealth = 100;
        maxDefense = 25;
        maxShield = 0;

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
