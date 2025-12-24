using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PopulateTerrainForest : EditorWindow
{
    [MenuItem("Tools/Populate Forest Terrain")]
    public static void PopulateForest()
    {
        // Terrain 찾기
        Terrain[] terrains = FindObjectsOfType<Terrain>();
        
        if (terrains.Length == 0)
        {
            Debug.LogError("Terrain을 찾을 수 없습니다!");
            return;
        }
        
        Terrain terrain = terrains[0];
        TerrainData terrainData = terrain.terrainData;
        
        Debug.Log("Terrain 배치 시작...");
        
        // 1. 나무 배치
        PlaceTrees(terrainData);
        
        // 2. 디테일 배치 (풀, 꽃 등)
        PlaceDetails(terrainData);
        
        EditorUtility.SetDirty(terrainData);
        AssetDatabase.SaveAssets();
        
        Debug.Log("숲 배치 완료!");
    }
    
    private static void PlaceTrees(TerrainData terrainData)
    {
        List<TreeInstance> trees = new List<TreeInstance>();
        int treeCount = 0;
        
        // 중앙 영역 정의 (정규화된 좌표)
        float centerStart = 0.3f;
        float centerEnd = 0.7f;
        
        // 나무 타입 개수
        int treePrototypeCount = terrainData.treePrototypes.Length;
        
        if (treePrototypeCount == 0)
        {
            Debug.LogWarning("Tree Prototypes가 없습니다!");
            return;
        }
        
        Debug.Log($"총 {treePrototypeCount}개의 Tree Prototype 사용 가능");
        
        // 주변부 (사이드): 빽빽하게 배치
        for (int i = 0; i < 2000; i++)
        {
            float x = Random.Range(0f, 1f);
            float z = Random.Range(0f, 1f);
            
            // 중앙 영역이면 스킵
            if (x >= centerStart && x <= centerEnd && z >= centerStart && z <= centerEnd)
                continue;
            
            // 가장자리로 갈수록 더 밀도 높게
            float distanceFromCenter = Mathf.Max(
                Mathf.Abs(x - 0.5f) * 2f,
                Mathf.Abs(z - 0.5f) * 2f
            );
            
            if (Random.value > distanceFromCenter * 0.1f)
            {
                // Terrain 높이 계산 (정규화된 좌표 -> 실제 좌표 변환)
                float height = terrainData.GetHeight(
                    Mathf.RoundToInt(x * (terrainData.heightmapResolution - 1)),
                    Mathf.RoundToInt(z * (terrainData.heightmapResolution - 1))
                );
                float normalizedHeight = height / terrainData.size.y;
                
                TreeInstance tree = new TreeInstance();
                tree.position = new Vector3(x, normalizedHeight, z);
                
                // 다양한 나무 선택 (큰 나무 위주)
                int treeType;
                float rand = Random.value;
                if (rand < 0.3f)
                {
                    // 30%: Oak trees (9, 10, 11)
                    treeType = Random.Range(9, 12);
                }
                else if (rand < 0.5f)
                {
                    // 20%: Pine trees (12, 13, 14, 15)
                    treeType = Random.Range(12, 16);
                }
                else if (rand < 0.7f)
                {
                    // 20%: Deciduous trees (3, 4, 5, 6)
                    treeType = Random.Range(3, 7);
                }
                else
                {
                    // 30%: Birch and Willow (0, 1, 2, 16, 17)
                    float r = Random.value;
                    if (r < 0.6f)
                        treeType = Random.Range(0, 3); // Birch
                    else
                        treeType = Random.Range(16, 18); // Willow
                }
                
                tree.prototypeIndex = Mathf.Clamp(treeType, 0, treePrototypeCount - 1);
                tree.widthScale = Random.Range(0.8f, 1.3f);
                tree.heightScale = Random.Range(0.8f, 1.3f);
                tree.color = Color.white;
                tree.lightmapColor = Color.white;
                
                trees.Add(tree);
                treeCount++;
            }
        }
        
        // 중앙 영역: 가끔씩만 나무 배치 (듬성듬성)
        for (int i = 0; i < 50; i++)
        {
            float x = Random.Range(centerStart, centerEnd);
            float z = Random.Range(centerStart, centerEnd);
            
            // Terrain 높이 계산
            float height = terrainData.GetHeight(
                Mathf.RoundToInt(x * (terrainData.heightmapResolution - 1)),
                Mathf.RoundToInt(z * (terrainData.heightmapResolution - 1))
            );
            float normalizedHeight = height / terrainData.size.y;
            
            TreeInstance tree = new TreeInstance();
            tree.position = new Vector3(x, normalizedHeight, z);
            
            // 중앙에는 예쁜 나무들 (Willow, Birch)
            float rand = Random.value;
            int treeType;
            if (rand < 0.5f)
                treeType = Random.Range(16, 18); // Willow
            else
                treeType = Random.Range(0, 3); // Birch
            
            tree.prototypeIndex = Mathf.Clamp(treeType, 0, treePrototypeCount - 1);
            tree.widthScale = Random.Range(0.9f, 1.2f);
            tree.heightScale = Random.Range(0.9f, 1.2f);
            tree.color = Color.white;
            tree.lightmapColor = Color.white;
            
            trees.Add(tree);
            treeCount++;
        }
        
        terrainData.treeInstances = trees.ToArray();
        Debug.Log($"{treeCount}개의 나무 배치 완료");
    }
    
    private static void PlaceDetails(TerrainData terrainData)
    {
        int detailWidth = terrainData.detailWidth;
        int detailHeight = terrainData.detailHeight;
        
        int detailPrototypeCount = terrainData.detailPrototypes.Length;
        
        if (detailPrototypeCount == 0)
        {
            Debug.LogWarning("Detail Prototypes가 없습니다!");
            return;
        }
        
        Debug.Log($"총 {detailPrototypeCount}개의 Detail Prototype 사용 가능");
        
        // 중앙 영역 정의
        int centerStartX = (int)(detailWidth * 0.3f);
        int centerEndX = (int)(detailWidth * 0.7f);
        int centerStartY = (int)(detailHeight * 0.3f);
        int centerEndY = (int)(detailHeight * 0.7f);
        
        // 각 Detail 레이어별로 설정
        for (int layer = 0; layer < detailPrototypeCount; layer++)
        {
            int[,] detailLayer = new int[detailHeight, detailWidth];
            
            string detailName = terrainData.detailPrototypes[layer].prototype.name.ToLower();
            
            bool isFlower = detailName.Contains("flower") || detailName.Contains("sunflower");
            bool isGrass = detailName.Contains("grass") || detailName.Contains("fern") || detailName.Contains("plant");
            bool isRock = detailName.Contains("rock") || detailName.Contains("stone");
            bool isMushroom = detailName.Contains("mushroom");
            bool isBush = detailName.Contains("bush") || detailName.Contains("rye");
            
            for (int y = 0; y < detailHeight; y++)
            {
                for (int x = 0; x < detailWidth; x++)
                {
                    bool isCenter = (x >= centerStartX && x <= centerEndX && 
                                   y >= centerStartY && y <= centerEndY);
                    
                    if (isCenter)
                    {
                        // 중앙 영역: 꽃 위주로 듬성듬성
                        if (isFlower)
                        {
                            if (Random.value < 0.15f) // 15% 확률
                                detailLayer[y, x] = Random.Range(1, 4);
                        }
                        else if (isGrass)
                        {
                            if (Random.value < 0.08f) // 8% 확률
                                detailLayer[y, x] = Random.Range(1, 3);
                        }
                        else if (isMushroom)
                        {
                            if (Random.value < 0.03f) // 3% 확률
                                detailLayer[y, x] = 1;
                        }
                    }
                    else
                    {
                        // 주변부: 모든 요소를 빽빽하게
                        float distanceFromCenter = Mathf.Max(
                            Mathf.Abs((float)x / detailWidth - 0.5f) * 2f,
                            Mathf.Abs((float)y / detailHeight - 0.5f) * 2f
                        );
                        
                        float density = distanceFromCenter * 0.8f;
                        
                        if (isFlower)
                        {
                            if (Random.value < 0.25f * density)
                                detailLayer[y, x] = Random.Range(2, 5);
                        }
                        else if (isGrass)
                        {
                            if (Random.value < 0.35f * density)
                                detailLayer[y, x] = Random.Range(2, 6);
                        }
                        else if (isBush)
                        {
                            if (Random.value < 0.20f * density)
                                detailLayer[y, x] = Random.Range(1, 4);
                        }
                        else if (isRock)
                        {
                            if (Random.value < 0.15f * density)
                                detailLayer[y, x] = Random.Range(1, 3);
                        }
                        else if (isMushroom)
                        {
                            if (Random.value < 0.10f * density)
                                detailLayer[y, x] = Random.Range(1, 3);
                        }
                    }
                }
            }
            
            terrainData.SetDetailLayer(0, 0, layer, detailLayer);
        }
        
        Debug.Log($"{detailPrototypeCount}개의 Detail 레이어 배치 완료");
    }
}