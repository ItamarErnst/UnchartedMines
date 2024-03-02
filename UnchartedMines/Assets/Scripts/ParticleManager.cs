using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public Transform particle_container;
    public ParticleSystem dig_pr_prefab;
    private ParticleSystem.Burst dig_burst;
    public static ParticleManager GetObject()
    {
        return GameObject.Find("ParticleManager").GetComponent<ParticleManager>();
    }
    
    public void PlayDigParticle(Vector2 cell_world_position,int score)
    {
        ParticleSystem dig_pr = Instantiate(dig_pr_prefab,particle_container);
        dig_pr.transform.position = cell_world_position;
        
        dig_burst.count = score;
        dig_pr.emission.SetBurst(0, dig_burst);
        dig_pr.Play();
        
        Destroy(dig_pr.gameObject,2f);
    }
}
