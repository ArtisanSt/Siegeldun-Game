using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimated
{
    int spriteDefaultFacing { get; } // 1 is right, -1 is left, 0 when not attacking
    string curAnimStateName { get; } 
    string curSpriteName { get; } 
}
