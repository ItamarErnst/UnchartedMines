using System;
using System.Collections;
using UnityEngine;

public class FloatingItem : MonoBehaviour
{ 
    public Transform holder;
    public float enterDuration = 1.0f;
    public float exitDuration = 1.0f;
    public float idleDuration = 2.0f;

    private void OnEnable()
    {
        StartCoroutine(AnimationController());
    }

    IEnumerator AnimationController()
    {
        yield return StartCoroutine(EnterAnimation());

        yield return StartCoroutine(IdleAnimation());

        yield return StartCoroutine(ExitAnimation());
        Destroy(gameObject);
    }

    IEnumerator EnterAnimation()
    {
        float time = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;
        Vector3 spawnPos = new Vector3(holder.localPosition.x, 0f, holder.localPosition.z);
        Vector3 startPos = new Vector3(holder.localPosition.x, 0.5f, holder.localPosition.z);
        Vector3 endPos = new Vector3(holder.localPosition.x, 1f, holder.localPosition.z);

        while (time < enterDuration * 1.5f)
        {
            holder.localScale = Vector3.Lerp(startScale, endScale, time / enterDuration);
            holder.localPosition = Vector3.Lerp(spawnPos, endPos, time / enterDuration);
            time += Time.deltaTime;
            yield return null;
        }

        time = 0;

        while (time < enterDuration)
        {
            holder.localPosition = Vector3.Lerp(endPos,startPos , time / enterDuration);
            time += Time.deltaTime;
            yield return null;
        }

        // Ensure that the final scale and position are exactly the target values
        holder.localScale = endScale;
        holder.localPosition = startPos;
    }

    IEnumerator IdleAnimation()
    {
        float time = 0f;

        while (time < idleDuration)
        {
            holder.localPosition = new Vector3(holder.localPosition.x, Mathf.Sin(time) * 0.2f, holder.localPosition.z);
            time += Time.deltaTime;
            yield return null;
        }
        
    }

    IEnumerator ExitAnimation()
    {
        float time = 0f;
        Vector3 startScale = holder.localScale;
        Vector3 endScale = Vector3.zero;

        while (time < exitDuration)
        {
            holder.localScale = Vector3.Lerp(startScale, startScale * 1.1f, time / exitDuration);
            time += Time.deltaTime;
            yield return null;
        }
        
        startScale = holder.localScale;
        time = 0;
        
        while (time < exitDuration)
        {
            holder.localScale = Vector3.Lerp(startScale, endScale, time / exitDuration);
            time += Time.deltaTime;
            yield return null;
        }

        holder.localScale = endScale;
    }
}