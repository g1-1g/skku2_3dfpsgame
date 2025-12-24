using UnityEngine;
using UnityEditor;

public class ClearTerrainContent : EditorWindow
{
    [MenuItem("Tools/Clear Terrain Trees and Details")]
    public static void ClearTerrain()
    {
        Terrain[] terrains = FindObjectsOfType<Terrain>();
        
        if (terrains.Length == 0)
        {
            Debug.LogError("Terrain을 찾을 수 없습니다!");
            return;
        }
        
        Terrain terrain = terrains[0];
        TerrainData terrainData = terrain.terrainData;
        
        // 모든 나무 제거
        terrainData.treeInstances = new TreeInstance[0];
        Debug.Log("모든 나무 제거 완료");
        
        // 모든 디테일 제거
        int detailWidth = terrainData.detailWidth;
        int detailHeight = terrainData.detailHeight;
        int detailPrototypeCount = terrainData.detailPrototypes.Length;
        
        for (int layer = 0; layer < detailPrototypeCount; layer++)
        {
            int[,] emptyLayer = new int[detailHeight, detailWidth];
            terrainData.SetDetailLayer(0, 0, layer, emptyLayer);
        }
        
        Debug.Log($"{detailPrototypeCount}개의 Detail 레이어 제거 완료");
        
        EditorUtility.SetDirty(terrainData);
        AssetDatabase.SaveAssets();
        
        Debug.Log("Terrain 초기화 완료!");
    }
}