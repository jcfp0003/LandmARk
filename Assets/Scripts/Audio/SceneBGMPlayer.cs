using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneBGMPlayer : MonoBehaviour
{
    [SerializeField, Min(0.0f)] protected float PlayDelaySeconds;
    [SerializeField] protected UnityEvent PlayBGMAction;
    [SerializeField] protected bool LoopBGM;
    [SerializeField] protected float MinLoopDelay;
    [SerializeField] protected float MaxLoopDelay;

    protected float nextLoopTime = 1;

    private void Start()
    {
        StartCoroutine(PlayBGM());
    }

    private IEnumerator PlayBGM()
    {
        if(!LoopBGM)
        {
            yield return new WaitForSeconds(PlayDelaySeconds);
            PlayBGMAction.Invoke();
        }
        else
        {
            yield return new WaitForSeconds(Random.Range(MinLoopDelay, MaxLoopDelay));
            PlayBGMAction.Invoke();
            StartCoroutine(PlayBGM());
        }
    }
}
