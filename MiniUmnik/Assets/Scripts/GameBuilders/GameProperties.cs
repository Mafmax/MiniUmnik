using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProperties
{

    public Dictionary<string, object> Arguments = new Dictionary<string, object>();

    public Action<int> ConstructProperties;
    
}
