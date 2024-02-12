using System.Collections;
using System.Collections.Generic;
using Common.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    
    public float spawnRate = 1.0f;

    [SerializeField] private bool randomRotation;
    
    [System.Serializable]
    public class ObjectSpawnedEvent : UnityEvent<DestructibleObject> {}
    public ObjectSpawnedEvent onObjectSpawned;
    
    private bool isSpawningActive = false;
    
    private float nextSpawnTime = 0.0f;
    
    void Update()
    {
        if (isSpawningActive && Time.time >= nextSpawnTime)
        {
            SpawnObject();
            nextSpawnTime = Time.time + (1.0f / spawnRate);
        }
    }

    public void OnSpawnObject(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            SpawnObject();
        }
    }
    
    public void SpawnObject()
    {
        Quaternion rotation = randomRotation ? Quaternion.Euler(0, Random.Range(0, 360), 0) : Quaternion.identity;
        
        GameObject spawnedObject = Instantiate(objectPrefab, transform.position, rotation);
        
        if (!spawnedObject.TryGetComponent(out DestructibleObject destructibleObject))
        {
            destructibleObject = spawnedObject.AddComponent<DestructibleObject>();
        }

        onObjectSpawned?.Invoke(destructibleObject);
    }
    
    public void StartSpawning()
    {
        isSpawningActive = true;
    }

    public void StopSpawning()
    {
        isSpawningActive = false;
    }
}
