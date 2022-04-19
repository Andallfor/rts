using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ITileGenerationRule {
    public List<generationRestrictionData> blacklist {get; protected set;}
    public List<generationRestrictionData> whitelist {get; protected set;}
    public int levelHeight {get; protected set;}

    public bool canTileGenerate(ITile t, cube pos, int level) {
        bool canGenerate = master.map[pos].allowedToGenerate(level, levelHeight);
        if (!canGenerate) return false;

        if (blacklist.Count == 0 && whitelist.Count == 0) return true;

        // there can be no other tiles below it
        if (level <= t.levelHeight) return listContains(whitelist, "") && !listContains(blacklist, ""); // "" represents empty space

        hex below = master.map[pos][level - t.levelHeight]; // allowedToGenerate ensures that this tile exists

        // blacklist has priority over whitelist
        // if an element exists in both blacklist and whitelist, then we adhere to blacklist and do not allow the tile to generate
        foreach (generationRestrictionData grd in blacklist) {
            if (grd.s == below.name || grd.t == below.type) return false;
        }

        // this means that we allow anything thats not in blacklist to generate
        if (whitelist.Count == 0) return true;
        else {
            foreach (generationRestrictionData grd in whitelist) {
                if (grd.s == below.name || grd.t == below.type) return true;
            }
        }

        return false;
    }

    public bool listContains(List<generationRestrictionData> l, generationRestrictionData grd) {
        if (grd.s == "empty") return l.Exists(x => x.t == grd.t);
        return l.Exists(x => x.s == grd.s);
    }

    public static string emptyString = "empty";
}

public enum generationRestriction {
    whitelist, blacklist
}

public struct generationRestrictionData {
    public string s;
    public tileType t;

    public generationRestrictionData(string s) {this.s = s; this.t = tileType.empty;}
    public generationRestrictionData(tileType t) {this.s = ITileGenerationRule.emptyString; this.t = t;}

    public static implicit operator generationRestrictionData(string s) => new generationRestrictionData(s);
    public static implicit operator generationRestrictionData(tileType t) => new generationRestrictionData(t);
}