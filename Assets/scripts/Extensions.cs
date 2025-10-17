using System.Collections.Generic;

// Métodos de extensión reutilizables para coleccions do proxecto
public static class Extensions
{
    // Xerador de números aleatorios (compartido para evitar crear moitos Rngs)
    private static System.Random rng = new System.Random();

    // Mezcla os elementos dunha lista in-place usando o algoritmo Fisher–Yates.
    // Este método é unha extensión: chámase como `myList.Shuffle()`.
    // Conserva o tipo e a capacidade da lista; a orde dos elementos cambia aleatoriamente.
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
