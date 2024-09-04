using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARProximityChecker : MonoBehaviour
{
    // #region PubSubEvents
    //
    // protected delegate void ObjectInside(GameObject obj);
    // protected static event ObjectInside ObjectInProximityEvent;
    //
    //
    // protected delegate void ObjectOutside(GameObject obj);
    // protected static event ObjectOutside ObjectOutProximityEvent;
    //
    // #endregion

    protected delegate void ObjectEnteredCallback(GameObject obj);
    protected ObjectEnteredCallback CheckEnteredCallback;
    protected delegate void ObjectExitedCallback(GameObject obj);
    protected ObjectExitedCallback CheckExitedCallback;

    [SerializeField] private Vector3 castCenterOffset;
    [SerializeField] private float spherecastRadius;
    [SerializeField] private LayerMask castLayer;
    [SerializeField] private uint castBufferSize;
    [SerializeField] private GameObject areaMarker;

    private Collider[] _castHits;
    private GameObject _currentTarget;

    protected void Awake()
    {
        _castHits = new Collider[castBufferSize];
    }

    protected void CheckProximity()
    {
        var size = Physics.OverlapSphereNonAlloc(transform.position + castCenterOffset, spherecastRadius, _castHits, castLayer);
        if (size > 0)
        {
            if(!_currentTarget)
            {
                for (var i = 0; i < size; i++)
                {
                    var castHit = _castHits[i];
                    if (!castHit.gameObject.CompareTag("ProximityTarget")) continue;

                    _currentTarget = castHit.gameObject;
                    i = size;

                    CheckEnteredCallback?.Invoke(_currentTarget);
                    areaMarker.SetActive(false);
                }

            }
        }
        else if (_currentTarget)
        {
            CheckExitedCallback?.Invoke(_currentTarget);
            _currentTarget = null;
            areaMarker.SetActive(true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + castCenterOffset, spherecastRadius);
    }
}
