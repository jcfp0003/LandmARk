using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityMerger : ARProximityChecker
{
    [SerializeField] private GameObject resultingObject;
    [SerializeField] private bool replaceOnSelf;

    private bool _targetMergeObjectSet;
    private Transform _targetMergeObjectTrf;
    private GameObject _resultGameObject;

    private void Start()
    {
        _targetMergeObjectSet = false;
        _targetMergeObjectTrf = null;
    }

    private void OnEnable()
    {
        CheckEnteredCallback = ProximityTargetEntered;
        CheckExitedCallback = ProximityTargetExited;
    }

    private void OnDisable()
    {
        CheckEnteredCallback = null;
        CheckExitedCallback = null;
    }

    private void ProximityTargetEntered(GameObject obj)
    {
        if (_targetMergeObjectSet)
        {
            return;
        }

        _targetMergeObjectTrf = obj.transform;
        _targetMergeObjectSet = true;

        transform.GetChild(0).gameObject.SetActive(false);
        _targetMergeObjectTrf.GetChild(0).gameObject.SetActive(false);
        // transform.GetChild(1).gameObject.SetActive(false);
        _resultGameObject = Instantiate(resultingObject, replaceOnSelf ? transform : _targetMergeObjectTrf);
    }

    private void ProximityTargetExited(GameObject obj)
    {
        if (!_targetMergeObjectSet)
        {
            return;
        }

        Destroy(_resultGameObject);
        transform.GetChild(0).gameObject.SetActive(true);
        // transform.GetChild(1).gameObject.SetActive(true);
        _targetMergeObjectTrf.GetChild(0).gameObject.SetActive(true);
        
        _targetMergeObjectSet = false;
        _targetMergeObjectTrf = null;
    }

    private void FixedUpdate()
    {
        CheckProximity();
    }

}
