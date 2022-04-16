using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class column {
    public Dictionary<int, hex> levels = new Dictionary<int, hex>();

    public column() {
        this.levels = new Dictionary<int, hex>();
    }
    public hex this[int i] {
        get => levels[i];
        set {levels[i] = value;}}
    
    public bool allowedToGenerate(int level, int height) {
        if (level == 0) return true;
        if (levels.ContainsKey(level)) return false;
        if (!levels.ContainsKey(level - height)) return false;
        return true;
    }
}
