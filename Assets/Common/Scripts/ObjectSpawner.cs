using System;
using System.Collections;
using System.Collections.Generic;
using Common.Scripts;
using Common.Scripts.RevisedLevelsSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private List<ConveyorObjectConfiguration> conveyorObjects;
    
    [SerializeField] private bool randomRotation;
    
    [System.Serializable]
    public class ObjectSpawnedEvent : UnityEvent<DestructibleObject> {}
    public ObjectSpawnedEvent onObjectSpawned;
    public UnityEvent<DestructibleObject> onSpawnedObjectDestroyed;
    
    private bool isSpawningActive = false;
    private float nextSpawnTime = 0.0f;
    private int _objectsSpawned;
    private LevelDescriptor _activeLevelDescriptor;
    
    private List<DestructibleObject> _spawnedObjects = new();

    private List<bool> objectValidity = new();

    void Update()
    {
        if (isSpawningActive && Time.time >= nextSpawnTime)
        {
            SpawnObject();
            nextSpawnTime = Time.time + (1.0f / _activeLevelDescriptor.spawnRate);
        }
    }

    private void OnDestroy()
    {
        foreach (var spawnedObject in _spawnedObjects)
        {
            if (spawnedObject != null) spawnedObject.onObjectDestroyed.RemoveListener(OnSpawnedObjectDestroyed);
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
        DestructibleObject destructibleObject = SpawnAndConfigureNewDestructibleObject(objectValidity[_objectsSpawned]);
        if (destructibleObject == null)
        {
            Debug.LogWarning("[WARNING]: failed to spawn a new object");
            return;
        }

        _objectsSpawned++;

        if (_objectsSpawned >= _activeLevelDescriptor.amountToSpawn) StopSpawning();

        onObjectSpawned?.Invoke(destructibleObject);
    }

    private DestructibleObject SpawnAndConfigureNewDestructibleObject(bool isValid)
    {
        Quaternion rotation = randomRotation ? Quaternion.Euler(0, Random.Range(0, 360), 0) : Quaternion.identity;
        
        ConveyorObjectSizeClass sizeClass = GetSizeClass(isValid);
        ConveyorObjectConfiguration objectConfig = conveyorObjects[Random.Range(0, conveyorObjects.Count)];
        GameObject objectPrefab = objectConfig.GetPrefab(sizeClass);

        if (objectPrefab == null)
        {
            Debug.LogWarning("[WARNING]: couldn't get a prefab for active level size classes");
            return null;
        }

        GameObject spawnedObject = Instantiate(objectPrefab, transform.position, rotation);
        if (!spawnedObject.TryGetComponent(out DestructibleObject destructibleObject))
        {
            destructibleObject = spawnedObject.AddComponent<DestructibleObject>();
        }
        
        destructibleObject.onObjectDestroyed.AddListener(OnSpawnedObjectDestroyed);
        
        _spawnedObjects.Add(destructibleObject);
        
        ConfigureObjectSpots(destructibleObject, isValid);

        return destructibleObject;
    }

    private ConveyorObjectSizeClass GetSizeClass(bool isValid)
    {
        var allSizeClasses = (ConveyorObjectSizeClass[])Enum.GetValues(typeof(ConveyorObjectSizeClass));
        
        // if the current rule set contains all size classes or none of them just return a random one
        if (_activeLevelDescriptor.ruleSet.allowedSizes.Count == allSizeClasses.Length || _activeLevelDescriptor.ruleSet.allowedSizes.Count == 0)
        {
            return allSizeClasses[Random.Range(0, allSizeClasses.Length)];
        }
        
        ConveyorObjectSizeClass sizeClass;
        
        do {
            sizeClass = allSizeClasses[Random.Range(0, allSizeClasses.Length)];
        } while (_activeLevelDescriptor.IsSizeClassValid(sizeClass) != isValid);
        
        return sizeClass;
    }

    private void ConfigureObjectSpots(DestructibleObject destructibleObject, bool isValid)
    {
        bool isSpotsAllowed = _activeLevelDescriptor.ruleSet.spotsAllowed;
        bool objectHasSpots = isValid ? isSpotsAllowed : !isSpotsAllowed;
        if (objectHasSpots && destructibleObject.TryGetComponent(out Spottalbe spottalbe))
        {
            spottalbe.SetSpotsVisible(true);
        }
    }
    
    public void StartSpawning(LevelDescriptor levelDescriptor)
    {
        PrepareForSpawn(levelDescriptor);
        isSpawningActive = true;
    }

    public void PrepareForSpawn(LevelDescriptor levelDescriptor)
    {
        _activeLevelDescriptor = levelDescriptor;

        int validObjectsCount = (int)(_activeLevelDescriptor.amountToSpawn * _activeLevelDescriptor.validObjectsPercentage);
        
        // create an array of validities
        objectValidity.Clear();
        for (int i = 0; i < _activeLevelDescriptor.amountToSpawn; i++)
        {
            objectValidity.Add(i < validObjectsCount);
        }
        
        // shuffle the array
        for (int i = 0; i < objectValidity.Count; i++)
        {
            bool temp = objectValidity[i];
            int randomIndex = Random.Range(i, objectValidity.Count);
            objectValidity[i] = objectValidity[randomIndex];
            objectValidity[randomIndex] = temp;
        }
        
        _objectsSpawned = 0;
    }

    public void StopSpawning()
    {
        isSpawningActive = false;
    }

    private void OnSpawnedObjectDestroyed(DestructibleObject destroyedObject)
    {
        onSpawnedObjectDestroyed?.Invoke(destroyedObject);
    }
}
