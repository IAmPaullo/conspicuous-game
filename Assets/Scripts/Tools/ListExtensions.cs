using System;
using System.Collections.Generic;

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        Random rng = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public static bool IsValid<T>(IList<T> list)
    {
        return list == null || list.Count == 0;
    }
    //public static void Validate<T>(IList<T> list, string listName)
    //{
    //    if (list == null || list.Count == 0)
    //        throw new System.Exception($"The List {listName} is null or empty.");
    //}
}
