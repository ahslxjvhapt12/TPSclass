using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraAction : MonoBehaviour
{
    public static CameraAction Instance { get; private set; }
    private CinemachineVirtualCamera virtualCam;
    private CinemachineBasicMultiChannelPerlin m_MultiChannelPerlin;

    private float totalTime = 0;
    private float currentTime;
    private float startIntensity;

    private void Awake()
    {
        Instance = this;
        virtualCam = GetComponent<CinemachineVirtualCamera>();
        m_MultiChannelPerlin = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCam(float intensity, float time)
    {
        m_MultiChannelPerlin.m_AmplitudeGain = intensity;
        totalTime = currentTime = time;
        startIntensity = intensity;
    }

    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0) currentTime = 0;
            m_MultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startIntensity, 0f, 1 - currentTime / totalTime);
        }
    }
}
