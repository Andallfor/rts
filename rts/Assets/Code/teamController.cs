using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class teamController
{
    public static Dictionary<teamId, List<cube>> ownedTiles = new Dictionary<teamId, List<cube>>();
    public static Dictionary<cube, teamId> tileKey = new Dictionary<cube, teamId>();
    public static void registerArea(List<cube> area, teamId t) {
        if (!ownedTiles.ContainsKey(t)) ownedTiles[t] = new List<cube>();

        foreach (cube c in area) {
            // remove current tile from team
            if (tileKey.ContainsKey(c)) {
                teamId key = tileKey[c];
                tileKey.Remove(c);
                ownedTiles[key].Remove(c);
            }

            foreach (hex h in master.map[c].levels.Values) {
                h.setTeam(t);
            }

            ownedTiles[t].Add(c);
            tileKey[c] = t;
        }
    }
}

public enum teamId : int {
    none = 0, red = 1, blue = 2
}