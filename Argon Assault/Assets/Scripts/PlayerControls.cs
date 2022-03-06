using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] float playerControlSpeed = 40f;
    [SerializeField] float xRange = 21f;
    [SerializeField] float yRange = 12f;

    [SerializeField] float positionPitchFactor = 2f;
    [SerializeField] float positionYawFactor = 2f;
    [SerializeField] float controlPitchFactor = 10f;
    [SerializeField] float controlRollFactor = 10f;

    float xThrow;
    float yThrow;
  
    void Update()
    {
        processTranslation();
        processRotation();
    }

    void processRotation()
    {
        float pitch = transform.localPosition.y * -positionPitchFactor; 
        pitch += yThrow * -controlPitchFactor;

        float yaw = transform.localPosition.x * positionYawFactor;

        float roll = xThrow * -controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void processTranslation()
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
}
