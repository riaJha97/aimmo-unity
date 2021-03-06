using UnityEngine;
using System;

public class TerrainGenerator
{
    public const float TerrainScalingFactor = 0.1f;
    public const float TerrainSnapToGridShift = 0.5f;
    private GameObject terrainGameObject;
    public GameObject terrainFolder;

    public TerrainGenerator(GameObject terrainFolder = null, GameObject terrain = null)
    {
        this.terrainFolder = terrainFolder ?? GetTerrainFolder();
        this.terrainGameObject = terrain ?? GetTerrainGameObject();
    }

    public GameObject GenerateTerrain(TerrainDTO terrain)
    {
        terrainFolder = terrainFolder ?? GetTerrainFolder();
        terrainGameObject = terrainGameObject ?? GetTerrainGameObject();
        Vector3 newScale = new Vector3(TerrainScalingFactor * terrain.width,
                                       TerrainScalingFactor,
                                       TerrainScalingFactor * terrain.height);
        if (terrainGameObject.transform.localScale != newScale)
        {
            float shiftX = terrain.width % 2 == 0 ? TerrainSnapToGridShift : 0f;
            float shiftZ = terrain.height % 2 == 0 ? TerrainSnapToGridShift : 0f;
            terrainFolder.transform.localPosition = new Vector3(shiftX, 0f, shiftZ);
            terrainGameObject.transform.localScale = newScale;
        }
        return terrainGameObject;
    }

    public GameObject GenerateTerrainMMO(TerrainDTO terrainDTO)
    {
        // TODO: make this dynamic for other types of terrains.
        GameObject terrainPrefab = Resources.Load<GameObject>("Prefabs/Terrains/terrain_future_default");
        terrainGameObject = UnityEngine.Object.Instantiate(terrainPrefab) as GameObject;
        terrainGameObject.transform.SetParent(terrainFolder.transform, false);
        terrainGameObject.transform.localPosition = new Vector3(0, 0, 0);

        GenerateTerrain(terrainDTO);

        UpdateTerrainMaterial(terrainDTO);

        return terrainGameObject;
    }

    public int CalculateHeightFromGameState(GameStateDTO gameState)
    {
        int height = Math.Abs(gameState.northEastCorner.y - gameState.southWestCorner.y) + 1;

        return height;
    }

    public int CalculateWidthFromGameState(GameStateDTO gameState)
    {
        int width = Math.Abs(gameState.northEastCorner.x - gameState.southWestCorner.x) + 1;

        return width;
    }

    private GameObject GetTerrainGameObject()
    {
        return GameObject.FindWithTag("Terrain");
    }

    private GameObject GetTerrainFolder()
    {
        return GameObject.Find("Terrain");
    }

    private void UpdateTerrainMaterial(TerrainDTO terrain)
    {
        Material material = terrainGameObject.GetComponent<Renderer>().sharedMaterial;
        material.mainTextureScale = new Vector2(terrain.width, terrain.height);
    }
}
