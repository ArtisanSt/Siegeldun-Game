using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRestrictable
{
    public bool paused { get; }
    public bool allowed { get; }
    public bool IsRestricted();
}
