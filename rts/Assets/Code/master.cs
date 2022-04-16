using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class master {
    public static List<Type> registeredTiles = new List<Type>() {
        typeof(ocean), typeof(grass), typeof(mountain)
    };
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

    public static float size = 2f / Mathf.Sqrt(3);
}
