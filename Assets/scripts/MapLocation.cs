using UnityEngine;

public class MapLocation       
{
    public int x;
    public int z;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="_x">Localización en x</param>
    /// <param name="_z">Localización en z</param>
    public MapLocation(int _x, int _z)
    {
        x = _x;
        z = _z;
    }

    /// <summary>
    /// Transformar un MapLocation nun Vector2
    /// </summary>
    /// <returns>Un Vector2 coas coordenadas da localización no mapa</returns>
    public Vector2 ToVector()
    {
        return new Vector2(x, z);
    }

    /// <summary>
    /// Permite sumar dous MapLocation
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>Devolve un novo MapLocation</returns>
    public static MapLocation operator +(MapLocation a, MapLocation b)
       => new MapLocation(a.x + b.x, a.z + b.z);

    /// <summary>
    /// Para comparar dous MapLocation utilízanse as coordenadas x e z
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            return false;
        else
            return x == ((MapLocation)obj).x && z == ((MapLocation)obj).z;
    }

    public override int GetHashCode()
    {
        return System.HashCode.Combine(x, z);
    }

}