using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using FMODUnity;

public class AudioController : MonoBehaviour
{
    //public EventReference PlaceTorch;

    public static AudioController GetObject()
    {
        return GameObject.Find("AudioController").GetComponent<AudioController>();
    }

    public void OnBlockDestroy(Vector3 position)
    {
        
    }

    public void OnPlaceTorch(Vector3 position)
    {
        //RuntimeManager.PlayOneShot(PlaceTorch, position);
    }

    public void OnRemoveTorch(Vector3 position)
    {
        
    }

    public void OnDigBlock(Vector3 position)
    {
        
    }

    public void OnCantDigBlock(Vector3 position)
    {
        
    }
}
