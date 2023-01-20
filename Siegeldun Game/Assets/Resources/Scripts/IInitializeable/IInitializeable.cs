using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInitializeable
{
    public void Init();
    public bool initialized { get; }
    public bool alive { get; set; }
}
