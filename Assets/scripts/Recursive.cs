/// <summary>
/// Esta clase herda de Maze
/// </summary>
public class Recursive : Maze
{
    /// <summary>
    /// Sobreescritura do método Generate da clase Maze
    /// </summary>
    public override void Generate()
    {
        Generate(5, 5);
    }

    /// <summary>
    /// Crear os camiños na escea para un tamaño concreto
    /// </summary>
    /// <param name="x">Coordenada x de inicio</param>
    /// <param name="z">Coordenada z de inicio</param>
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
