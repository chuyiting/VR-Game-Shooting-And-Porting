using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{

    public bool IsOpen = false;
    public float Speed = 1f;
    public Vector3 SlideDirection = Vector3.up;
    public float SlideAmount = 1.9f;

    private Vector3 StartPosition;
    private Coroutine AnimationCoroutine;


    void Awake()
    {
        StartPosition = transform.position;
    }


    public void Open()
    {
        if (!IsOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }
            AnimationCoroutine = StartCoroutine(DoSlidingOpen());

        }
    }

    public void Close()
    {
        if (IsOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            AnimationCoroutine = StartCoroutine(DoSlidingClose());
        }
    }
    private IEnumerator DoSlidingOpen()
    {
        Vector3 endPosition = StartPosition + SlideAmount * SlideDirection;
        Vector3 startPosition = transform.position;

        float time = 0;
        IsOpen = true;
        while (time < 1)
        {
            Vector3 move = Vector3.Lerp(startPosition, endPosition, time) - transform.position;
            transform.position = Vector3.Lerp(startPosition, endPosition, time);
            foreach (Transform child in transform)
            {
                child.position += move;
            }

            yield return null;
            time += Time.deltaTime * Speed;
        }
    }

    private IEnumerator DoSlidingClose()
    {
        Vector3 endPosition = StartPosition;
        Vector3 startPosition = transform.position;

        float time = 0;
        IsOpen = false;
        while (time < 1)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }
    }
}
