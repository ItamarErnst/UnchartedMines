using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController GetObject()
    {
        return GameObject.Find("AudioController").GetComponent<AudioController>();
    }

    public void OnBlockDestroy()
    {
        
    }

    public void OnPlaceTorch()
    {
        
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
