using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component allows the player to move by clicking the arrow keys,
 * but only if the new position is on an allowed tile.
 */
public class KeyboardMoverByTile: KeyboardMover {
    [SerializeField] Tilemap tilemap = null;
    [SerializeField] AllowedTiles allowedTiles = null;
    [SerializeField] TileBase grassTile = null;
    [SerializeField] TileBase mountainTile = null;

    private TileBase TileOnPosition(Vector3 worldPosition) {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        return tilemap.GetTile(cellPosition);
    }

    private void UpdateTile(Vector3 worldPosition, TileBase newTile) {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        tilemap.SetTile(cellPosition, newTile);
    }

    void Update() {
        Vector3 newPosition = NewPosition();
        TileBase tileOnNewPosition = TileOnPosition(newPosition);
        
        if (allowedTiles.Contains(tileOnNewPosition)) {
            // Move to the new position
            transform.position = newPosition;
            GameObject pick = GameObject.Find("pick");
            
            if (tileOnNewPosition == mountainTile && pick == null) {
                // If the new tile is a mountain, change it to grass
                UpdateTile(newPosition, grassTile);
            }
        } else {
            Debug.Log("You cannot walk on " + tileOnNewPosition + "!");
        }
    }
}

