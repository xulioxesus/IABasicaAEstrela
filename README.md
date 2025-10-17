### IABasicaAEstrela

Proxecto de exemplo en Unity que combina xeración de labirintos e visualización do algoritmo A* para a procura de camiños.

#### Estrutura principal

`Assets/Scripts/` - scripts C# relevantes:

- `Maze.cs` - clase base para xerar e debuxar un labirinto simple (map como grid de bytes: 1 = parede, 0 = corredor). Contén métodos para inicializar o mapa, xerar un estado inicial e debuxar paredes como cubos.

- `Recursive.cs` - derivada de `Maze` que implementa unha xeración recursiva do labirinto (evita loops comprobando veciños).

- `FindPathAStar.cs` - compoñente que mostra paso a paso o algoritmo A* sobre un `Maze`. Selecciona dous puntos libres (inicio e fin), crea `PathMarker` para cada nodo e actualiza G/H/F visualmente.

- `PathMarker.cs` - estrutura que representa un nodo no algoritmo A* (contén `location`, `G`, `H`, `F`, `marker` e `parent`).

- `MapLocation.cs` - pequena clase que representa unha posición no grid (x, z) e provee utilidades como conversión a `Vector2` e operador +.

#### Comportamento das clases de IA e camiños

Xeración do labirinto:

- `Maze` proporciona utilidades básicas: crear o mapa, marcar paredes/camiños e debuxar cubos para paredes.

- `Recursive` substitúe `Generate()` para aplicar un algoritmo recursivo que crea camiños máis estruturados.

Busca de camiños (A*):

- `FindPathAStar` recolle todas as celas libres do `Maze`, escolla dous puntos aleatorios (inicio e destino) e executa o algoritmo A* paso a paso.

- Os nodos do algoritmo represéntanse con `PathMarker` e utilízanse listas `open` e `closed` para xestionar o estado.

- Para cada novo nodo visualízanse G, H e F mediante `TextMesh` asociados ao `pathP` prefab.

#### Uso rápido

1. Abre a escena no Editor de Unity.
2. Engade o compoñente `FindPathAStar` a un GameObject na escena e asigna:
   - `Maze` (referencia ao obxecto que contén o script `Maze`/`Recursive`).
   - Prefabs/Materiales: `start`, `end`, `pathP`, `openMaterial`, `closedMaterial`.
3. No modo Play, preme `P` para iniciar a busca e `C` para executar un paso do algoritmo A*.