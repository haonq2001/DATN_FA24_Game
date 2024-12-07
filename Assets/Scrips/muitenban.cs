using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class muitenban : MonoBehaviour
{
    public GameObject arrowPrefab; // Prefab của mũi tên
    public Transform viTriBan;     // Vị trí bắn
    public float fireInterval = 5f; // Thời gian giữa các lần bắn
    public float arrowSpeed = 10f;  // Tốc độ bay của mũi tên
    public float arrowLifetime = 5f; // Thời gian tồn tại tối đa của mũi tên nếu không va chạm
    public float chieuCaoToiDa = 5f; // Chiều cao tối đa
    public float chieuCaoToiThieu = 0f; // Chiều cao tối thiểu
    public float speeddeten = 2f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FireArrowRoutine());
        StartCoroutine(DeTenDiChuyen());
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator FireArrowRoutine()
    {
        while (true)
        {
            // Tạo mũi tên từ vị trí bắn
            GameObject arrow = Instantiate(arrowPrefab, viTriBan.position, viTriBan.rotation);

            // Lấy Rigidbody2D để gán vận tốc
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = viTriBan.right * arrowSpeed; // Mũi tên bay theo hướng của viTriBan
            }

            // Hủy mũi tên sau thời gian tồn tại tối đa nếu không va chạm
            Destroy(arrow, arrowLifetime);

            // Chờ trước khi bắn mũi tên tiếp theo
            yield return new WaitForSeconds(fireInterval);
        }
    }
    private IEnumerator DeTenDiChuyen()
    {
        while (true)
        {
            yield return StartCoroutine(MoveToHeight(chieuCaoToiThieu, chieuCaoToiDa));
            yield return StartCoroutine(MoveToHeight(chieuCaoToiDa, chieuCaoToiThieu));

        }

    }
    private IEnumerator MoveToHeight(float startHeight, float endHeight)
    {
        float elapsedTime = 0f;
        float journeyLength = Mathf.Abs(endHeight - startHeight);

        Vector3 startPosition = new Vector3(transform.position.x, startHeight, transform.position.z);
        Vector3 endPosition = new Vector3(transform.position.x, endHeight, transform.position.z);

        while (elapsedTime < journeyLength / speeddeten)
        {
            // Tính vị trí hiện tại dựa trên thời gian
            transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime * speeddeten) / journeyLength);
            elapsedTime += Time.deltaTime;
            yield return null; // Chờ cho đến khung hình tiếp theo
        }

        // Đảm bảo đối tượng đứng chính xác tại vị trí cuối
        transform.position = endPosition;
    }
}