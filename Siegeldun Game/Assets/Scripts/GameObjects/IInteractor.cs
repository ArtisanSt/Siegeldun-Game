using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractor
{
    bool InteractorColliderConditions(Collider2D coll);
}
