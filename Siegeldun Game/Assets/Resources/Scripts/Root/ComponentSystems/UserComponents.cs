using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UserComponents : MonoBehaviour
{
    // ============================== SYSTEM PROPERTIES ==============================
    // Checks if game is paused
    public bool paused { get { return GameSystem.paused; } }
    public int difficulty { get { return GameSystem.difficulty; } }
    public bool allow; // Runtime Restriction
    public static bool alive { get; protected set; }


    // ============================== INITIALIZATION ==============================
    public virtual bool IsRestricted() { return paused || !allow; }
    protected abstract void PropertyInit();


    // ============================== PROCESS ==============================
    public virtual void Awake()
    {
        processes = new Dictionary<string, List<string>>();
    }

    public Dictionary<string, List<string>> processes; // ["Method"] = List of processID

    public bool AddProcess(string methodName, string processID)
    {
        if (!ContainsMethodName(methodName)) processes.Add(methodName, new List<string>());
        if (ContainsProcessID(methodName, processID)) return false;
        processes[methodName].Add(processID);
        return true;
    }

    private bool ContainsMethodName(string methodName)
    {
        return processes.ContainsKey(methodName);
    }

    private bool ContainsProcessID(string methodName, string processID)
    {
        if (!ContainsMethodName(methodName)) return false;
        return processes[methodName].Contains(processID);
    }

    public void RemoveProcess(string methodName, string processID)
    {
        if (!ContainsMethodName(methodName) || !ContainsProcessID(methodName, processID)) return;
        processes[methodName].Remove(processID);
    }
}
