using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Header("General Setup Settings")]
    [Tooltip("How fast ship moves up and down based upon player input")] [SerializeField] float playerControlSpeed = 40f;
    [Tooltip("How far the player can move in the x-range")] [SerializeField] float xRange = 21f;
    [Tooltip("How far the player can move in the y-range")] [SerializeField] float yRange = 12f;
    
    [Header("Laser gun array")]
    [Tooltip("Add all player lasers here")] [SerializeField] GameObject[] lasers;

    [Header("Screen position based tuning")]
    [SerializeField] float positionPitchFactor = 2f;
    [SerializeField] float positionYawFactor = 2f;
    
    [Header("Player input based tuning")]
    [SerializeField] float controlPitchFactor = 10f;
    [SerializeField] float controlRollFactor = 10f;

    bool controlsEnabled;
    float controlsEnabledTimer = 5f;

    float xThrow;
    float yThrow;

    bool fireButton;

    private void Start()
    {
        controlsEnabled = false;
        SetLasersActive(false);
        StartCoroutine(enableControls(controlsEnabledTimer));
    }

    void Update()
    {
        if (controlsEnabled)
        {
            ProcessTranslation();
            ProcessRotation();
            ProcessFire();
        } 
        else
        {
            return;
        }
    }
    void ProcessTranslation()
    {
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");

        float xOffset = xThrow * Time.deltaTime * playerControlSpeed;
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

        float yOffset = yThrow * Time.deltaTime * playerControlSpeed;
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }

    void ProcessRotation()
    {
        float pitch = transform.localPosition.y * -positionPitchFactor; 
        pitch += yThrow * -controlPitchFactor;

        float yaw = transform.localPosition.x * positionYawFactor;

        float roll = xThrow * -controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    IEnumerator enableControls(float timer)
    {
        yield return new WaitForSeconds(timer);
        Debug.Log("Controls enabled");
        controlsEnabled = true;
    }
    
    void ProcessFire()
    {
        fireButton = Input.GetButton("Fire1");

        if (fireButton)
        {
            SetLasersActive(true);
            Debug.Log("Gun fired");
        }
        else
        {
            SetLasersActive(false);
        }
    }

    private void SetLasersActive(bool isActive)
    {
        foreach (GameObject laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }
}
