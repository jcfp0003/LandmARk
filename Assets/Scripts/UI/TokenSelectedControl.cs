using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class TokenSelectedControl : MonoBehaviour
{
    [SerializeField] protected Image SelectionImage;
    [SerializeField] protected TMP_Text SelectionTitle;
    [SerializeField] protected TMP_Text SelectionTurns;

    public void HidePreview()
    {
        transform.localScale = Vector3.zero;

        SelectionImage.sprite = null;
        SelectionTitle.text = "";
        SelectionTurns.text = "";
    }

    public void ShowPreview(Sprite sprite, string title, string turns)
    {
        transform.localScale = Vector3.one;

        SelectionImage.sprite = sprite;
        SelectionTitle.text = title;
        SelectionTurns.text = turns;
    }
}
