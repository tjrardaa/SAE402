using UnityEngine;

public class Particle : MonoBehaviour
{
    public ParticleSystem ps;

    private void Awake()
    {
        if (ps == null)
        {
            ps = GetComponent<ParticleSystem>();
        }
        
        if (ps != null)
        {
            ps.Stop();
        }
    }

    private void OnBecameVisible()
    {
        if (ps != null)
        {
            ps.Play();
        }
    }

    private void OnBecameInvisible()
    {
        if (ps != null)
        {
            ps.Stop();
        }
    }
}
