using UnityEngine;

public class HazardBehaviour : MonoBehaviour
{
    public int strength;
    [SerializeField] private float speed, maxSpeed;
    [SerializeField] private GameObject explosionParticles, coreDeathParticles, equipmentDeathParticles;
    private Rigidbody2D hazardRigidbody;

    void Start()
    {
        hazardRigidbody = gameObject.GetComponent<Rigidbody2D>();
        Vector3 dir = GameObject.Find("Base-Core").transform.position - transform.position;
        dir.Normalize();
        hazardRigidbody.velocity = dir * Mathf.Min((speed + (GameManager.instance.levelCount * 1.0f)), maxSpeed);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Camera.main.GetComponent<CameraShake>().enabled = true;

        switch (other.gameObject.tag)
        {
            case "Core":
                Destroy(other.gameObject);
                Instantiate(coreDeathParticles, transform.position, Quaternion.identity);
                KillHazardWithoutPoints();
                GUIManager.instance.GameOver();
                break;

            case "Equipment-Converter":
                GameManager.instance.IncreaseCoins(200);
                KillHazardWithoutPoints();
                break;

            case "Equipment-Barrier":
                if (strength > other.gameObject.GetComponent<Equipment>().level)
                {
                    other.transform.parent.GetComponent<BaseSlotBehaviour>().isEquipped = false;
                    Destroy(other.gameObject);
                }
                KillHazardWithoutPoints();
                break;

            case "Equipment-Seeker":
            case "Equipment-Cannon":
                other.transform.parent.GetComponent<BaseSlotBehaviour>().isEquipped = false;
                Camera.main.GetComponent<CameraShake>().enabled = true;
                Instantiate(equipmentDeathParticles, other.GetContact(0).point, Quaternion.identity);
                Destroy(other.gameObject);
                KillHazardWithoutPoints();
                break;
        }
    }

    void KillHazardWithoutPoints()
    {
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        AudioManager.instance.Play("Explosion");
        Destroy(gameObject);
    }

    public void KillHazard()
    {
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        GameManager.instance.IncreaseCoins(100);
        AudioManager.instance.Play("Explosion");
        Destroy(gameObject);
    }

    public void HurtHazard(int value)
    {
        strength -= value;
        if (strength <= 0)
        {
            KillHazard();
        }
    }
}