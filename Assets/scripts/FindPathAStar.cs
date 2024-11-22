using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindPathAStar : MonoBehaviour {

    public Maze maze;
    public Material closedMaterial;
    public Material openMaterial;
    public GameObject start;
    public GameObject end;
    public GameObject pathP;//GameObject para representar o camiño

    PathMarker startNode;
    PathMarker goalNode;
    PathMarker lastPos; //Última posición para explorar
    bool done = false;
    bool hasStarted = false;

    List<PathMarker> open = new List<PathMarker>();
    List<PathMarker> closed = new List<PathMarker>();

    /// <summary>
    /// Borrar todos os "marker" que hai na escea
    /// </summary>
    void RemoveAllMarkers() {

        GameObject[] markers = GameObject.FindGameObjectsWithTag("marker");

        foreach (GameObject m in markers) Destroy(m);
    }

    /// <summary>
    /// Reinicia a búsqueda do camiño
    /// Preparación para o algoritmo A*
    /// </summary>
    void BeginSearch() {

        done = false;
        //Borra todos os marcadores da escea
        RemoveAllMarkers();

        //Crea unha lista de posibles localizacións para os marcadores
        List<MapLocation> locations = new List<MapLocation>();

        //Recorre o mapa e se a posición está libre engade a posición á lista de localizacións
        for (int z = 1; z < maze.depth - 1; ++z) {
            for (int x = 1; x < maze.width - 1; ++x) {

                if (maze.map[x, z] != 1) {

                    locations.Add(new MapLocation(x, z));
                }
            }
        }

        //Mezcla as localizacións
        locations.Shuffle();

        //Utiliza a primeira localización como startNode
        Vector3 startLocation = new Vector3(locations[0].x * maze.scale, 0.0f, locations[0].z * maze.scale);
        startNode = new PathMarker(new MapLocation(locations[0].x, locations[0].z),
            0.0f, 0.0f, 0.0f, Instantiate(start, startLocation, Quaternion.identity), null);

        //Utiliza a segunda localización como stopNode
        Vector3 endLocation = new Vector3(locations[1].x * maze.scale, 0.0f, locations[1].z * maze.scale);
        goalNode = new PathMarker(new MapLocation(locations[1].x, locations[1].z),
            0.0f, 0.0f, 0.0f, Instantiate(end, endLocation, Quaternion.identity), null);

        //Limpa a lista de nodos abertos
        open.Clear();
        //Limpa a lista de nodos pechados
        closed.Clear();

        //Mete na lista de nodos abertos o nodo de comezo
        open.Add(startNode);

        //Marca como ultimo nodo visitiado o startNode
        this.lastPos = startNode;
    }

    /// <summary>
    /// Fai a búsqueda a partir do nodo thisNode
    /// </summary>
    /// <param name="thisNode"></param>
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

            //Se o veciño é unha parede non fai nada
            if (maze.map[neighbour.x, neighbour.z] == 1) continue;
            //Se o veciño sae dos límites do xogo, non fai nada
            if (neighbour.x < 1 || neighbour.x >= maze.width || neighbour.z < 1 || neighbour.z >= maze.depth) continue;
            //Se o veciño xa é un nodo pechado, non fai nada
            if (IsClosed(neighbour)) continue;

            //Esta é a clave do algoritmo
            // g =  g(de este nodo) + distancia ao veciño
            float g = Vector2.Distance(thisNode.location.ToVector(), neighbour.ToVector()) + thisNode.G;
            // h = distancia do veciño ao obxectivo
            float h = Vector2.Distance(neighbour.ToVector(), goalNode.location.ToVector());

            // O coste total estimado
            float f = g + h;

            //Crea un GameObject para representar o novo marker
            GameObject pathBlock = Instantiate(pathP, new Vector3(neighbour.x * maze.scale, 0.0f, neighbour.z * maze.scale), Quaternion.identity);

            //Actualiza os textos do novo GameObject
            TextMesh[] values = pathBlock.GetComponentsInChildren<TextMesh>();

            values[0].text = "G: " + g.ToString("0.00");
            values[1].text = "H: " + h.ToString("0.00");
            values[2].text = "F: " + f.ToString("0.00");

            if (!UpdateMarker(neighbour, g, h, f, thisNode)) {
                //Se o nodo non estaba na lista de abertos, engadeo
                open.Add(new PathMarker(neighbour, g, h, f, pathBlock, thisNode));
            }
        }
        //Ordea a lista de nodos abertos utilizando o valor de f
        open = open.OrderBy(p => p.F).ToList<PathMarker>();

        //Extrae o primeiro nodo da lista de abertos e meteo na lista de pechados
        PathMarker pm = (PathMarker)open.ElementAt(0);
        closed.Add(pm);
        open.RemoveAt(0);
        pm.marker.GetComponent<Renderer>().material = closedMaterial;

        //A partir de este  nodo seguirá a búsqueda.
        this.lastPos = pm;
    }

    /// <summary>
    /// Actualiza o marcador da lista aberta cos valores indicados
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="g"></param>
    /// <param name="h"></param>
    /// <param name="f"></param>
    /// <param name="prt"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Devolce se o marcador está na lista de marcadores pechados
    /// </summary>
    /// <param name="marker"></param>
    /// <returns></returns>
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
