using System;
using System.Collections.Generic;
using Common.Scripts.RevisedLevelsSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Scripts
{
    public class SimpleVegetableSpawner : MonoBehaviour
    {
        [SerializeField] private List<ConveyorObjectConfiguration> configs;
        
        [SerializeField] private AnimationCurve spawnRateCurve;
        
        private float _timeSinceLastSpawn;
        private float _spawnDelay;

        private void Start()
        {
            _spawnDelay = spawnRateCurve.Evaluate(Random.value);
        }

        private void Update()
        {
            _timeSinceLastSpawn += Time.deltaTime;
            if (_timeSinceLastSpawn >= _spawnDelay)
            {
                _timeSinceLastSpawn = 0;
                _spawnDelay = spawnRateCurve.Evaluate(Random.value);
                SpawnVegetable();
            }
        }
        
        private void SpawnVegetable()
        {
            ConveyorObjectConfiguration config = configs[Random.Range(0, configs.Count)];
            GameObject prefab = config.prefabs[Random.Range(0, config.prefabs.Count)].prefab;
            Vector3 rotation = new Vector3(Random.Range(0, 360), Random.Range(0, 360), 0);
            
            GameObject newVegetable = Instantiate(prefab, transform.position, Quaternion.Euler(rotation));
            newVegetable.transform.localScale = Vector3.one * 0.5f;
        }
    }
}