using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritableButton : MonoBehaviour
{
    [SerializeField] protected RectTransform ButtonContent;
    [SerializeField] protected float ContentPressedOffsetY;

    public void OnHover()
    {

    }

    public void OnUnhover()
    {

    }

    public void OnDown()
    {
        ButtonContent.Translate(0, -ContentPressedOffsetY, 0);
    }

    public void OnUp()
    {
        ButtonContent.Translate(0, ContentPressedOffsetY, 0);
    }
}
