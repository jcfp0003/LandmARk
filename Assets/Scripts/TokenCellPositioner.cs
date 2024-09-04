using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class TokenCellPositioner : MonoBehaviour
{
    [SerializeField] protected LayerMask GroundLayerMask;
    public BoardToken AssignedTokenObject;

    private RaycastHit[] _hits;

    private void Start()
    {
        _hits = new RaycastHit[1];
    }

    private void FixedUpdate()
    {
        Ray testRay = new Ray(transform.position, transform.up);
        int hitCount = Physics.RaycastNonAlloc(testRay, _hits, float.MaxValue, GroundLayerMask, QueryTriggerInteraction.Collide);

        if (hitCount == 0)
        {
            hitCount = Physics.RaycastNonAlloc(new Ray(transform.position, -transform.up), _hits, float.MaxValue, GroundLayerMask, QueryTriggerInteraction.Collide);
        }

        if (hitCount > 0)
        {
            BoardCell hitCell = _hits[0].collider.GetComponent<BoardCell>();
            TokenMoveRegister.CurrentBoardMovements[AssignedTokenObject] = hitCell;
        }
    }
}
