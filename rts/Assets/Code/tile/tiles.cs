using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// TODO: make children into a singleton? 
// have health and whatnot be part of hex instead of the tile
public abstract class ITile : ITileGenerationRule {
    public GameObject modelPrefab {get; protected set;}
    public tileType type {get; protected set;}

    public string name {get; protected set;}
    public float health {get; protected set;}
    public float maxHealth {get; protected set;}
    public float defense {get; protected set;}
    public float maxDefense {get; protected set;}
    public float shield {get; protected set;}
    public float maxShield {get; protected set;}
    public List<ITileAction> possibleActions = new List<ITileAction>();

    public abstract void action(tileActionData data);
    public abstract float hit(tileHitData data);

    protected void init() {
        health = maxHealth;
        defense = maxDefense;
        shield = maxShield;
    }

    public void forceSetHealth(float f) {this.health = f;}
    public void forceSetDefense(float d) {this.defense = d;}
    public void forceSetShield(float s) {this.shield = s;}
}

public struct tileActionData {

}

public struct tileHitData {

}

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

public class forest : ITile {
    public forest() {
        name = "forest";
        modelPrefab = Resources.Load("Objects/forest") as GameObject;
        type = tileType.resource;

        maxHealth = 15;
        maxDefense = 0;
        maxShield = 0;

        whitelist = new List<generationRestrictionData>() {"forest", "grass"};
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