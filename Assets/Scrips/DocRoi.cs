using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocRoi : MonoBehaviour
{
    public GameObject waterDropPrefab; // Prefab của giọt nước
    public Transform dropStartPoint;  // GameObject vị trí rơi (bắt đầu)
    public float maxHeight = 0f;      // Chiều cao điểm đến (mặt đất hoặc nơi giọt nước dừng lại)

    public float spawnInterval = 5f;  // Thời gian giữa các lần tạo giọt nước
    public float dropSpeed = 2f;      // Tốc độ rơi của giọt nước

    void Start()
    {
        InvokeRepeating("SpawnWaterDrop", 0f, spawnInterval);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
      private void SpawnWaterDrop()
    {
        if (dropStartPoint == null)
        {
            Debug.LogError("Drop Start Point is not assigned!");
            return;
        }

        // Lấy vị trí của GameObject làm điểm bắt đầu
        Vector3 startPoint = dropStartPoint.position;

        // Điểm đến là tọa độ của vị trí rơi nhưng chiều cao là maxHeight
        Vector3 endPoint = new Vector3(startPoint.x, maxHeight, startPoint.z);

        // Tạo giọt nước tại vị trí bắt đầu
        GameObject waterDrop = Instantiate(waterDropPrefab, startPoint, Quaternion.identity);

        // Gắn script để giọt nước tự di chuyển
        WaterDrop dropScript = waterDrop.AddComponent<WaterDrop>();
        dropScript.Initialize(endPoint, dropSpeed);
    }
}

