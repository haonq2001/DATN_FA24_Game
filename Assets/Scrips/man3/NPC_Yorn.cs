using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Yorn : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public string[] dialogue;
    private int dialogueIndex;
    public float typingSpeed;
    public bool playerIsClose;
    public GameObject continueButton;
     public playermap3 playermap;

    // Update is called once per frame
    void Update()
    {
        // Mở hội thoại bằng phím E
        if ((playerIsClose && Input.GetKeyDown(KeyCode.E)))
        {
            if (dialoguePanel.activeInHierarchy)
            {
                ZeroText();
            }
            else
            {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }

        // Nhấn Enter để tiếp tục nếu button đang bật
        if (dialoguePanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Return))
        {
            if (continueButton.activeSelf) // Chỉ cho phép nhấn Enter khi button đang bật
            {
                NextLine();
            }
        }

        // Kiểm tra nếu hội thoại đã gõ xong để bật button
        if (dialogueText.text == dialogue[dialogueIndex])
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            ZeroText();
        }
    }

    public void ZeroText()
    {
        dialogueText.text = "";
        dialogueIndex = 0;
        dialoguePanel.SetActive(false);
        continueButton.SetActive(false);
    }

    IEnumerator Typing()
    {
        dialogueText.text = ""; // Đảm bảo xóa nội dung cũ trước khi gõ
        foreach (char letter in dialogue[dialogueIndex].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextLine()
    {
        continueButton.SetActive(false); // Ẩn nút trước khi gõ đoạn mới
        if (dialogueIndex < dialogue.Length - 1)
        {
            dialogueIndex++; // Chuyển sang đoạn tiếp theo
            StartCoroutine(Typing());
        }
        else
        {
            ZeroText(); // Kết thúc hội thoại
        }
    }
}
