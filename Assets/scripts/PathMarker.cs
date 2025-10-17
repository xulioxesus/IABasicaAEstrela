using UnityEngine;

// Representa un nodo/marker usado polo algoritmo A* para rastrexar posicións no mapa.
// - `location` define a posición no grid (MapLocation).
// - G, H, F son os custos usados por A* (G = custo desde a orixe, H = heurística, F = G + H).
// - `marker` é o GameObject visible na escena que simboliza este nodo.
// - `parent` apunta ao nodo anterior na ruta (permite reconstruír o camiño).
public class PathMarker {

    // Posición no mapa (índices enteros)
    public MapLocation location;

    // Custos A*
    // G - custo acumulado desde o nodo inicial ata este nodo
    // H - estimación do custo desde este nodo ata o obxectivo
    // F - suma G + H (custo total estimado)
    public float G, H, F;

    // GameObject que representa visualmente este marker na escena
    public GameObject marker;

    // Nodo pai dende o que chegamos a este nodo (permite reconstruír a ruta)
    public PathMarker parent;

    // Constructor
    // l - localización no mapa
    // g - custo acumulado ata este marcador
    // h - custo estimado dende este marcador ao obxectivo
    // f - custo total estimado (G + H)
    // m - GameObject que representa este marcador
    // p - pai do marcador (desde onde viñemos)
    public PathMarker(MapLocation l, float g, float h, float f, GameObject m, PathMarker p) {

        location = l;
        G = g;
        H = h;
        F = f;
        marker = m;
        parent = p;
    }

    // Comparación: dous PathMarker considéranse iguais se teñen a mesma localización.
    public override bool Equals(object obj) {

        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            return false;
        else
            return location.Equals(((PathMarker)obj).location);
    }

    // Devolve un hash para este obxecto. Neste caso combínanse os valores G, H, F.
    // Nota: Equals usa `location` para a comparación, polo que, en termos ideais,
    // GetHashCode debería basearse na mesma información (p.ex. location) para garantir
    // coherencia entre Equals e GetHashCode. Non se modifica aquí a lóxica, só se documenta.
    public override int GetHashCode() {

        return System.HashCode.Combine(G, H, F);
    }
}