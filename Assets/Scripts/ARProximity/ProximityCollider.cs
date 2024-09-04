using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityCollider : ARProximityChecker
{
    private GameObject _nearObject;
    private LineRenderer _nearMarkers;

    private void Start()
    {
        _nearMarkers = GetComponent<LineRenderer>();
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
        _nearObject = obj;
    }

    private void ProximityTargetExited(GameObject obj)
    {
        _nearObject = null;
        _nearMarkers.positionCount = 0;
    }

    private void FixedUpdate()
    {
        CheckProximity();

        if (_nearObject)
        {
            _nearMarkers.positionCount = 2;
            _nearMarkers.SetPosition(0, transform.position);
            _nearMarkers.SetPosition(1, _nearObject.transform.position);
        }
    }
}
