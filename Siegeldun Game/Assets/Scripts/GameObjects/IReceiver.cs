using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReceiver
{
    void Activate(GameObject source);
    void Deactivate(GameObject source);
}
