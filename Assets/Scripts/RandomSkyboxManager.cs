using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkyboxManager : MonoBehaviour
{

    public Material[] skyboxMats;

    private Material currentSkyboxMat;

    void Start()
    {
        currentSkyboxMat = skyboxMats[Random.Range(0, skyboxMats.Length)];
        RenderSettings.skybox = currentSkyboxMat;
    }

    void Update()
    {
    }
}
