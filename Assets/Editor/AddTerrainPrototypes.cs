using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class AddTerrainPrototypes : EditorWindow
{
    [MenuItem("Tools/Add Terrain Prototypes")]
    public static void AddPrototypes()
    {
        // Game 씬 로드
        var scene = EditorSceneManager.OpenScene("Assets/01. Scenes/Game.unity");
        
        // Terrain 찾기
        Terrain[] terrains = FindObjectsOfType<Terrain>();
        
        if (terrains.Length == 0)
        {
            Debug.LogError("Terrain을 찾을 수 없습니다!");
            return;
        }
        
        Terrain terrain = terrains[0];
        TerrainData terrainData = terrain.terrainData;
        
        Debug.Log($"Terrain 찾음: {terrain.name}");
        
        // Tree Prototypes 추가
        AddTreePrototypes(terrainData);
        
        // Detail Prototypes 추가
        AddDetailPrototypes(terrainData);
        
        EditorUtility.SetDirty(terrainData);
        AssetDatabase.SaveAssets();
        
        Debug.Log("모든 프로토타입이 성공적으로 추가되었습니다!");
    }
    
    private static void AddTreePrototypes(TerrainData terrainData)
    {
        string treePath = "Assets/10. Asset/FantasyEnvironments/Environments/Ambient-Occlusion-Trees/Prefabs";
        
        List<TreePrototype> treePrototypes = new List<TreePrototype>(terrainData.treePrototypes);
        
        string[] treePrefabs = new string[]
        {
            "Birch_tree1", "Birch_tree2", "Birch_tree3",
            "Deciduous_tree1", "Deciduous_tree2", "Deciduous_tree3", "Deciduous_tree4",
            "Deciduous_tree_stump",
            "Oak_tree1", "Oak_tree2", "Oak_tree3",
            "Pine_stump", "Pine_tree1", "Pine_tree2", "Pine_tree3",
            "Willow_tree1", "Willow_tree2"
        };
        
        foreach (string prefabName in treePrefabs)
        {
            string prefabPath = $"{treePath}/{prefabName}.prefab";
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            
            if (prefab != null)
            {
                TreePrototype treePrototype = new TreePrototype();
                treePrototype.prefab = prefab;
                treePrototype.bendFactor = 0f;
                treePrototypes.Add(treePrototype);
                Debug.Log($"Tree 추가됨: {prefabName}");
            }
            else
            {
                Debug.LogWarning($"Tree 프리팹을 찾을 수 없음: {prefabPath}");
            }
        }
        
        terrainData.treePrototypes = treePrototypes.ToArray();
        Debug.Log($"총 {treePrefabs.Length}개의 Tree Prototypes 추가 완료");
    }
    
    private static void AddDetailPrototypes(TerrainData terrainData)
    {
        string detailPath = "Assets/10. Asset/FantasyEnvironments/Environments/Prefabs";
        
        List<DetailPrototype> detailPrototypes = new List<DetailPrototype>(terrainData.detailPrototypes);
        
        string[] detailPrefabs = new string[]
        {
            "Bush1",
            "Fern1", "Fern2", "Fern3",
            "Flower1", "Flower2", "Flower3", "Flower4", "Flower5", "Flower6", "Flower7", "Flower8", "Flower9",
            "Grass1", "Grass2", "Grass3", "Grass4",
            "Mushroom1", "Mushroom2", "Mushroom3", "Mushroom4", "Mushroom5",
            "Plant1", "Plant2", "Plant3", "Plant4", "Plant5",
            "Rock1", "Rock2", "Rock3",
            "Rye",
            "Stone1", "Stone1_detail", "Stone2", "Stone3",
            "Sunflower"
        };
        
        foreach (string prefabName in detailPrefabs)
        {
            string prefabPath = $"{detailPath}/{prefabName}.prefab";
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            
            if (prefab != null)
            {
                DetailPrototype detailPrototype = new DetailPrototype();
                detailPrototype.prototype = prefab;
                detailPrototype.renderMode = DetailRenderMode.VertexLit;
                detailPrototype.usePrototypeMesh = true;
                detailPrototype.minHeight = 0.5f;
                detailPrototype.maxHeight = 1.0f;
                detailPrototype.minWidth = 0.5f;
                detailPrototype.maxWidth = 1.0f;
                detailPrototype.healthyColor = Color.white;
                detailPrototype.dryColor = new Color(0.8f, 0.8f, 0.6f);
                detailPrototypes.Add(detailPrototype);
                Debug.Log($"Detail 추가됨: {prefabName}");
            }
            else
            {
                Debug.LogWarning($"Detail 프리팹을 찾을 수 없음: {prefabPath}");
            }
        }
        
        terrainData.detailPrototypes = detailPrototypes.ToArray();
        Debug.Log($"총 {detailPrefabs.Length}개의 Detail Prototypes 추가 완료");
    }
}