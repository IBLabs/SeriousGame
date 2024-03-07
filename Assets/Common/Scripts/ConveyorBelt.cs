using System;
using System.Collections;
using System.Collections.Generic;
using Common.Scripts.RevisedLevelsSystem;
using DG.Tweening;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public GameObject partPrefab;
    public int amount = 10;

    public float conveyorBeltSpeed = 1;

    [SerializeField] private float loopDelta = 50f;
    [SerializeField] private bool flipDirection;
    [SerializeField] private float levelSpeedMutliplier = 3f;

    private GameObject[] parts;
    private Vector3 duplicationAxis = Vector3.left;

    private List<Rigidbody> touchingRigidbodies = new();
    
    private Vector3 _velocity => new(conveyorBeltSpeed, 0f, 0f);

    void Start()
    {
        CreateConveyorBelt();
    }

    void FixedUpdate()
    {
        MoveConveyorBelt();
        MoveTouchingRigidbodies();
    }

    void CreateConveyorBelt()
    {
        parts = new GameObject[amount];
        Vector3 startPosition = transform.position;
        
        for (int i = 0; i < amount; i++)
        {
            GameObject part = Instantiate(partPrefab, startPosition + duplicationAxis * i, Quaternion.identity,
                transform);

            if (part.TryGetComponent<MeshFilter>(out var partMeshFilter))
            {
                float meshWidth = partMeshFilter.mesh.bounds.size.x;
                Vector3 startPos = startPosition + duplicationAxis * i * meshWidth * (flipDirection ? -1 : 1);
                part.transform.position = startPos;
            }

            parts[i] = part;
        }
    }

    void MoveConveyorBelt()
    {
        foreach (GameObject part in parts)
        {
            if (!part.TryGetComponent(out Rigidbody partRb)) return;
            
            partRb.MovePosition(partRb.position + _velocity * (flipDirection ? -1 : 1) * Time.fixedDeltaTime);
            
            bool shouldReposition = flipDirection ? partRb.position.x < -loopDelta : partRb.position.x > loopDelta;
            
            if (shouldReposition)
            {
                MeshFilter partMeshFilter = part.GetComponent<MeshFilter>();
                float partWidth = partMeshFilter.mesh.bounds.size.x;

                if (flipDirection)
                {
                    Vector3 newPos = new Vector3(FindHighestX(parts) + partWidth, transform.position.y, transform.position.z);
                    partRb.MovePosition(newPos);
                }
                else
                {
                    Vector3 newPos = new Vector3(FindLowestX(parts) - partWidth, transform.position.y, transform.position.z);
                    partRb.MovePosition(newPos);
                }
            }
        }
    }
    
    private void MoveTouchingRigidbodies()
    {
        foreach (Rigidbody touchingRigidbody in touchingRigidbodies)
        {
            touchingRigidbody.AddForce(_velocity * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
    
    public float FindLowestX(GameObject[] gameObjects)
    {
        if (gameObjects == null || gameObjects.Length == 0)
        {
            Debug.LogError("GameObject array is empty or null.");
            return float.NaN; // Return NaN to indicate error
        }

        float lowestX = float.MaxValue; // Initialize with the maximum possible value

        foreach (GameObject obj in gameObjects)
        {
            if (obj.transform.position.x < lowestX)
            {
                lowestX = obj.transform.position.x; // Update lowestX with the new lowest value
            }
        }

        return lowestX;
    }
    
    public float FindHighestX(GameObject[] gameObjects)
    {
        if (gameObjects == null || gameObjects.Length == 0)
        {
            Debug.LogError("GameObject array is empty or null.");
            return float.NaN; // Return NaN to indicate error
        }

        float highestX = float.MinValue; // Initialize with the maximum possible value

        foreach (GameObject obj in gameObjects)
        {
            if (obj.transform.position.x > highestX)
            {
                highestX = obj.transform.position.x; // Update lowestX with the new lowest value
            }
        }

        return highestX;
    }

    private void OnCollisionEnter(Collision other)
    {
        touchingRigidbodies.Add(other.rigidbody);
    }

    private void OnCollisionExit(Collision other)
    {
        touchingRigidbodies.Remove(other.rigidbody);
    }

    public void OnLevelStart(LevelDescriptor levelDescriptor)
    {
        DOTween.To(() => conveyorBeltSpeed, (x) => conveyorBeltSpeed = x, levelDescriptor.spawnRate * levelSpeedMutliplier, .5f);
    }
}