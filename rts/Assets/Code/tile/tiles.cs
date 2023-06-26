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


