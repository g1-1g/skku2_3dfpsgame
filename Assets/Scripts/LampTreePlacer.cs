using UnityEngine;
using System.Collections.Generic;

public class LampTreePlacer : MonoBehaviour
{
    [Header("References")]
    public GameObject lampPrefab;
    public Transform player;
    public float searchRadius = 50f; // Player 주변 검색 반경
    
    [Header("Lamp Settings")]
    public float lampHeightOffset = 3f; // 나무 기준 램프 높이
    public float lampDistanceFromTree = 0.5f; // 나무 중심에서 램프까지 거리
    
    private List<GameObject> spawnedLamps = new List<GameObject>();
    
    void Start()
    {
        // Lamp prefab 자동 로드
        if (lampPrefab == null)
        {
            lampPrefab = Resources.Load<GameObject>("Prefabs/Lamp");
            if (lampPrefab == null)
            {
                Debug.LogError("Lamp prefab을 찾을 수 없습니다! Prefabs 폴더를 확인하세요.");
                return;
            }
        }
        
        // Player 자동 찾기
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null)
            {
                player = GameObject.Find("Player")?.transform;
            }
        }
        
        // 기존 Lamp들 제거
        ClearExistingLamps();
        
        // 나무에 Lamp 배치
        PlaceLampsOnTrees();
    }
    
    void ClearExistingLamps()
    {
        GameObject[] existingLamps = GameObject.FindGameObjectsWithTag("Untagged");
        foreach (GameObject obj in existingLamps)
        {
            if (obj.name.Contains("Lamp"))
            {
                DestroyImmediate(obj);
            }
        }
    }
    
    void PlaceLampsOnTrees()
    {
        // 모든 Terrain 찾기
        Terrain[] terrains = FindObjectsOfType<Terrain>();
        
        if (terrains.Length == 0)
        {
            Debug.LogError("Terrain을 찾을 수 없습니다!");
            return;
        }
        
        Vector3 playerPos = player != null ? player.position : Vector3.zero;
        int totalLamps = 0;
        
        foreach (Terrain terrain in terrains)
        {
            TerrainData terrainData = terrain.terrainData;
            Vector3 terrainPosition = terrain.transform.position;
            
            // 나무 인스턴스 가져오기
            TreeInstance[] trees = terrainData.treeInstances;
            
            Debug.Log($"{terrain.name}에서 {trees.Length}개의 나무를 찾았습니다.");
            
            foreach (TreeInstance tree in trees)
            {
                // 나무의 월드 좌표 계산
                Vector3 treeWorldPos = Vector3.Scale(tree.position, terrainData.size) + terrainPosition;
                
                // Player 주변의 나무만 처리
                float distanceToPlayer = Vector3.Distance(treeWorldPos, playerPos);
                if (distanceToPlayer > searchRadius)
                    continue;
                
                // Lamp 배치 위치 계산 (나무 옆쪽에 배치)
                Vector3 lampPosition = treeWorldPos;
                lampPosition.y += lampHeightOffset; // 높이 조정
                
                // 나무에서 약간 떨어진 위치에 배치 (랜덤 방향)
                float randomAngle = Random.Range(0f, 360f);
                Vector3 offset = new Vector3(
                    Mathf.Cos(randomAngle * Mathf.Deg2Rad),
                    0,
                    Mathf.Sin(randomAngle * Mathf.Deg2Rad)
                ) * lampDistanceFromTree;
                
                lampPosition += offset;
                
                // Lamp 생성
                GameObject lamp = Instantiate(lampPrefab, lampPosition, Quaternion.identity);
                lamp.transform.parent = transform; // 이 스크립트의 오브젝트를 부모로 설정
                lamp.name = $"Lamp_Tree_{totalLamps}";
                
                // Lamp가 나무를 향하도록 회전
                Vector3 directionToTree = treeWorldPos - lampPosition;
                directionToTree.y = 0; // 수평 방향만
                if (directionToTree != Vector3.zero)
                {
                    lamp.transform.rotation = Quaternion.LookRotation(directionToTree);
                }
                
                spawnedLamps.Add(lamp);
                totalLamps++;
            }
        }
        
        Debug.Log($"총 {totalLamps}개의 Lamp를 배치했습니다.");
    }
    
    // Inspector에서 실행할 수 있는 헬퍼 메서드
    [ContextMenu("Refresh Lamps")]
    void RefreshLamps()
    {
        ClearSpawnedLamps();
        PlaceLampsOnTrees();
    }
    
    void ClearSpawnedLamps()
    {
        foreach (GameObject lamp in spawnedLamps)
        {
            if (lamp != null)
            {
                DestroyImmediate(lamp);
            }
        }
        spawnedLamps.Clear();
    }
    
    void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            // 검색 반경 시각화
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.position, searchRadius);
        }
    }
}
