using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Process : MonoBehaviour
{
    // =========================================  PROCESS EVALUATOR =========================================
    protected System.Func<int, bool> ChanceRandomizer = dropChance => (dropChance >= 1) ? Random.Range(0, dropChance) == 0 : false;
    protected System.Func<float, float, bool> TimerIncrement = (timeStart, timeDuration) => Time.time - timeStart >= timeDuration;

    protected List<float> curProcess = new List<float>();

    protected bool ProcessEvaluator(float instanceID, float time = 1)
    {
        if (!curProcess.Contains(instanceID))
        {
            curProcess.Add(instanceID);

            StartCoroutine(ClearID(instanceID, time));
            return true;
        }
        else
        {
            return false;
        }
    }

    protected IEnumerator ClearID(float instanceID, float time = 1)
    {
        yield return new WaitForSeconds(time);
        curProcess.Remove(instanceID);
    }

    protected delegate void Del();

    protected void IgnoreErrors(Del handler, Del backUp = null, Del ender = null)
    {
        try
        {
            handler();
        }
        catch
        {
            if (backUp != null) backUp();
        }
        finally
        {
            if (ender != null) ender();
        }
    }
}
