using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegTargetCoordinator4Limb : MonoBehaviour
{
    public LegController4Limb leg;
    // Update is called once per frame
    void Update()
    {
        if (!leg.isMoving)
        {
            transform.position = leg.currentPosition;
        }
    }
}
