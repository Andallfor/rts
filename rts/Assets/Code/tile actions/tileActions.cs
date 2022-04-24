using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class growTree : ITileAction
{
    public override void action(hex parent, object[] args) {
        if (master.map[parent.pos].allowedToGenerate(parent.level + 4, 4) && canRunAction(parent)) {
            forceAction(parent, args);
        }
    }

    public override void forceAction(hex parent, object[] args)
    {
        master.localPlayer.syncAction(parent, this.name, this.serializeArgs(args));
    }

    public override void actuallyRunAction(hex parent, object[] args)
    {
        new hex(master.size, parent.pos, parent.level + 4, new forest(), 0);
    }

    public override string serializeArgs(object[] args) => "";
    public override object[] deserialzeArgs(string s) => new object[0];

    public override bool canRunAction(hex h) {
        if (h.inventory < this.cost) return false;
        if (!master.registeredTiles.Find(x => x.name == "forest").canTileGenerate(h.pos, h.level + 4)) return false;
        return true;
    }

    private static readonly Lazy<growTree> lazy = new Lazy<growTree>(() => new growTree());
    public static growTree instance {get => lazy.Value;}

    private growTree() {
        cost = new resources(0, 0, 0, 0);
        this.name = "growTree";
        this.displayName = "Grow Tree";
    }
}
