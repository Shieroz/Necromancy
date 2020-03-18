using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollAnimator4Limb : MonoBehaviour
{
    Component[] rigidbodies;
    public LegController4Limb leftLeg;
    public LegController4Limb rightLeg;
    public LegController4Limb leftArm;
    public LegController4Limb rightArm;

    [SerializeField] bool ragdoll;

    public float moveDuration = 0.2f;
    public bool Ragdoll
    {
        set
        {
            ragdoll = value;
            setKinematic(!value);
        }
        get
        {
            return ragdoll;
        }
    }

    private void Awake()
    {
        StartCoroutine(Walk());
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>(true))
        {
            if (r.tag == "Gizmos")
            {
                r.enabled = false;
            }
        }
        rigidbodies = GetComponentsInChildren<Rigidbody>(true);
        Ragdoll = false;
    }

    IEnumerator Walk()
    {
        // Run continuously
        while (!Ragdoll)
        {
            // Try moving one leg
            do
            {
                leftLeg.TryWalk(moveDuration);
                rightArm.TryWalk(moveDuration);
                yield return null;

                // Stay in this loop while this leg is moving.
            } while (leftLeg.isMoving);

            // Do the same thing for the other leg
            do
            {
                rightLeg.TryWalk(moveDuration);
                leftArm.TryWalk(moveDuration);
                yield return null;
            } while (rightLeg.isMoving);
        }
    }

    public void OnCollisionEnter(Collision col)
    {
        if ((col.impulse / Time.fixedDeltaTime).magnitude > 200f)
            Ragdoll = true;
    }

    void setKinematic(bool active)
    {
        foreach (Rigidbody r in rigidbodies)
        {
            r.isKinematic = active;
        }
    }
}
