// Clase que herda de Maze
// Implementa o algoritmo recursivo para xerar o labirinto
public class Recursive : Maze
{
    // Sobreescritura do método Generate de Maze
    // Inicia a xeración a partir dunha posición por defecto
    public override void Generate()
    {
        Generate(5, 5);
    }

    // Xera camiños recursivamente a partir da celda (x, z).
    // Non avanza se a celda ten xa 2 ou máis veciños baleiros (evita loops e fíos estreitos).
    // Parámetros:
    // x - índice da fila (coordenada x)
    // z - índice da columna (coordenada z)
    void Generate(int x, int z)
    {
        if (CountSquareNeighbours(x, z) >= 2) return;
        map[x, z] = 0;

        directions.Shuffle();

        Generate(x + directions[0].x, z + directions[0].z);
        Generate(x + directions[1].x, z + directions[1].z);
        Generate(x + directions[2].x, z + directions[2].z);
        Generate(x + directions[3].x, z + directions[3].z);
    }

}
