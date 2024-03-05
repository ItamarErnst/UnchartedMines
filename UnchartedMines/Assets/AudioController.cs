using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


public class AudioController : MonoBehaviour
{

public EventReference PlaceTorch;

    public static AudioController GetObject()
    {
        return GameObject.Find("AudioController").GetComponent<AudioController>();
    }

    public void OnBlockDestroy()
    {
        
    }

    public void OnPlaceTorch()
    {
        RuntimeManager.PlayOneShot(PlaceTorch, gameObject.transform.position);
        
    }

    public void OnRemoveTorch()
    {
        
    }

    public void OnDigBlock()
    {
        
    }

    public void OnCantDigBlock()
    {
        
    }
}
