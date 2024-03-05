using FMODUnity;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public EventReference on_block_destroy;
    public EventReference place_torch;
    public EventReference on_remove_torch;
    public EventReference on_dig_block;

    public static AudioManager GetObject()
    {
        return GameObject.Find("AudioManager").GetComponent<AudioManager>();
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
    
    public void OnCantDigBlock(Vector3 position)
    {
        //Do not delete functions in use, it will cause problems
        //Keep it empty or tell me to delete for now :D
    }
}