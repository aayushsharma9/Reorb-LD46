using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject explosionParticles;
    public int level;

    void Start()
    {
        AudioManager.instance.Play("CannonShoot");
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Hazard")
        {
            other.gameObject.GetComponent<HazardBehaviour>().HurtHazard(level);
        }
        Instantiate(explosionParticles, other.GetContact(0).point, Quaternion.identity);
        Destroy(gameObject);
    }

    public void KillBullet()
    {
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}