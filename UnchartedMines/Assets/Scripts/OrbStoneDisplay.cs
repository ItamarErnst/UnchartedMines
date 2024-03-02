using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbStoneDisplay : WallDisplay
{
    public Material orb_material;

    public Color orb_color;
    public List<SpriteRenderer> orb_pixels_renderer_list = new();

    protected override void SetPixelColors()
    {
        foreach (SpriteRenderer pixel in pixels_renderer_list)
        {
            if (orb_pixels_renderer_list.Contains(pixel))
            {
                pixel.color = orb_color;
                pixel.material = orb_material;
            }
            else
            {
                if (wallData.fogged)
                {
                    pixel.color = fogged_pixel_color_list[Random.Range(0, fogged_pixel_color_list.Count)];
                    pixel.material = default_material;
                }
                else
                {
                    pixel.color = pixel_color_list[Random.Range(0, pixel_color_list.Count)];
                    pixel.material = default_material;
                }
            }
        }        
    }

    public override void ChangeColors(BaseWallDisplay pooled_object)
    {
        //OrbStoneDisplay pooled_ore_display = pooled_object.GetComponent<OrbStoneDisplay>();
        //
        //pixel_color_list = pooled_ore_display.pixel_color_list;
        //fogged_pixel_color_list = pooled_ore_display.fogged_pixel_color_list;
    }
}
