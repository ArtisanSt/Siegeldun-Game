using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectUtils
{
    // ============================== PLACEHOLDER ==============================
    public static string nullPlaceholder
    {
        get
        {
            string value;
            try
            {
                value = Globals.nullPlaceholder;
            }
            catch (System.Exception)
            {
                value = "NULL";
            }

            return value;
        }
    }

    public static string Defaults(string var)
    {
        return (var.Trim() == "") ? nullPlaceholder : var;
    }

    public static void Defaults(ref string var)
    {
        var = (var.Trim() == "") ? nullPlaceholder : var;
    }
}
