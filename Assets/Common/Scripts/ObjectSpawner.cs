using System.Collections;
using System.Collections.Generic;
using Common.Scripts;
using Common.Scripts.RevisedLevelsSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private List<ConveyorObjectConfiguration> conveyorObjects;
    
    [SerializeField] private bool randomRotation;
    
    [System.Serializable]
    public class ObjectSpawnedEvent : UnityEvent<DestructibleObject> {}
    public ObjectSpawnedEvent onObjectSpawned;
    
    private bool isSpawningActive = false;
    private float nextSpawnTime = 0.0f;
    private float _spawnRate = 1.0f;
    private int _objectsSpawned;
    private int _amountToSpawn;
    private SpawnConfiguration _activeSpawnConfig;

    void Update()
    {
        if (isSpawningActive && Time.time >= nextSpawnTime)
        {
            SpawnObject();
            nextSpawnTime = Time.time + (1.0f / _spawnRate);
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
        
        ConveyorObjectConfiguration objectConfig = conveyorObjects[Random.Range(0, conveyorObjects.Count)];
        GameObject objectPrefab = objectConfig.GetPrefab(_activeSpawnConfig.sizeClasses[Random.Range(0, _activeSpawnConfig.sizeClasses.Count)]);

        if (objectPrefab == null)
        {
            Debug.LogWarning("[WARNING]: couldn't get a prefab for active level size classes");
            return;
        }
        
        GameObject spawnedObject = Instantiate(objectPrefab, transform.position, rotation);
        
        if (!spawnedObject.TryGetComponent(out DestructibleObject destructibleObject))
        {
            destructibleObject = spawnedObject.AddComponent<DestructibleObject>();
        }

        bool objectHasSpots = _activeSpawnConfig.spotsEnabled && Random.Range(0, 10) % 2 == 0;
        if (objectHasSpots && spawnedObject.TryGetComponent(out Spottalbe spottalbe))
        {
            spottalbe.SetSpotsVisible(true);
        }

        _objectsSpawned++;
        if (_objectsSpawned >= _amountToSpawn) StopSpawning();

        onObjectSpawned?.Invoke(destructibleObject);
    }
    
    public void StartSpawning(LevelDescriptor levelDescriptor)
    {
        _spawnRate = levelDescriptor.spawnRate;
        _amountToSpawn = levelDescriptor.amountToSpawn;
        _activeSpawnConfig = levelDescriptor.spawnConfiguration;
        
        _objectsSpawned = 0;

        isSpawningActive = true;
    }

    public void StopSpawning()
    {
        isSpawningActive = false;
    }
}
