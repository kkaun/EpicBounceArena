using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPitController : MonoBehaviour
{
    public ParticleSystem aura1;
    public ParticleSystem aura2;

    private const float rotationSpeed = 45f;
    private const float rotationAngle = 358f;

    private bool isRotating;

    void Start()
    {
        isRotating = false;

        aura1 = Instantiate(aura1, transform.position, aura1.transform.rotation);
        aura2 = Instantiate(aura2, transform.position, aura2.transform.rotation);
        aura1.Stop();
        aura2.Stop();
    }

    void LateUpdate()
    {
        if(isRotating)
        {
            aura1.gameObject.SetActive(true);
            aura2.gameObject.SetActive(true);

            aura1.Play();
            aura2.Play();

            float rotationAmount = rotationSpeed * Time.deltaTime;

            transform.Rotate(Vector3.forward, rotationAmount);

            Debug.Log("ANGLE: " + transform.rotation.eulerAngles.z);

            if (Mathf.Abs(transform.rotation.eulerAngles.z) >= rotationAngle)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                isRotating = false;
            }
        } else
        {
            aura1.Stop();
            aura2.Stop();

            aura1.gameObject.SetActive(false);
            aura2.gameObject.SetActive(false);
        }
    }

    public void OpenPit()
    {
        isRotating = true;
    }
}
