using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct resources
{
    public int gold, iron, food, wood;

    public resources(int gold, int iron, int food, int wood) {
        this.gold = gold;
        this.iron = iron;
        this.food = food;
        this.wood = wood;
    }

    public bool satisfied(resources r) => this.gold >= r.gold && this.iron >= r.iron && this.food >= r.food && this.wood >= r.wood;

    public static resources operator-(resources r1, resources r2) => new resources(r1.gold - r2.gold, r1.iron - r2.iron, r1.food - r2.food, r1.wood - r2.wood);
    public static resources operator+(resources r1, resources r2) => new resources(r1.gold + r2.gold, r1.iron + r2.iron, r1.food + r2.food, r1.wood + r2.wood);
    public static resources operator*(resources r1, int f) => new resources(r1.gold * f, r1.iron * f, r1.food * f, r1.wood * f);
    public static resources operator*(int f, resources r1) => new resources(r1.gold * f, r1.iron * f, r1.food * f, r1.wood * f);
    public static resources operator/(resources r1, int f) => new resources(r1.gold / f, r1.iron / f, r1.food / f, r1.wood / f);
    public static resources operator/(int f, resources r1) => new resources(r1.gold / f, r1.iron / f, r1.food / f, r1.wood / f);
    public static bool operator>(resources r1, resources r2) => r1.gold > r2.gold && r1.iron > r2.iron && r1.food > r2.food && r1.wood > r2.wood;
    public static bool operator>=(resources r1, resources r2) => r1.gold >= r2.gold && r1.iron >= r2.iron && r1.food >= r2.food && r1.wood >= r2.wood;
    public static bool operator<(resources r1, resources r2) => r1.gold < r2.gold && r1.iron < r2.iron && r1.food < r2.food && r1.wood < r2.wood;
    public static bool operator<=(resources r1, resources r2) => r1.gold <= r2.gold && r1.iron <= r2.iron && r1.food <= r2.food && r1.wood <= r2.wood;
    public static bool operator>(resources r1, int i) => r1.gold > i && r1.iron > i && r1.food > i && r1.wood > i;
    public static bool operator>=(resources r1, int i) => r1.gold >= i && r1.iron >= i && r1.food >= i && r1.wood >= i;
    public static bool operator<(resources r1, int i) => r1.gold < i && r1.iron < i && r1.food < i && r1.wood < i;
    public static bool operator<=(resources r1, int i) => r1.gold <= i && r1.iron <= i && r1.food <= i && r1.wood <= i;
}
