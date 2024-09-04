using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ARBoardPlacement : MonoBehaviour
{
    [SerializeField] protected AnchorBehaviour GameBoardTransform;

    // Update is called once per frame
    void Update()
    {
        GameBoardTransform.transform.SetPositionAndRotation(transform.position, transform.rotation);
    }

    public void SetupBoardAnchor()
    {
        GameBoardTransform.UnconfigureAnchor();
        GameBoardTransform.ConfigureAnchor("arboard", transform.position, transform.rotation);
    }
}
