using UnityEngine;

// Representa unha posición no grid do mapa.
// Usado para indicar coordenadas inteiras en X e Z (columna, fila).
public class MapLocation       
{
    // Coordenada X (columna)
    public int x;

    // Coordenada Z (fila)
    public int z;

    // Constructor
    // _x - localización en x
    // _z - localización en z
    public MapLocation(int _x, int _z)
    {
        x = _x;
        z = _z;
    }

    // Converte esta localización a un Vector2 (x, z). Útil para cálculos de distancia.
    public Vector2 ToVector()
    {
        return new Vector2(x, z);
    }

    // Sobrecarga do operador + para sumar dúas localizacións
    public static MapLocation operator +(MapLocation a, MapLocation b)
       => new MapLocation(a.x + b.x, a.z + b.z);

    // Compara dúas MapLocation polas súas coordenadas (x e z)
    public override bool Equals(object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            return false;
        else
            return x == ((MapLocation)obj).x && z == ((MapLocation)obj).z;
    }

    // Combina as coordenadas para obter un hash.
    // Isto é coherente con Equals porque Equals tamén compara (x,z).
    public override int GetHashCode()
    {
        return System.HashCode.Combine(x, z);
    }

}