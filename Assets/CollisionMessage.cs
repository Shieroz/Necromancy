using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionMessage : MonoBehaviour
{
    public RagdollAnimator target;

    private void OnCollisionEnter(Collision collision)
    {
        target.OnCollisionEnter(collision);
    }
}
