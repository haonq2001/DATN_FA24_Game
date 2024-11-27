using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ruongkhobaublock;
    public GameObject ruongkhobauopen;
    public GameObject skillpow;
    void Start()
    {
        ruongkhobaublock.SetActive(true);
        ruongkhobauopen.SetActive(false);
        skillpow.SetActive(false);
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("boxmoruong"))
        {
            ruongkhobaublock.SetActive(false);
            ruongkhobauopen.SetActive(true);
            skillpow.SetActive(true);
        }
    }
}
