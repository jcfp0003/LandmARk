using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;

    [SerializeField] private float height;
    [SerializeField] private float animationTime;
    [SerializeField] private Ease appearEase;
    [SerializeField] private Ease disappearEase;

    public void SetupIndicator(string damageValue, Transform origin)
    {
        damageText.text = damageValue;

        transform.position = origin.position;
        transform.localScale = Vector3.zero;

        Sequence tweenSequence = DOTween.Sequence();
        tweenSequence.Append(transform.DOMoveY(transform.position.y + height, animationTime).SetEase(appearEase));
        tweenSequence.Join(transform.DOScale(Vector3.one, animationTime).SetEase(appearEase));
        tweenSequence.Append(transform.DOMoveY(transform.position.y, animationTime).SetEase(disappearEase));
        tweenSequence.Join(transform.DOScale(Vector3.zero, animationTime).SetEase(disappearEase));

        tweenSequence.OnComplete(() =>
        {
            Destroy(this.gameObject);
        });

        tweenSequence.Play();
    }
}
