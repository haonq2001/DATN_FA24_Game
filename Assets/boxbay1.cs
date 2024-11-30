using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxbay1 : MonoBehaviour
{
    public GameObject boxbay;
    // Start is called before the first frame update
    void Start()
    {
        boxbay.SetActive(false);
    }

    // Update is called once per frame

    public void boxbayon()
    {
        boxbay.SetActive(true);
    }
    public void boxbayoff()
    {
        boxbay.SetActive(false);
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        animator.SetTrigger("dizzy");
    //        audioManager.Instance.PlaySFX("bidau");
    //        //   StartCoroutine(WaitForDeathAnimation());
    //    }
    //}
}
