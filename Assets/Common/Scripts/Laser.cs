using Common.Scripts;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer beamLineRenderer;
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private float maxDistance = 100f;
    
    [SerializeField] private ParticleSystem muzzleParticleSystem;
    [SerializeField] private ParticleSystem hitParticleSystem;

    [SerializeField] private float damage;

    private bool _isLaserEnabled;

    private void Awake()
    {
        DeactivateLaser();
    }

    public void ActivateLaser()
    {
        _isLaserEnabled = true;
    }

    public void DeactivateLaser()
    {
        _isLaserEnabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) ActivateLaser();
        else if (Input.GetMouseButtonUp(0)) DeactivateLaser();
    }

    private void FixedUpdate()
    {
        if (!_isLaserEnabled)
        {
            beamLineRenderer.enabled = false;
            muzzleParticleSystem.Stop();
            hitParticleSystem.Stop();

            return;
        }

        Ray ray = new Ray(muzzlePoint.position, muzzlePoint.up);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            Vector3 hitPosition = hit.point;
        
            beamLineRenderer.SetPosition(0, muzzlePoint.position);
            beamLineRenderer.SetPosition(1, hitPosition);
            
            beamLineRenderer.enabled = true;

            hitParticleSystem.transform.position = hitPosition;
            
            if (!muzzleParticleSystem.isPlaying) muzzleParticleSystem.Play();
            if (!hitParticleSystem.isPlaying) hitParticleSystem.Play();

            if (hit.collider.TryGetComponent(out Damageable damageable))
            {
                damageable.ApplyDamage(damage * Time.fixedDeltaTime);
            }
        }
        else
        {
            beamLineRenderer.enabled = false;
            
            muzzleParticleSystem.Stop();
            hitParticleSystem.Stop();
        }
        
    }
}
