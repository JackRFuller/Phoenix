using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpingClasses
{
    public float lerpingSpeed = 1;
    public bool isLerping = false;
    public float timeStartedLerping;

    public float percentageComplete;

    public void CalculatePercentageComplete()
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        percentageComplete = timeSinceStarted / lerpingSpeed;
    }

    public class Vector3Lerping : LerpingClasses
    {
        public Vector3 startPosition;
        public Vector3 targetPosition;

        public Vector3 ReturnLerpingPosition()
        {
            CalculatePercentageComplete();
            return Vector3.Lerp(startPosition, targetPosition,percentageComplete);
        }
    }
}
