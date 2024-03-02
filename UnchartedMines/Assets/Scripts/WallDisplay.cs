using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WallDisplay : BaseWallDisplay
{
    private ParticleManager particleManager;

    public List<Color> pixel_color_list = new List<Color>();
    public List<SpriteRenderer> pixels_renderer_list = new();

    private void Awake()
    {
        particleManager = ParticleManager.GetObject();
    }

    public void Dig(int current_hits)
    {
        float hit_presentage = GetPercentageOfHits(current_hits);
        
        DisableLinearSpriteRenderers(hit_presentage);
        particleManager.PlayDigParticle(transform.position,GetHitPercentageDifference(current_hits));
    }
    
    public override void SetDisplay(WallData data)
    {
        base.SetDisplay(data);
        SetPixelColors();

        DisableLinearSpriteRenderers(GetPercentageOfHits(data.currentHits));
    }

    private void SetPixelColors()
    {
        foreach (SpriteRenderer pixel in pixels_renderer_list)
        {
            pixel.color = pixel_color_list[Random.Range(0, pixel_color_list.Count)];
        }
    }
    
    void DisableLinearSpriteRenderers(float percentage)
    {
        int numberOfRenderersToDisable = GetHitPercentageToInt(percentage);
        
        for (int i = 0; i < pixels_renderer_list.Count; i++)
        {
            pixels_renderer_list[i].enabled = i >= numberOfRenderersToDisable;
        }
        
        //int numberOfRenderersToDisable = GetHitPercentageToInt(percentage);
        //for (int i = 0; i < numberOfRenderersToDisable; i++)
        //{
        //    int indexToDisable = i % pixels_renderer_list.Count;
        //    pixels_renderer_list[indexToDisable].enabled = false;
        //}
    }
    
    public float GetPercentageOfHits(float current)
    {
        return (float)current / wallData.hitsRequired;
    }

    public int GetHitPercentageToInt(float current)
    {
        return Mathf.RoundToInt(pixels_renderer_list.Count * current);
    }
    
    public int GetHitPercentageDifference(float hits)
    {
        int currentHits = GetHitPercentageToInt(GetPercentageOfHits(hits - 1));
        int newHits = GetHitPercentageToInt(GetPercentageOfHits(hits));
        
        return newHits - currentHits;
    }

    public override void ChangeColors(BaseWallDisplay pooled_object)
    {
        WallDisplay pooled_wall_display = pooled_object.GetComponent<WallDisplay>();
        
        base.ChangeColors(pooled_object);
        
        pixel_color_list = pooled_wall_display.pixel_color_list;
    }
}