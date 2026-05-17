using UnityEngine;
using System.Collections;
using System;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement instance;
    public float slideDuration = 1f;
    public AnimationCurve slideCurve;
    private bool isSliding = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    public void SlideTo(Vector3 targetPos, Action onComplete = null)
    {
        if (!isSliding)
        {
            StartCoroutine(SlideRoutine(targetPos, onComplete));
        }
    }

    private IEnumerator SlideRoutine(Vector3 target, Action onComplete = null)
    {
        isSliding = true;
        Vector3 start = transform.position;
        float passed = 0f;

        while (passed < slideDuration)
        {
            passed += Time.deltaTime;
            float t = slideCurve.Evaluate(passed / slideDuration);
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }
        transform.position = target;
        isSliding = false;
        onComplete?.Invoke();
    }
}
