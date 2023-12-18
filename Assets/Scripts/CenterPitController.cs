using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPitController : MonoBehaviour
{
    public ParticleSystem aura1;
    public ParticleSystem aura2;

    private const float rotationSpeed = 45f;
    private const float rotationAngle = 90f;
    private const float rotateAndStayOnTopTimeSec = 4f;

    private bool isRotating;

    void Start()
    {
        isRotating = false;

        aura1 = Instantiate(aura1, transform.position, aura1.transform.rotation);
        aura2 = Instantiate(aura2, transform.position, aura2.transform.rotation);
        aura1.Stop();
        aura2.Stop();
    }

    void Update()
    {
        if(isRotating)
        {
            aura1.Play();
            aura2.Play();

            float rotationAmount = rotationSpeed * Time.deltaTime;

            transform.Rotate(Vector3.right, rotationAmount);

            if (Mathf.Abs(transform.rotation.eulerAngles.x) >= rotationAngle)
            {
                isRotating = false;
            }
        } else
        {
            aura1.Stop();
            aura2.Stop();
        }
    }

    public void OpenPit()
    {
        isRotating = true;
        StartCoroutine(ResetPitTask());
    }

    IEnumerator ResetPitTask()
    {
        yield return new WaitForSeconds(rotateAndStayOnTopTimeSec);
        OpenPit();
    }
}
