using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInitializer
{
    public IInitializeable[] iInits { get; }
    public void InitInitializeables();
}
