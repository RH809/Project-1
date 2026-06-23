/// <summary>
/// This script destroys the explosion gameobject when the particle system finishes playing
/// </summary>
using UnityEngine;

public class ExplosionDestroy : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        Destroy(transform.parent.gameObject);
    }
}
