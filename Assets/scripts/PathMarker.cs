using UnityEngine;

public class PathMarker {

    public MapLocation location;
    public float G, H, F;
    public GameObject marker;
    public PathMarker parent;

    /// <summary>
    /// Constructor para os marcadores que aparecen no xogo
    /// </summary>
    /// <param name="l">Localización no mapa</param>
    /// <param name="g">Coste acumulado ata este marcador</param>
    /// <param name="h">Coste estimado desde este marcador ao obxectivo</param>
    /// <param name="f">f+g</param>
    /// <param name="m">Gameobject para representar este marcador</param>
    /// <param name="p">O obxecto pai de este marcador para saber desde onde chegamos</param>
    public PathMarker(MapLocation l, float g, float h, float f, GameObject m, PathMarker p) {

        location = l;
        G = g;
        H = h;
        F = f;
        marker = m;
        parent = p;
    }

    /// <summary>
    /// Para comparar dous marcadores utilízase a súa localización
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj) {

        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            return false;
        else
            return location.Equals(((PathMarker)obj).location);
    }

    public override int GetHashCode() {

        return System.HashCode.Combine(G, H, F);
    }
}