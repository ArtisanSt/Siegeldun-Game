using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractible
{
    bool isInteractible { get; }
    void Interact();
    void ToggleInteractible();
    void DisableInteract();
}
