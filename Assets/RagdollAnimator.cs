using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollAnimator : MonoBehaviour
{
    Component[] rigidbodies;
    public LegController leftLeg;
    public LegController rightLeg;

    [SerializeField] bool ragdoll;
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
                leftLeg.TryMove();
                yield return null;

                // Stay in this loop while this leg is moving.
            } while (leftLeg.isMoving);

            // Do the same thing for the other leg
            do
            {
                rightLeg.TryMove();
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
