using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndicatorControl : MonoBehaviour
{
    [Header("State materials")]
    [SerializeField] protected Material baseMaterial;
    [SerializeField] protected Material activeMaterial;

    [Header("Gameobjects")]
    [SerializeField] protected MeshRenderer[] targetMeshes;

    public void SwapMaterial(bool isActive)
    {
        if (isActive)
        {
            foreach (MeshRenderer mesh in targetMeshes)
            {
                mesh.material = activeMaterial;
            }
        } else
        {
            foreach (MeshRenderer mesh in targetMeshes)
            {
                mesh.material = baseMaterial;
            }
        }
    }

}
