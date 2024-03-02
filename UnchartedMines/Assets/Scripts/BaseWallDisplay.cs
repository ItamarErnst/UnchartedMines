using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseWallDisplay : MonoBehaviour
{
    protected WallData wallData = null;
    public Animation animation;

    public virtual void SetDisplay(WallData data)
    {
        wallData = data;
    }
    
    public virtual void ChangeColors(BaseWallDisplay pooled_object)
    {
        
    }
    
    public void PlaySpawn(bool instant = false)
    {
        if (instant)
        {
            animation.Play("WallSpawn");
        }
        else
        {
            StartCoroutine(DelayedAnimation());
        }
    }

    private IEnumerator DelayedAnimation()
    {
        yield return new WaitForSeconds(Random.Range(0.05f, 0.5f));
        animation.Play("WallSpawn");
    }
}