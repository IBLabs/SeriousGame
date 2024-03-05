using UnityEngine;

namespace Common.Scripts
{
    public class SimpleSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;

        public void SpawnObject()
        {
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}