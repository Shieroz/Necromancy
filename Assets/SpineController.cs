using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineController : MonoBehaviour
{
    public Transform IKTarget;
    public float separateDistance = 0.04f;
    // Start is called before the first frame update
    IEnumerator moveToTarget(Transform target, float moveDuration)
    {
        //Store initial and end transforms
        Quaternion startRot = IKTarget.rotation;
        Vector3 startPoint = IKTarget.position;

        Quaternion endRot = target.rotation;
        // Directional vector from the foot to the home position
        Vector3 towardTarget = (target.position - IKTarget.position);
        // Total distnace to overshoot by   
        float overshootDistance = separateDistance;
        Vector3 overshootVector = towardTarget * overshootDistance;
        // Since we don't ground the point in this simplified implementation,
        // we restrict the overshoot vector to be level with the ground
        // by projecting it on the world XZ plane.
        overshootVector = Vector3.ProjectOnPlane(overshootVector, Vector3.up);

        Vector3 endPoint = target.position + overshootVector;

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
    }
}
