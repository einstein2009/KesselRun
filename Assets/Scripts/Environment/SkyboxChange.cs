using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChange : MonoBehaviour
{
    public Material[] skyboxes;
    public float speedMultiplier = 5;

    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * speedMultiplier);
    }
    public void ChangeMySkybox()
    {
        int x = Random.Range(0, skyboxes.Length - 1);
        RenderSettings.skybox = skyboxes[x];
    }
}
