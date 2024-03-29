﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WallDisplay : BaseWallDisplay
{
    public Transform holder;
    
    private ParticleManager particleManager;
    public Material default_material;

    public List<Color> pixel_color_list = new List<Color>();
    public List<Color> fogged_pixel_color_list = new List<Color>();
    public List<SpriteRenderer> pixels_renderer_list = new();

    public float shakeDuration = 0.2f;
    public float shakeIntensity = 0.1f;
    
    private void Awake()
    {
        particleManager = ParticleManager.GetObject();
    }

    public void Dig(int current_hits)
    {
        float hit_presentage = GetPercentageOfHits(current_hits);
        
        StartShakeAnimation();
        DisableLinearSpriteRenderers(hit_presentage);
        particleManager.PlayDigParticle(transform.position,GetHitPercentageDifference(current_hits));
    }
    
    public override void SetDisplay(WallData data)
    {
        base.SetDisplay(data);
        holder.transform.localPosition = Vector3.zero;
        SetPixelColors();
        DisableLinearSpriteRenderers(GetPercentageOfHits(data.currentHits));
    }

    protected virtual void SetPixelColors()
    {
        foreach (SpriteRenderer pixel in pixels_renderer_list)
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
    
    void DisableLinearSpriteRenderers(float percentage)
    {
        int numberOfRenderersToDisable = GetHitPercentageToInt(percentage);
        
        for (int i = 0; i < pixels_renderer_list.Count; i++)
        {
            pixels_renderer_list[i].enabled = i >= numberOfRenderersToDisable;
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
        int prev_power = PlayerStats.GetDigDamage() * (PlayerStats.GetPlayerSTR() - 1);
        int currentHits = GetHitPercentageToInt(GetPercentageOfHits(hits - prev_power));
        int newHits = GetHitPercentageToInt(GetPercentageOfHits(hits));
        
        return Mathf.Max(1,newHits - currentHits);
    }

    public override void ChangeColors(BaseWallDisplay pooled_object)
    {
        WallDisplay pooled_wall_display = pooled_object.GetComponent<WallDisplay>();
        
        base.ChangeColors(pooled_object);
        
        pixel_color_list = pooled_wall_display.pixel_color_list;
        fogged_pixel_color_list = pooled_wall_display.fogged_pixel_color_list;
    }

    public void StartShakeAnimation()
    {
        StartCoroutine(AnimateHit());
    }

    private IEnumerator AnimateHit()
    {
        Vector3 originalPosition = Vector3.zero;

        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = originalPosition.x + Random.Range(-1f, 1f) * shakeIntensity;
            float y = originalPosition.y + Random.Range(-1f, 1f) * shakeIntensity;

            holder.transform.localPosition = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        holder.transform.localPosition = Vector3.zero;
    }
}