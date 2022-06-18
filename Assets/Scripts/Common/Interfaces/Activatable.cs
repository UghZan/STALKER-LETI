using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activatable : MonoBehaviour
{
    public virtual bool OnActivate() { return false; }
    public virtual bool OnActivate(bool state) { return false; }
}
