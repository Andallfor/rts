using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ITileAction
{
    public resources cost {get; protected set;}
    public string name {get; protected set;}
    public string displayName {get; protected set;}

    public abstract void action(hex parent, object[] args);
    public abstract void forceAction(hex parent, object[] args);
    public abstract bool canRunAction(hex h);
    public abstract string serializeArgs(object[] args);
    public abstract object[] deserialzeArgs(string s);
    public abstract void actuallyRunAction(hex parent, object[] args);
}
