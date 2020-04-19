using System.Collections;
using UnityEngine;

public class SeekerBulletBehaviour : MonoBehaviour
{
    [SerializeField] private float safeDistance;
    public int level;
    private GameObject target;
    private Vector3 dir;

    void Start()
    {
        StartCoroutine(Chase());
        AudioManager.instance.Play("SeekerShoot");
        safeDistance = (transform.position - GameObject.Find("Base-Core").transform.position).magnitude + 2;
    }

    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Hazard")
        {
            if (other.gameObject.GetComponent<HazardBehaviour>().strength <= level)
            {
                Destroy(other.gameObject);
                GameManager.instance.IncreaseCoins(100);
            }
        }

        Destroy(gameObject);
    }

    IEnumerator Chase()
    {
        GameObject[] hazards = GameObject.FindGameObjectsWithTag("Hazard");
        if (hazards.Length <= 0)
        {
            Destroy(gameObject);
            yield break;
        }
        target = hazards[0];
        foreach (GameObject obj in hazards)
        {
            if ((transform.position - obj.transform.position).magnitude < (transform.position - target.transform.position).magnitude)
                target = obj;
        }
        GameObject core = GameObject.Find("Base-Core");
        dir = (target.transform.position - transform.position);
        while (dir.magnitude > 0)
        {
            if ((core.transform.position - transform.position).magnitude < safeDistance)
                gameObject.GetComponent<Rigidbody2D>().AddForce((transform.position - core.transform.position).normalized * 600);

            gameObject.GetComponent<Rigidbody2D>().AddForce(dir.normalized * 600);

            try
            {
                dir = (target.transform.position - transform.position);
            }
            catch
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        gameObject.GetComponent<BulletBehaviour>().KillBullet();
    }
}