using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLaunch : MonoBehaviour
{
    public ParticleSystem bulletAura;
    private Transform target;
    private float speed = 15.0f;
    private bool homing;
    private float rocketStrength = 15.0f;
    private float aliveTimer = 5.0f;

    void Start()
    {
        bulletAura = Instantiate(bulletAura, transform.position, transform.rotation);
    }

    void Update()
    {
        if (homing && target != null)
        {
            Vector3 moveDirection = (target.transform.position - transform.position).normalized;

            transform.position += moveDirection * speed * Time.deltaTime;

            bulletAura.transform.position += moveDirection * speed * Time.deltaTime;
            bulletAura.Play();

            transform.LookAt(target);
        }
    }

    public void Fire(Transform newTarget)
    {
        target = newTarget;
        homing = true;
        
        Destroy(gameObject, aliveTimer);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (target != null && collision.gameObject.CompareTag(target.tag))
        {
            SimulateBullet(collision);
        }
    }

    void SimulateBullet(Collision collision)
    {
        Rigidbody targetRigidbody = collision.gameObject.GetComponent<Rigidbody>();
        Vector3 away = -collision.contacts[0].normal; //should refactor this, as may produce memory garbage
        targetRigidbody.AddForce(away * rocketStrength, ForceMode.Impulse);
        Destroy(gameObject);

        bulletAura.Stop();
        Destroy(bulletAura);
    }
}
