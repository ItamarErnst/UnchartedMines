using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioController : MonoBehaviour
{

  public EventReference on_block_destroy;
  public EventReference place_torch;
  public EventReference on_remove_torch;

  public EventReference on_dig_block;
    public static AudioController GetObject()
    {
        return GameObject.Find("AudioController").GetComponent<AudioController>();
    }

    public void OnBlockDestroy(Vector3 position)
    {
     RuntimeManager.PlayOneShot(on_block_destroy, position);   
    }

    public void OnPlaceTorch(Vector3 position)
    {
      RuntimeManager.PlayOneShot(place_torch, position);
    }

    public void OnRemoveTorch(Vector3 position)
    {
     RuntimeManager.PlayOneShot(on_remove_torch, position);
        
    }

    public void OnDigBlock(Vector3 position)
    {
     RuntimeManager.PlayOneShot(on_dig_block, position);       
    }



}
