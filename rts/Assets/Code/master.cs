using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Linq;

public static class master {
    public static List<ITile> registeredTiles = new List<ITile>() {
        new forest(), new ocean(), new dirt(), new mountain(), new grass(), new castle()
    };
    public static player localPlayer;
    public static Dictionary<cube, column> map {get => _map;}
    private static Dictionary<cube, column> _map = new Dictionary<cube, column>();

    /// <summary> note: uses h.pos as the key </summary>
    public static bool addHex(hex h, bool check = true) {
        if (!_map.ContainsKey(h.pos)) _map[h.pos] = new column();
        if (check) {
            if (_map[h.pos].allowedToGenerate(h.level, h.levelHeight)) {
                _map[h.pos][h.level] = h;
                return true;
            } else return false;
        } else {
            _map[h.pos][h.level] = h;
            return true;
        }
    }

    /// <summary> note: uses h.pos as the key </summary>
    public static bool containsHex(hex h) {
        if (_map.ContainsKey(h.pos)) {
            if (_map[h.pos].levels.ContainsKey(h.level)) {
                return true;
            }
        }
        return false;
    }

    /// <summary> note: uses h.pos as the key </summary>
    public static void removeHex(hex h) {
        _map[h.pos].levels.Remove(h.level);
    }

    public static void removeColumn(cube c) {
        List<hex> hCopy = new List<hex>(_map[c].levels.Values);
        foreach (hex h in hCopy) {
            h.remove();
        }
        _map[c] = new column();
    }

    public static string serializeMap() {
        /* example format-> name:str~team:int
           each tile has its own line
        transmitted values:
            name: str
            team: int
            health: float
            defense: float
            shield: float
            pos: cube (but as a string)
            level: int
            size: float

        */
        StringBuilder sb = new StringBuilder();
        foreach (column c in map.Values) {
            foreach (hex h in c.levels.Values) {
                sb.AppendLine(
                    $"name:{h.name}~" +
                    $"team:{(int) h.team}~" +
                    $"health:{h.health}~" +
                    $"defense:{h.defense}~" +
                    $"shield:{h.shield}~" +
                    $"pos:{h.pos.ToString()}~" +
                    $"level:{h.level}~" +
                    $"size:{h.size}");
            }
        }

        return sb.ToString();
    }

    public static void deserializeMap(string s) {
        string[] tiles = s.Split(new char[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);

        foreach (string tile in tiles) {
            string[] tileData = tile.Split('~');
            string name = default;
            int team = default, level = default;
            float health = default, defense = default, shield = default, size = default;
            cube pos = default;

            foreach (string data in tileData) {
                string key = data.Split(':').First();
                string value = data.Split(':').Last();

                switch (key) {
                    case "name":
                        name = value;
                        break;
                    case "team":
                        team = int.Parse(value);
                        break;
                    case "health":
                        health = float.Parse(value);
                        break;
                    case "defense":
                        defense = float.Parse(value);
                        break;
                    case "shield":
                        shield = float.Parse(value);
                        break;
                    case "pos":
                        pos = new cube(value);
                        break;
                    case "level":
                        level = int.Parse(value);
                        break;
                    case "size":
                        size = float.Parse(value);
                        break;
                }
            }

            new hex(name, (teamId) team, level, health, defense, shield, size, pos);
            teamController.registerArea(new List<cube>() {pos}, (teamId) team, false);
        }
    }

    public static float size = 2f / Mathf.Sqrt(3);
}
