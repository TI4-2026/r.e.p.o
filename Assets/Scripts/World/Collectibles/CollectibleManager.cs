using System.Collections.Generic;
using UnityEngine;

public static class CollectibleManager
{
    public static int[] collected = new int[3];
    private static HashSet<int>[] collectedIDs =
{
        new HashSet<int>(),
        new HashSet<int>(),
        new HashSet<int>()
    };

    public static void RegisterCollectible(int id, int area)
    {
        if (collectedIDs[area].Contains(id))
            return;

        collectedIDs[area].Add(id);
        collected[area]++;

        Debug.Log($"Área {area}: {collected[area]} coletados");
    }

    public static bool IsCollected(int area, int id)
    {
        return collectedIDs[area].Contains(id);
    }
}
