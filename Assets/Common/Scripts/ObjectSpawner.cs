using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;

    [SerializeField] private bool randomRotation;

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
        Instantiate(objectPrefab, transform.position, rotation);
    }
}
