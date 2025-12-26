using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LampTreePlacerEditor : EditorWindow
{
    private GameObject lampPrefab;
    private Transform player;
    private float searchRadius = 50f;
    private float lampHeightOffset = 3f;
    private float lampDistanceFromTree = 0.5f;
    private List<string> targetTreeNames = new List<string> { "Cedar_Tree_03", "Pine_Tree", "Larch_Tree" };
    
    [MenuItem("Tools/Lamp Tree Placer")]
    static void Init()
    {
        LampTreePlacerEditor window = (LampTreePlacerEditor)EditorWindow.GetWindow(typeof(LampTreePlacerEditor));
        window.titleContent = new GUIContent("Lamp Tree Placer");
        window.Show();
    }
    
    void OnGUI()
    {
        GUILayout.Label("Lamp Tree Placer Settings", EditorStyles.boldLabel);
        
        lampPrefab = (GameObject)EditorGUILayout.ObjectField("Lamp Prefab", lampPrefab, typeof(GameObject), false);
        player = (Transform)EditorGUILayout.ObjectField("Player", player, typeof(Transform), true);
        searchRadius = EditorGUILayout.FloatField("Search Radius", searchRadius);
        lampHeightOffset = EditorGUILayout.FloatField("Lamp Height Offset", lampHeightOffset);
        lampDistanceFromTree = EditorGUILayout.FloatField("Distance From Tree", lampDistanceFromTree);
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Target Trees:", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Cedar_Tree_03, Pine_Tree, Larch_Tree");
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Load Lamp Prefab"))
        {
            lampPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/03. Prefabs/Lamp.prefab");
            if (lampPrefab != null)
            {
                Debug.Log("Lamp prefab loaded successfully!");
            }
        }
        
        if (GUILayout.Button("Find Player"))
        {
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                Debug.Log("Player found!");
            }
        }
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Clear All Lamps"))
        {
            ClearAllLamps();
        }
        
        if (GUILayout.Button("Place Lamps on Trees"))
        {
            PlaceLampsOnTrees();
        }
    }
    
    void ClearAllLamps()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> lampsToDelete = new List<GameObject>();
        
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("Lamp"))
            {
                lampsToDelete.Add(obj);
            }
        }
        
        foreach (GameObject lamp in lampsToDelete)
        {
            DestroyImmediate(lamp);
        }
        
        Debug.Log($"Deleted {lampsToDelete.Count} lamp objects");
    }
    
    void PlaceLampsOnTrees()
    {
        if (lampPrefab == null)
        {
            EditorUtility.DisplayDialog("Error", "Please load or assign a Lamp prefab first!", "OK");
            return;
        }
        
        if (player == null)
        {
            EditorUtility.DisplayDialog("Error", "Please find or assign Player first!", "OK");
            return;
        }
        
        // 기존 Lamp들 제거
        ClearAllLamps();
        
        // 모든 Terrain 찾기
        Terrain[] terrains = GameObject.FindObjectsOfType<Terrain>();
        
        if (terrains.Length == 0)
        {
            EditorUtility.DisplayDialog("Error", "No Terrain found in the scene!", "OK");
            return;
        }
        
        Vector3 playerPos = player.position;
        int totalLamps = 0;
        int checkedTrees = 0;
        
        // Lamp들을 담을 부모 오브젝트 생성
        GameObject lampParent = new GameObject("TreeLamps");
        
        foreach (Terrain terrain in terrains)
        {
            TerrainData terrainData = terrain.terrainData;
            Vector3 terrainPosition = terrain.transform.position;
            
            // 나무 인스턴스 가져오기
            TreeInstance[] trees = terrainData.treeInstances;
            
            Debug.Log($"{terrain.name}: {trees.Length} trees found");
            
            foreach (TreeInstance tree in trees)
            {
                // 나무의 월드 좌표 계산
                Vector3 treeWorldPos = Vector3.Scale(tree.position, terrainData.size) + terrainPosition;
                
                // 나무 타입 확인
                TreePrototype treePrototype = terrainData.treePrototypes[tree.prototypeIndex];
                string treeName = treePrototype.prefab.name;
                
                // 특정 나무 타입만 처리
                bool isTargetTree = false;
                foreach (string targetName in targetTreeNames)
                {
                    if (treeName.Contains(targetName))
                    {
                        isTargetTree = true;
                        break;
                    }
                }
                
                if (!isTargetTree)
                    continue;
                
                checkedTrees++;
                
                // Player 주변의 나무만 처리
                float distanceToPlayer = Vector3.Distance(treeWorldPos, playerPos);
                if (distanceToPlayer > searchRadius)
                    continue;
                
                // Terrain 높이 샘플링
                float terrainHeight = terrain.SampleHeight(treeWorldPos);
                
                // Lamp 배치 위치 계산
                Vector3 lampPosition = treeWorldPos;
                lampPosition.y = terrainPosition.y + terrainHeight + lampHeightOffset;
                
                // 나무에서 약간 떨어진 위치에 배치
                float randomAngle = Random.Range(0f, 360f);
                Vector3 offset = new Vector3(
                    Mathf.Cos(randomAngle * Mathf.Deg2Rad),
                    0,
                    Mathf.Sin(randomAngle * Mathf.Deg2Rad)
                ) * lampDistanceFromTree;
                
                lampPosition += offset;
                
                // Lamp 생성
                GameObject lamp = (GameObject)PrefabUtility.InstantiatePrefab(lampPrefab);
                lamp.transform.position = lampPosition;
                lamp.transform.parent = lampParent.transform;
                lamp.name = $"Lamp_{treeName}_{totalLamps}";
                
                // Lamp가 나무를 향하도록 회전 (Y축 기준)
                Vector3 directionToTree = treeWorldPos - lampPosition;
                directionToTree.y = 0;
                if (directionToTree != Vector3.zero)
                {
                    lamp.transform.rotation = Quaternion.LookRotation(directionToTree);
                }
                
                Undo.RegisterCreatedObjectUndo(lamp, "Create Lamp");
                totalLamps++;
            }
        }
        
        Debug.Log($"Checked {checkedTrees} target trees (Cedar_Tree_03, Pine_Tree, Larch_Tree)");
        Debug.Log($"Placed {totalLamps} lamps on trees near player!");
        EditorUtility.DisplayDialog("Success", $"Placed {totalLamps} lamps on {checkedTrees} target trees near the player!", "OK");
    }
}
