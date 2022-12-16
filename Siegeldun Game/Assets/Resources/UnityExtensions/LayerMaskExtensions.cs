using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerMaskExtensions
{
    // ============================== LAYERMASK ==============================
    public static string IndexToName(this int index)
    {
        return LayerMask.LayerToName(index);
    }

    public static string BinToName(this int bin)
    {
        return LayerMask.LayerToName(bin.ToInt());
    }

    public static int BinToIndex(this int bin)
    {
        return bin.ToInt();
    }

    public static int NameToIndex(this string name)
    {
        return LayerMask.NameToLayer(name);
    }

    public static int IndexToBin(this int index)
    {
        return index.ToBin();
    }

    public static int NameToBin(this string name)
    {
        return LayerMask.NameToLayer(name).ToBin();
    }

    // Checks if a layermask contain the following layers
    public static bool ContainsBin(this LayerMask mask, int bin)
    {
        return (mask &= bin) == bin;
    }

    public static bool[] ContainsBin(this LayerMask mask, params int[] bin)
    {
        bool[] output = new bool[bin.Length];
        for (int i = 0; i < bin.Length; i++)
        {
            output[i] = mask.ContainsBin(bin[i]);
        }
        return output;
    }

    public static bool ContainsIndex(this LayerMask mask, int index)
    {
        return mask == (mask | (1 << index));
    }

    public static bool[] ContainsIndex(this LayerMask mask, params int[] index)
    {
        bool[] output = new bool[index.Length];
        for (int i=0; i< index.Length; i++)
        {
            output[i] = mask.ContainsIndex(index[i]);
        }
        return output;
    }

    // Combine layermasks
    public static LayerMask Join(this LayerMask mask, params LayerMask[] layermasks)
    {
        for (int i=0; i<layermasks.Length; i++)
        {
            mask |= layermasks[i];
        }
        return mask;
    }

    // Outputs the layermasks found in the mask
    public static LayerMask Compare(this LayerMask mask, params LayerMask[] layermasks)
    {
        for (int i = 0; i < layermasks.Length; i++)
        {
            mask &= layermasks[i];
        }
        return mask;
    }

    // Lists all of the layers
    public static List<int> ToListIndex(this LayerMask mask)
    {
        int i = 1; // Starts with 1 because 0 layer is nothing
        List<int> output = new List<int>();
        while (mask > 0)
        {
            mask = mask >> 1;
            if ((mask.value & 1) == 1)
            {
                output.Add(i);
            }
            i++;
        }
        return output;
    }

    public static List<string> ToListName(this LayerMask mask)
    {
        int i = 1; // Starts with 1 because 0 layer is nothing
        List<string> output = new List<string>();
        while (mask > 0)
        {
            mask = mask >> 1;
            if ((mask.value & 1) == 1)
            {
                output.Add((i).IndexToName());
            }
            i++;
        }
        return output;
    }

    public static List<int> ToListIndex(this List<string> masks)
    {
        List<int> output = new List<int>();
        for (int i = 0; i < masks.Count; i++)
        {
            output.Add(masks[i].NameToIndex());
        }
        return output;
    }

    public static List<string> ToListName(this List<int> masks)
    {
        List<string> output = new List<string>();
        for (int i = 0; i < masks.Count; i++)
        {
            output.Add(masks[i].IndexToName());
        }
        return output;
    }

    public static Dictionary<int, string> ToDict(this LayerMask mask)
    {
        Dictionary<int, string> output = new Dictionary<int, string>();
        int i = 1; // Starts with 1 because 0 layer is nothing
        while (mask > 0)
        {
            mask = mask >> 1;
            if ((mask.value & 1) == 1)
            {
                output.Add(i, i.IndexToName());
            }
            i++;
        }
        return output;
    }
}