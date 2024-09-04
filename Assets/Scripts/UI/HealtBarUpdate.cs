using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealtBarUpdate : MonoBehaviour
{
    [SerializeField] protected BoardToken AssignedToken;

    protected Slider SliderSelf;

    private void Start()
    {
        SliderSelf = GetComponent<Slider>();
    }

    private void Update()
    {
        SliderSelf.value = AssignedToken.GetRemainingHp01();
    }

}
