using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerBubbleChat : MonoBehaviour
{
    public TMP_Text bubbleText;
    public GameObject bubbleObj;
    public float displayDuration = 3f;

    private Coroutine bubbleRoutine;

    public void ShowBubble(string message)
    {
        if (bubbleRoutine != null)
            StopCoroutine(bubbleRoutine);

        bubbleRoutine = StartCoroutine(ShowBubbleRoutine(message));
    }

    IEnumerator ShowBubbleRoutine(string message)
    {
        bubbleObj.SetActive(true);
        bubbleText.text = message;
        yield return new WaitForSeconds(displayDuration);
        bubbleObj.SetActive(false);
    }
}
