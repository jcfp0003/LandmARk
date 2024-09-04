using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class MovePositionMarker : MonoBehaviour
{
    [SerializeField] protected Material MainColorMaterial;
    [SerializeField] protected Material AlteColorMaterial;

    protected LineRenderer lineRenderer;

    public Vector2Int CellIndex;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void RetargetLine(float3 beginPosition, float3 targetPosition)
    {
        lineRenderer.SetPosition(0, beginPosition);
        lineRenderer.SetPosition(1, targetPosition);
    }

    public void RetargetLine(Vector3 beginPosition, Vector3 targetPosition)
    {
        RetargetLine(
            new float3(beginPosition.x, beginPosition.y, beginPosition.z),
            new float3(targetPosition.x, targetPosition.y, targetPosition.z));
    }

    public void RetargetLine(Transform beginPosition, Transform targetTransform)
    {
        RetargetLine(beginPosition.position, targetTransform.position);
    }

    public void SwapToMaterial(bool toAlt = false)
    {
        lineRenderer.material = toAlt ? AlteColorMaterial : MainColorMaterial;
    }
}
