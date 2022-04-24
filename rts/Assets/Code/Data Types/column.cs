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
        for (int i = 0; i < height; i++) {
            if (levels.ContainsKey(level - i)) return false;
        }
        
        return true;
    }
}
