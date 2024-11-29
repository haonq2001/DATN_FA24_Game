using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class muiTenBan : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject muiten;
    public Transform viTriBan;
    public float tocDoTen = 10f;
    public float tgianAn = 5f;
    public float tgianBan = 5f;
    private void Start()
    {
        
        StartCoroutine(FireArrows());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator FireArrows()
    {
        while (true)
        {
            // T?o m?i t�n
            GameObject arrow = Instantiate(muiten, viTriBan.position, viTriBan.rotation);

            // G?n Rigidbody2D ?? m?i t�n di chuy?n
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = viTriBan.right * tocDoTen; // Bay theo h??ng c?a viTriBan
            }

            // H?y m?i t�n sau arrowLifetime gi�y n?u kh�ng g?p v?t c?n
            Destroy(arrow, tgianAn);

            // ??i fireInterval gi�y tr??c khi b?n m?i t�n ti?p theo
            yield return new WaitForSeconds(tgianBan);
        }
    }
    }
