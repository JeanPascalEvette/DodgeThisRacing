using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

static class Helpers
{
    public static GameObject FindOrCreateGameObject(string name)
    {
        GameObject go = GameObject.Find(name);
        if (go == null)
            go = new GameObject("Track");
        return go;
    }
}

