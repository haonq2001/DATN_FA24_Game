using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CotDuocController : MonoBehaviour
{
    public static CotDuocController cotDuocInstance;
    private int soNgonDuocDaThap = 0; // Số ngọn đuốc đã thắp
    public Animator doorAnimator;      // Animator của cánh cửa
    // private bool isDoorOpened = false; // Kiểm tra nếu cánh cửa đã mở

    private void Awake()
    {
        if (cotDuocInstance == null)
        {
            cotDuocInstance = this;
            Debug.Log("cotDuocInstance đã được khởi tạo");
        }
        else
        {
            Debug.LogWarning("Đã có một instance của CotDuocController, đối tượng này sẽ không được sử dụng.");
             Destroy(gameObject);
        }
    }

    void Start()
{
    if (cotDuocInstance == null)
    {
        cotDuocInstance = this;
        Debug.Log("cotDuocInstance được khởi tạo trong Start.");
    }
}


    public void TangSoNgonDuocDaThap()
    {
        // if (isDoorOpened) return;

        soNgonDuocDaThap++;
        Debug.Log("Số ngọn đuốc đã thắp: " + soNgonDuocDaThap);

        if (soNgonDuocDaThap >= 5)
        {
            // isDoorOpened = true;
            doorAnimator.SetBool("OpenDoor", true);
            Debug.Log("Đủ 5 ngọn đuốc - mở cửa");
        }
    }
}
