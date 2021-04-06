﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public Camera mainCam;

    float shakeAmount = 0;

    private void Awake()
    {
        if (mainCam == null)
            mainCam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Shake(0.1f, 0.2f);
        }
    }

    public void Shake(float amount, float length)
    {
        shakeAmount = amount;
        InvokeRepeating("BeginShake", 0, 0.01f);
        Invoke("StopShake", length);
    }

    void BeginShake()
    {
        if(shakeAmount > 0)
        {
            Vector3 camPos = mainCam.transform.position;

            float shakeAmountX = Random.value * shakeAmount * 2 - shakeAmount;
            float shakeAmountY = Random.value * shakeAmount * 2 - shakeAmount;
            camPos.x += shakeAmountX;
            camPos.y += shakeAmountY;

            mainCam.transform.position = camPos;
             
        }
    }

    void StopShake()
    {
        CancelInvoke("BeginShake");
        mainCam.transform.localPosition = Vector3.zero;
    }

}
