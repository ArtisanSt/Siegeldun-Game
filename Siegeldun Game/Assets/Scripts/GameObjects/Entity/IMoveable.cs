using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable
{
    bool doMoveX { get; }
    /*
    float totalXSpeed { get; }
    float mvSpeed { get; }
    float dirXFacing { get; } // Entity Facing: -1 to 1
    float dirXFinal { get; } // -1 to 1

    bool doMoveY { get; }
    float totalYSpeed { get; }
    float jumpForce { get; }

    List<SpeedBoost> speedBoost { get; }

    float MvSpeedBoost(string boostName, Vector2 speedBoost);
    float MvSpeedBoost(string boostName, string x_or_y, float xyBoost);

    void Movement();
    */
}
