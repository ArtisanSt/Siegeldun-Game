using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogue
{
    void OnEndMessage(Dialogue curDialogue);
    void OnStartMessage(Dialogue curDialogue);
    void OnDisplayMessage(Dialogue curDialogue);
}
