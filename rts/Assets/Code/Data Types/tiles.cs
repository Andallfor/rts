using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ITile : ITileGenerationRule {
    public GameObject modelPrefab {get; protected set;}
    public tileType type {get; protected set;}

    public string name {get; protected set;}
    public float health {get; protected set;}
    public float defense {get; protected set;}
    public float shield {get; protected set;}
    public GameObject model {get; protected set;}

    public abstract void action(tileActionData data);
    public abstract float hit(tileHitData data);
    public GameObject instTile() {
        this.model = GameObject.Instantiate(modelPrefab);
        return this.model;
    }

    public void remove() {
        GameObject.Destroy(model);
    }
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

        health = 0;
        defense = 0;
        shield = 0;

        whitelist = new List<generationRestrictionData>() {"", tileType.water};
        blacklist = new List<generationRestrictionData>();
        levelHeight = 4;
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

        health = 0;
        defense = 0;
        shield = 0;

        whitelist = new List<generationRestrictionData>() {tileType.ground};
        blacklist = new List<generationRestrictionData>() {};
        levelHeight = 4;
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

        health = 100;
        defense = 25;
        shield = 0;

        whitelist = new List<generationRestrictionData>() {tileType.ground};
        blacklist = new List<generationRestrictionData>() {};
        levelHeight = 4;
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

        health = 150;
        defense = 25;
        shield = 0;

        whitelist = new List<generationRestrictionData>() {tileType.ground};
        blacklist = new List<generationRestrictionData>() {};
        levelHeight = 4;
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

        health = 15;
        defense = 0;
        shield = 0;

        whitelist = new List<generationRestrictionData>() {"forest", "grass"};
        blacklist = new List<generationRestrictionData>() {};
        levelHeight = 4;
    }

    public override void action(tileActionData data) {
        return; // ground does nothing
    }

    public override float hit(tileHitData data) {
        return 0; // ground takes no damage
    }
}