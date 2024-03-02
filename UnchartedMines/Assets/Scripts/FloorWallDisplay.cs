using System.Collections.Generic;
using UnityEngine;

public class FloorWallDisplay : BaseWallDisplay
{
    public SpriteRenderer renderer;
    public List<Color> floor_color_list = new();

    public override void SetDisplay(WallData data)
    {
        base.SetDisplay(data);
        renderer.color = floor_color_list[Random.Range(0, floor_color_list.Count)];
    }
}