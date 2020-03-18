using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegController4Limb : MonoBehaviour
{
    public Transform IKTarget;
    public Transform forwardTarget;
    public Transform backwardTarget;
    public Transform restTarget;

    public Transform hip;

    public float separateDistance = 0.8f;
    public float stepOvershootFraction;
    public Vector3 currentPosition;
    public bool isMoving;
    // Start is called before the first frame update
    void Start()
    {
        currentPosition = IKTarget.position;
        isMoving = false;
    }

    public void TryWalk(float moveDuration)
    {
        if (isMoving) return;

        Vector3 moveDir = hip.position - lastPos;

        if (hip.position != lastPos)
        {
            if (Vector3.Angle(hip.forward, moveDir) <= 90f)
            {
                if ((IKTarget.position - forwardTarget.position).magnitude > separateDistance)
                {
                    StartCoroutine(moveToTarget(forwardTarget, moveDuration));
                }
            }
            else
            {
                if ((IKTarget.position - backwardTarget.position).magnitude > separateDistance)
                {
                    StartCoroutine(moveToTarget(backwardTarget, moveDuration));
                }
            }
        }
        else
        {
            StartCoroutine(moveToTarget(restTarget, moveDuration));
        }
    }

    Vector3 lastPos;
    private void LateUpdate()
    {
        lastPos = hip.position;
    }

    IEnumerator moveToTarget(Transform target, float moveDuration)
    {
        isMoving = true;

        //Store initial and end transforms
        Quaternion startRot = IKTarget.rotation;
        Vector3 startPoint = IKTarget.position;

        Quaternion endRot = target.rotation;
        // Directional vector from the foot to the home position
        Vector3 towardTarget = (target.position - IKTarget.position);
        // Total distnace to overshoot by   
        float overshootDistance = separateDistance * stepOvershootFraction;
        Vector3 overshootVector = towardTarget * overshootDistance;
        // Since we don't ground the point in this simplified implementation,
        // we restrict the overshoot vector to be level with the ground
        // by projecting it on the world XZ plane.
        overshootVector = Vector3.ProjectOnPlane(overshootVector, Vector3.up);

        Vector3 randomeOffset = hip.right * Random.Range(-0.05f, 0.05f) + hip.forward * Random.Range(-0.05f, 0.05f); //NOTE: Add randomness for spice

        Vector3 endPoint = target.position + overshootVector;
        if (target.name != "RestTarget")
            endPoint += randomeOffset;
        currentPosition = endPoint;

        // We want to pass through the center point
        Vector3 centerPoint = (startPoint + endPoint) / 2;
        // But also lift off, so we move it up by half the step distance (arbitrarily)
        centerPoint += target.up * Vector3.Distance(startPoint, endPoint) / 2f;

        float timeElapsed = 0;
        float normalizedTime;

        do
        {
            // Add time since last frame to the time elapsed
            timeElapsed += Time.deltaTime;
            normalizedTime = timeElapsed / moveDuration;

            // Quadratic bezier curve
            IKTarget.position =
              Vector3.Lerp(
                Vector3.Lerp(startPoint, centerPoint, normalizedTime),
                Vector3.Lerp(centerPoint, endPoint, normalizedTime),
                normalizedTime
              );
            IKTarget.rotation = Quaternion.Slerp(startRot, endRot, normalizedTime);

            // Wait for one frame
            yield return null;
        }
        while (timeElapsed < moveDuration);

        // Done moving
        isMoving = false;
    }
}
