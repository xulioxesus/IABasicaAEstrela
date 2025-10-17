using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Componente que implementa unha visualización paso a paso do algoritmo A* sobre un `Maze`.
// - Selecciona dous puntos aleatorios libres no `Maze` como inicio e fin.
// - Emprega `PathMarker` para representar nodos abertos/pechados na escena.
// - As cores/materials e os prefabs `start`, `end`, `pathP` úsanse para visualización.
public class FindPathAStar : MonoBehaviour {

    // Referencia ao labirinto sobre o que buscamos (debe estar asignado no inspector)
    public Maze maze;
    // Materiais para distinguir nodos pechados/abertos (visual)
    public Material closedMaterial;
    public Material openMaterial;
    // Prefabs/objetos para representar o nodo de inicio e fin
    public GameObject start;
    public GameObject end;
    // Prefab para representar cada marcador de camiño (contén TextMesh para G/H/F)
    public GameObject pathP;

    // Nodo inicial e obxectivo usados pola búsqueda
    PathMarker startNode;
    PathMarker goalNode;
    // Último nodo extraído para seguir a busca paso a paso
    PathMarker lastPos;
    // Estado da busca
    bool done;
    bool hasStarted = false;

    // Conxuntos de nodos abertos e pechados usados por A*
    List<PathMarker> open = new List<PathMarker>();
    List<PathMarker> closed = new List<PathMarker>();

    // Elimina todos os GameObjects co tag "marker" da escena (limpa a visualización anterior)
    void RemoveAllMarkers() {

        GameObject[] markers = GameObject.FindGameObjectsWithTag("marker");

        foreach (GameObject m in markers) Destroy(m);
    }

    // Prepara e inicia a busca A*:
    // - Recolle todas as celas libres do `maze`.
    // - Selecciona aleatoriamente dous puntos (inicio, fin).
    // - Crea os PathMarker iniciais e limpa as listas open/closed.
    void BeginSearch() {

        done = false;
        //Borra todos os marcadores da escea
        RemoveAllMarkers();

        // Crea unha lista de posibles localizacións libres para colocar os marcadores
        List<MapLocation> locations = new List<MapLocation>();

        // Recorre o mapa e engade as posicións libres (non paredes) á lista
        for (int z = 1; z < maze.depth - 1; ++z) {
            for (int x = 1; x < maze.width - 1; ++x) {

                if (maze.map[x, z] != 1) {

                    locations.Add(new MapLocation(x, z));
                }
            }
        }

    // Mestura as localizacións para escoller dous puntos aleatorios
    locations.Shuffle();

    // Utiliza a primeira localización como startNode (xera un GameObject de inicio)
        Vector3 startLocation = new Vector3(locations[0].x * maze.scale, 0.0f, locations[0].z * maze.scale);
        startNode = new PathMarker(new MapLocation(locations[0].x, locations[0].z),
            0.0f, 0.0f, 0.0f, Instantiate(start, startLocation, Quaternion.identity), null);

    // Utiliza a segunda localización como goalNode (xera un GameObject de destino)
        Vector3 endLocation = new Vector3(locations[1].x * maze.scale, 0.0f, locations[1].z * maze.scale);
        goalNode = new PathMarker(new MapLocation(locations[1].x, locations[1].z),
            0.0f, 0.0f, 0.0f, Instantiate(end, endLocation, Quaternion.identity), null);

        // Reseta as estruturas de estado
        open.Clear();
        closed.Clear();

        // Engade o nodo inicial á lista de abertos e marca como último nodo visitado
        open.Add(startNode);
        this.lastPos = startNode;
    }

    // Realiza un paso de búsqueda a partir do nodo `thisNode`.
    // Explora os 4 veciños en cruz, calcula G/H/F e engade/actualiza marcadores en `open`.
    void Search(PathMarker thisNode) {

        //Se o nodo actual é o obxectivo, a búsqueda acaba
        if (thisNode.Equals(goalNode)) {

            done = true;
            // Debug.Log("DONE!");
            return;
        }

        //Para cada un dos veciños
        foreach (MapLocation dir in maze.directions) {

            MapLocation neighbour = dir + thisNode.location;

            // Ignora veciños que son paredes, están fóra de límites ou xa están pechados
            if (maze.map[neighbour.x, neighbour.z] == 1) continue;
            if (neighbour.x < 1 || neighbour.x >= maze.width || neighbour.z < 1 || neighbour.z >= maze.depth) continue;
            if (IsClosed(neighbour)) continue;

            //Esta é a clave do algoritmo
            // g =  g(de este nodo) + distancia ao veciño
            float g = Vector2.Distance(thisNode.location.ToVector(), neighbour.ToVector()) + thisNode.G;
            // h = distancia do veciño ao obxectivo
            float h = Vector2.Distance(neighbour.ToVector(), goalNode.location.ToVector());

            // O coste total estimado
            float f = g + h;

            // Crea un GameObject (visual) que mostrará G/H/F e representa este veciño
            GameObject pathBlock = Instantiate(pathP, new Vector3(neighbour.x * maze.scale, 0.0f, neighbour.z * maze.scale), Quaternion.identity);

            // Actualiza os textos do novo GameObject para mostrar G, H e F
            TextMesh[] values = pathBlock.GetComponentsInChildren<TextMesh>();
            values[0].text = "G: " + g.ToString("0.00");
            values[1].text = "H: " + h.ToString("0.00");
            values[2].text = "F: " + f.ToString("0.00");

            if (!UpdateMarker(neighbour, g, h, f, thisNode)) {
                //Se o nodo non estaba na lista de abertos, engadeo
                open.Add(new PathMarker(neighbour, g, h, f, pathBlock, thisNode));
            }
        }
        // Ordena a lista de abertos por F (custo estimado) e move o mellor nodo a `closed`.
        open = open.OrderBy(p => p.F).ToList<PathMarker>();
        PathMarker pm = (PathMarker)open.ElementAt(0);
        closed.Add(pm);
        open.RemoveAt(0);
        // Visualmente marca o nodo como pechado aplicando o material correspondente
        pm.marker.GetComponent<Renderer>().material = closedMaterial;

        // O seguinte paso da busca partirá deste nodo
        this.lastPos = pm;
    }

    // Se existe un marcador en `open` con a posición `pos`, actualiza os seus valores G,H,F e o pai.
    // Retorna true se se actualizou, false en caso contrario.
    bool UpdateMarker(MapLocation pos, float g, float h, float f, PathMarker prt) {

        foreach (PathMarker p in open) {

            if (p.location.Equals(pos)) {

                p.G = g;
                p.H = h;
                p.F = f;
                p.parent = prt;
                return true;
            }
        }
        return false;
    }

    // Comproba se `marker` está na lista de nodos pechados (closed)
    bool IsClosed(MapLocation marker) {

        foreach (PathMarker p in closed) {

            if (p.location.Equals(marker)) return true;
        }
        return false;
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.P)) {

            BeginSearch();
            hasStarted = true;
        }

        if (hasStarted)
            if (Input.GetKeyDown(KeyCode.C)) Search(this.lastPos);
    }
}
