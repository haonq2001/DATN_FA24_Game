using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CotDuoc : MonoBehaviour
{
    public GameObject ParticleCotDuoc;
    public GameObject btnThapDuoc;
    private bool isCotDuocGan = false;
      private Button btnThapDuocComponent;

    void Start()
    {
        ParticleCotDuoc.SetActive(false);
        btnThapDuoc.SetActive(false);
        
        Button btnThapDuocComponent = btnThapDuoc.GetComponent<Button>();

        if (btnThapDuocComponent != null)
        {
            btnThapDuocComponent.onClick.AddListener(ThapDuoc);
        } else
        {
            Debug.LogError("btnThapDuocComponent is null, check if btnThapDuoc has Button component.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isCotDuocGan && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("bật btn Thap duoc");
            btnThapDuoc.SetActive(true);
           
        }

        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isCotDuocGan = true;
            Debug.Log("Cot duoc gan");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isCotDuocGan = false;
        }
    }

    public void ThapDuoc()
    {
        ParticleCotDuoc.SetActive(true);
        btnThapDuoc.SetActive(false);
    }
}
