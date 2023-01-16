using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInitializeable
{
    public string DefaultsToJson();
}

public interface IInitializeable<TDataProp> : IInitializeable
    where TDataProp : IDataProp
{
    public TDataProp dataProp { get; }
    public void Init(TDataProp dataProp);
}

/*public static class InitializeableExtensions
{
    public static string DefaultsToJson<TDataProp>(this IInitializeable<TDataProp> value)
        where TDataProp : IDataProp
    {
        return value.dataProp.ToJson();
    }
}*/
