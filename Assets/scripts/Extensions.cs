using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    private static System.Random rng = new System.Random();
    /// <summary>
    /// Método para mezclar os elementos dunha Lista
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void Shuffle<T>(this IList<T> list)
    {
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

}
