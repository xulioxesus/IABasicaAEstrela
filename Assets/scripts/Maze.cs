using System.Collections.Generic;
using UnityEngine;

// Clase base para xerar e debuxar un labirinto simple en escena.
// - `map` almacena o estado das celas: 1 = parede, 0 = corredor.
// - As coordenadas do mapa usan índices enteiros (x, z). O valor `scale`
//   controla o tamaño en unidades Unity de cada cela cando se crea o cubo parede.
public class Maze : MonoBehaviour
{
    // Lista de movementos en cruz (dereita, arriba, esquerda, abaixo) utilizada para explorar veciños
    public List<MapLocation> directions = new List<MapLocation>() {
                                            new MapLocation(1,0),
                                            new MapLocation(0,1),
                                            new MapLocation(-1,0),
                                            new MapLocation(0,-1) };

    // Dimensións do mapa en número de celas
    public int width = 30; // lonxitude en X (número de columnas)
    public int depth = 30; // lonxitude en Z (número de filas)
    
    // Mapa de bytes: 1 = parede, 0 = corredor
    // Accédese como map[x, z]
    public byte[,] map;

    // Tamaño en unidades Unity dunha cela ao crear o cubo parede
    public int scale = 6;

    // Método Unity que se executa ao iniciar: prepara o mapa, xenera e debuxa o labirinto
    void Start()
    {
        InitialiseMap();
        Generate();
        DrawMap();
    }

    // Inicializa o array `map` e marca todas as celas como parede (1).
    // Este método pode ser chamado para reiniciar o estado do mapa antes de xerar.
    void InitialiseMap()
    {
        map = new byte[width,depth];
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                    map[x, z] = 1;     // 1 = parede, 0 = corredor
            }
    }

    // Xera un mapa inicial asignando a cada cela, por defecto, un 50% de probabilidades de ser corredor (0).
    // Sobrescribible (virtual) para permitir algoritmos de xeración máis avanzados en clases derivadas.
    // Postcondición: `map` estará poboado con valores 0 (camiños) ou 1 (paredes).
    public virtual void Generate()
    {
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
               if(Random.Range(0,100) < 50)
                    map[x, z] = 0;     // 1 = parede, 0 = corredor
            }
    }

    // Recorre `map` e crea un `GameObject` (cube) para cada parede (valor 1).
    // Os cubos son creados na posición (x * scale, 0, z * scale) e escalados a (scale, scale, scale).
    // Nota: en proxectos reais sería mellor reutilizar prefabs e evitar CreatePrimitive en tempo de execución frecuente.
    void DrawMap()
    {
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                if (map[x, z] == 1)
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.localScale = new Vector3(scale, scale, scale);
                    wall.transform.position = pos;
                }
            }
    }

    // Conta cantas celas veciñas en cruz (N/S/E/O) son corredores (0).
    // Se a posición dada está no bordo (non ten todos os veciños) devolve 5 como marcador de caso non válido.
    // Retorno: 0..4 (número de veciños libres) ou 5 se a posición está no bordo.
    // Parámetros:
    // x - índice X (columna)
    // z - índice Z (fila)
    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z] == 0) count++;
        if (map[x + 1, z] == 0) count++;
        if (map[x, z + 1] == 0) count++;
        if (map[x, z - 1] == 0) count++;
        return count;
    }

    // Conta cantas das catro diagonais (NE, NW, SE, SW) son corredores (0).
    // Devolve 5 se a posición está no bordo (non se poden consultar as catro diagonais).
    public int CountDiagonalNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z - 1] == 0) count++;
        if (map[x + 1, z + 1] == 0) count++;
        if (map[x - 1, z + 1] == 0) count++;
        if (map[x + 1, z - 1] == 0) count++;
        return count;
    }

    // Conta todos os veciños libres (cruz + diagonais). Pode devolver 10 se algunha das funcións
    // anteriores devolveu 5 por estar no bordo.
    public int CountAllNeighbours(int x, int z)
    {
        return CountSquareNeighbours(x,z) + CountDiagonalNeighbours(x,z);
    }
}
