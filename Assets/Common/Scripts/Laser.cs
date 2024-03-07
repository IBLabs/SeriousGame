using Common.Scripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer beamLineRenderer;
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private float maxDistance = 100f;

    [SerializeField] private Camera targetCamera;
    [SerializeField] private ParticleSystem muzzleParticleSystem;
    [SerializeField] private ParticleSystem hitParticleSystem;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource explosionAudioSource;

    [SerializeField] private float damage;
    [SerializeField] private AudioClip smallExplosionClip;

    private bool _isLaserEnabled;

    private void Awake()
    {
        DeactivateLaser();
    }

    public void ActivateLaser()
    {
        SetLaserSoundActive(true);
        _isLaserEnabled = true;
    }

    public void DeactivateLaser()
    {
        SetLaserSoundActive(false);
        _isLaserEnabled = false;
    }

    private void SetLaserSoundActive(bool isActive)
    {
        if (isActive && !audioSource.isPlaying)
        {
            audioSource.Play();
            audioSource.DOFade(0.5f, .2f);
        }
        else if (!isActive && audioSource.isPlaying)
        {
            audioSource.DOFade(0f, .2f).onComplete += () => audioSource.Stop();
        }
    }

    private void Update()
    {
        // if (Input.GetMouseButtonDown(0)) ActivateLaser();
        // else if (Input.GetMouseButtonUp(0)) DeactivateLaser();
    }

    public void OnShootLaser(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started) ActivateLaser();
        else if (context.phase == InputActionPhase.Canceled) DeactivateLaser();
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
                bool didDie = damageable.ApplyDamage(damage * Time.fixedDeltaTime, gameObject);
                if (didDie)
                {
                    Instantiate(explosionPrefab, damageable.transform.position, Quaternion.identity);
                    explosionAudioSource.pitch = Random.Range(0.9f, 1.1f);
                    explosionAudioSource.PlayOneShot(smallExplosionClip);
                    
                    targetCamera.DOShakePosition(.1f, .1f);
                }
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
