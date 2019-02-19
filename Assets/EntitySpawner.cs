﻿using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    public GameObject entity;
    [Range(0.0f,1.0f)]
    public float ChanceSpawn;

    private void Awake() {
        if (BoltNetwork.IsServer) {
            if (Random.Range(0.0f,1.0f) < ChanceSpawn) {
                Debug.LogFormat("Spawning {0}", entity.name);
                BoltNetwork.Instantiate(entity, transform.position, Quaternion.identity);
            }
        }
    }
}