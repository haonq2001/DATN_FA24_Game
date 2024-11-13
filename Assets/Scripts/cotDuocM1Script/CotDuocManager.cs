using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CotDuocManager : MonoBehaviour
{
    public GameObject ParticleCotDuoc;
    public GameObject btnThapDuoc;
    private bool isCotDuocGan = false;
    private Button btnThapDuocComponent;
    private bool isThapDuoc = false;

    void Start()
    {
        ParticleCotDuoc.SetActive(false);
        btnThapDuoc.SetActive(false);
        btnThapDuocComponent = btnThapDuoc.GetComponent<Button>();

        if (btnThapDuocComponent != null)
        {
            btnThapDuocComponent.onClick.AddListener(ThapDuoc);
        }
    }

    void Update()
    {
        if (isCotDuocGan && Input.GetKeyDown(KeyCode.Return))
        {
            btnThapDuoc.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            isCotDuocGan = true;
            Debug.Log("Nhân vật đang ở gần cột đuốc");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            isCotDuocGan = false;
            btnThapDuoc.SetActive(false);
            Debug.Log("Nhân vật rời khỏi cột đuốc");
        }
    }

    public void ThapDuoc()
    {
        if (!isThapDuoc && isCotDuocGan)
        {
            ParticleCotDuoc.SetActive(true);
            btnThapDuoc.SetActive(false);
            isThapDuoc = true;
            Debug.Log("Thắp được cột đuốc");

            if (CotDuocController.cotDuocInstance != null)
            {
                CotDuocController.cotDuocInstance.TangSoNgonDuocDaThap();
                Debug.Log("Gọi hàm TangSoNgonDuocDaThap từ CotDuoc");
            }
            else
            {
                Debug.LogWarning("CotDuocController instance không tồn tại!");
            }
        }
    }


    // public static CotDuocManager instance;     // Singleton cho quản lý chung
    // public GameObject ParticleCotDuoc;          // Hệ thống hạt lửa cho đuốc
    // public GameObject btnThapDuoc;             // Nút "Thắp đuốc"
    // public Animator doorAnimator;              // Animator của cánh cửa

    // private int soNgonDuocDaThap = 0;          // Số ngọn đuốc đã thắp
    // private bool isCotDuocGan = false;         // Kiểm tra nếu nhân vật gần cột đuốc này
    // private bool isThapDuoc = false;           // Kiểm tra nếu cột đuốc đã được thắp sáng
    // private bool isDoorOpened = false;         // Kiểm tra nếu cánh cửa đã mở

    // void Awake()
    // {
    //     // Thiết lập Singleton
    //     if (instance == null)
    //     {
    //         instance = this;
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    // void Start()
    // {
    //     ParticleCotDuoc.SetActive(false); // Ẩn Particle khi bắt đầu
    //     btnThapDuoc.SetActive(false);     // Ẩn nút khi bắt đầu
    //     Button btnThapDuocComponent = btnThapDuoc.GetComponent<Button>();

    //     if (btnThapDuocComponent != null)
    //     {
    //         btnThapDuocComponent.onClick.AddListener(ThapDuoc);
    //     }
    // }

    // void Update()
    // {
    //     // Hiển thị nút khi nhân vật gần và nhấn phím E
    //     if (isCotDuocGan && Input.GetKeyDown(KeyCode.E) && !isThapDuoc)
    //     {
    //         btnThapDuoc.SetActive(true);
    //     }
    // }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         isCotDuocGan = true;
    //         Debug.Log("Nhân vật đang ở gần cột đuốc");
    //     }
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         isCotDuocGan = false;
    //         btnThapDuoc.SetActive(false); // Ẩn nút khi nhân vật rời khỏi cột đuốc
    //         Debug.Log("Nhân vật rời khỏi cột đuốc");
    //     }
    // }

    // public void ThapDuoc()
    // {
    //     if (!isThapDuoc) // Nếu cột đuốc chưa được thắp sáng
    //     {
    //         ParticleCotDuoc.SetActive(true); // Bật hệ thống hạt lửa
    //         btnThapDuoc.SetActive(false);    // Ẩn nút sau khi nhấn
    //         isThapDuoc = true;               // Đánh dấu rằng cột đuốc đã được thắp sáng
    //         Debug.Log("Thắp được cột đuốc");

    //         TangSoNgonDuocDaThap();          // Gọi phương thức kiểm tra mở cửa
    //     }
    // }

    // private void TangSoNgonDuocDaThap()
    // {
    //     if (isDoorOpened) return; // Nếu cửa đã mở, không thực hiện thêm

    //     soNgonDuocDaThap++;
    //     Debug.Log("Số ngọn đuốc đã thắp: " + soNgonDuocDaThap);

    //     if (soNgonDuocDaThap >= 5)
    //     {
    //         isDoorOpened = true;
    //         doorAnimator.SetBool("OpenDoor", true); // Chuyển Animator sang trạng thái mở cửa
    //         Debug.Log("Đủ 5 ngọn đuốc - mở cửa");
    //     }
    // }

}//
