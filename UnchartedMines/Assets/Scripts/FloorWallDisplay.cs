    using System.Collections.Generic;
    using UnityEngine;

    public class FloorWallDisplay : BaseWallDisplay
    {
        public SpriteRenderer renderer;
        public List<Color> floor_color_list = new();
        public GameObject torch;
        public override void SetDisplay(WallData data)
        {
            base.SetDisplay(data);
            renderer.color = floor_color_list[Random.Range(0, floor_color_list.Count)];
            torch.SetActive(data.wallType == WallType.Torch);
            if(data.wallType == WallType.Torch)
            {
            torch.SetActive(true);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Sfx/Ligh", GetComponent<Transform>().position); 
            }


            else
            {torch.SetActive(false);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Sfx/LighOff", GetComponent<Transform>().position);
            }
        }
    }