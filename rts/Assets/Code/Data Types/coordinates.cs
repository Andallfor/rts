using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public struct cube {
    public int q, r, s;
    private static readonly cube epsilon = new cube((int) 1e-6, (int) 2e-6, (int) 3e-6);
    public cube(int q, int r, int s) {
        if (q + r + s != 0) {
            throw new System.Exception($"Given cube coordinates (q: {q}, r: {r}, s: {s}) do not equal 0.");
        }
        this.q = q;
        this.r = r;
        this.s = s;
    }

    public cube(string s) {
        s = s.Replace(' ', '\0');
        string[] d = s.Split('|');
        this.q = int.Parse(d[0].Split('=').Last());
        this.r = int.Parse(d[1].Split('=').Last());
        this.s = int.Parse(d[2].Split('=').Last());
    }

    public cube getNeighbor(hexDirection direction) {
        switch (direction) {
            case hexDirection.north:
                return this + new cube(0, -1, 1);
            case hexDirection.northEast:
                return this + new cube(1, -1, 0);
            case hexDirection.southEast:
                return this + new cube(1, 0, -1);
            case hexDirection.south:
                return this + new cube(0, 1, -1);
            case hexDirection.southWest:
                return this + new cube(-1, 1, 0);
            case hexDirection.northWest:
                return this + new cube(1, 0, 1);
        }

        return this;
    }

    public int distance(cube c) {
        return manhattanDistance(c) / 2;
    }

    public int manhattanDistance(cube c) {
        c -= this;
        return (Mathf.Abs(c.q) + Mathf.Abs(c.r) + Mathf.Abs(c.s)) / 2;
    }

    public static cube lerp(cube c1, cube c2, float t) {
        c1 += epsilon;
        c2 += epsilon;
        Vector3 v = new Vector3(Mathf.Lerp(c1.q, c2.q, t),
                                Mathf.Lerp(c1.r, c2.r, t),
                                Mathf.Lerp(c1.s, c2.s, t));
        return cube.round(v);
    }

    public List<cube> line(cube c) {
        List<cube> output = new List<cube>();
        int d = distance(c);
        for (int i = 0; i < d; i++) {
            output.Add(lerp(this, c, 1f / d * i));
        }

        return output;
    }

    public List<cube> nearby(int d) {
        List<cube> output = new List<cube>();
        for (int q = -d; q < d; q++) {
            for (int r = Mathf.Max(-d, -q-d); r < Mathf.Min(d, -q+d); r++) {
                output.Add(this + new cube(q, r, -q-r));
            }
        }

        return output;
    }

    private static cube round(Vector3 v) {
        int q = Mathf.RoundToInt(v.x);
        int r = Mathf.RoundToInt(v.y);
        int s = Mathf.RoundToInt(v.z);

        float qd = Mathf.Abs((float) q - v.x);
        float rd = Mathf.Abs((float) r - v.y);
        float sd = Mathf.Abs((float) s - v.z);

        if (qd > rd && qd > sd) {
            q = -r-s;
        } else if (rd > sd) {
            r = -q-s;
        } else {
            s = -q-r;
        }

        return new cube(q, r, s);
    }

    public Vector2 position(float size) {
        return new Vector2(
            size * (Mathf.Sqrt(3) * q + Mathf.Sqrt(3) / 2f * r),
            size * (3f / 2f * r));
    }

    public static cube operator+(cube c1, cube c2) => new cube(c1.q + c2.q, c1.r + c2.r, c1.s + c2.s);
    public static cube operator-(cube c1, cube c2) => new cube(c1.q - c2.q, c1.r - c2.r, c1.s - c2.s);

    public static bool operator==(cube p1, cube p2) => (p1.q == p2.q && p1.r == p2.r && p1.s == p2.s);
    public static bool operator!=(cube p1, cube p2) => (p1.q != p2.q || p1.r != p2.r || p1.s != p2.s);

    public override bool Equals(object obj)
    {
        if (obj is cube) {
            return (cube) obj == this;
        }

        return false;
    }

    public static implicit operator cube(Vector3Int v) => new cube(v.x, v.y, v.z);
    public static implicit operator Vector3Int(cube c) => new Vector3Int(c.q, c.r, c.s);

    public override int GetHashCode()
    {
        ulong A = (ulong)(q >= 0 ? 2 * (long)q : -2 * (long)q - 1);
        ulong B = (ulong)(r >= 0 ? 2 * (long)r : -2 * (long)r - 1);
        long C = (long)((A >= B ? A * A + A + B : A + B * B) / 2);
        return (int) (q < 0 && r < 0 || q >= 0 && r >= 0 ? C : -C - 1);
    }

    public override string ToString() => $"q={q} | r={r} | s={s}";
}
