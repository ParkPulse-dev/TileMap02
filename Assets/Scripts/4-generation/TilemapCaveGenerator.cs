using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;


public class TilemapCaveGenerator : MonoBehaviour
{
    [SerializeField] Tilemap tilemap = null;
    [SerializeField] TileBase wallTile = null;
    [SerializeField] TileBase floorTile = null;
    [Range(0, 1)][SerializeField] float randomFillPercent = 0.5f;
    [SerializeField] int gridSize = 100;
    [SerializeField] int simulationSteps = 20;
    [SerializeField] float pauseTime = 1f;
    [SerializeField] GameObject playerPrefab = null; // Serialized field for the player prefab.
    private CaveGenerator caveGenerator;
    private TilemapGraph tilemapGraph;

    void Start()
    {
        Random.InitState(100);
        caveGenerator = new CaveGenerator(randomFillPercent, gridSize);
        caveGenerator.RandomizeMap();
        GenerateAndDisplayTexture(caveGenerator.GetMap());
        StartCoroutine(SimulateCavePattern());

        PlacePlayerRandomly(); // Call the method to place the player randomly.

        // Create tilemap graph using allowed tiles (floor tiles)
        TileBase[] allowedTiles = { floorTile };
        tilemapGraph = new TilemapGraph(tilemap, allowedTiles);

        // Calculate the number of different paths from the player's position to itself
        Vector3Int playerPosition = GetPlayerPosition();
        int pathCount = BFSPATHS.CountPaths(tilemapGraph, playerPosition, playerPosition);
        Debug.Log("Number of different paths from player's position to itself: " + pathCount);

        if (pathCount == 1)
        {
            PlacePlayerRandomly();
            // pathCount = BFSPATHS.CountPaths(tilemapGraph, playerPosition, playerPosition);
            Debug.Log("Number of different paths from player's position to itself: " + pathCount);
        }


    }

    private Vector3Int GetPlayerPosition()
    {
        // Assuming playerPrefab has been instantiated and is not null
        return tilemap.WorldToCell(playerPrefab.transform.position);
    }

    private IEnumerator SimulateCavePattern()
    {
        for (int i = 0; i < simulationSteps; i++)
        {
            yield return new WaitForSeconds(pauseTime);
            caveGenerator.SmoothMap();
            GenerateAndDisplayTexture(caveGenerator.GetMap());
        }
        Debug.Log("Simulation completed!");
    }

    private void GenerateAndDisplayTexture(int[,] data)
    {
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                var position = new Vector3Int(x, y, 0);
                var tile = data[x, y] == 1 ? wallTile : floorTile;
                tilemap.SetTile(position, tile);
            }
        }
    }

    private void PlacePlayerRandomly()
    {
        // Get random coordinates within the bounds of the tilemap.
        Vector3Int randomPosition = new Vector3Int(Random.Range(0, gridSize), Random.Range(0, gridSize), 0);

        // Convert the position to world coordinates.
        Vector3 worldPosition = tilemap.CellToWorld(randomPosition);

        // Instantiate the player prefab at the random position.
        Instantiate(playerPrefab, worldPosition, Quaternion.identity);

        // Log the position where the player is being placed.
        Debug.Log("Player placed at: " + worldPosition);
    }
}
