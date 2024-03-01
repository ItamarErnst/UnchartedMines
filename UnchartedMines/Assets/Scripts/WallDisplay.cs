using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WallDisplay : BaseWallDisplay
{
    public ParticleSystem dig_pr;
    private ParticleSystem.Burst dig_burst;

    public List<Color> pixel_color_list = new List<Color>();
    public List<SpriteRenderer> pixels_renderer_list = new();

    public void Dig(int current_hits)
    {
        float hit_presentage = GetPercentageOfHits(current_hits);
        
        DisableLinearSpriteRenderers(hit_presentage);
        PlayDigParticle(GetHitPercentageDifference(current_hits));
    }
    
    public override void SetDisplay(WallData data)
    {
        base.SetDisplay(data);
        SetPixelColors();

        if (wallData.currentHits <= 0)
        {
            Reset();
        }
    }

    public void Reset()
    {
        DisableLinearSpriteRenderers(0);
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

        for (int i = 0; i < numberOfRenderersToDisable; i++)
        {
            int indexToDisable = i % pixels_renderer_list.Count;
            pixels_renderer_list[indexToDisable].enabled = false;
        }
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
    
    public void PlayDigParticle(int score)
    {
        dig_burst.count = score;
        dig_pr.emission.SetBurst(0, dig_burst);
        dig_pr.Play();
    }
}