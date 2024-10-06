using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public static CameraControl Instance { get; private set; }

    private Camera cam;

    private bool startShake = false;
    private PlayerWeaponScript playerScript;

    private float shakeDuration = 0.5f; // Total time for shake
    private float distance = 1f;        // How far the camera moves
    private float strength = 5f;        // Speed of the shake
    private Vector3 originalPosition;
    private float shakeTimeRemaining;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        cam = Camera.main;
    }


    void Start()
    {
        playerScript = PlayerWeaponScript.Instance;
    }

    private void Update()
    {
        if (startShake == false)
        {
            cam.transform.position = (Vector2)playerScript.transform.position;
        }
        
    }

    public void Shake(Vector2 direction, float shakeDuration, float distance, float strength)
    {
        startShake = true;
        shakeTimeRemaining = shakeDuration;
        this.shakeDuration = shakeDuration;
        this.distance = distance;
        this.strength = strength;
        originalPosition = (Vector2)playerScript.transform.position;
        StartCoroutine(ShakeRoutine(direction));
    }

    private IEnumerator ShakeRoutine(Vector2 direction)
    {
        Vector2 normalizedDirection = direction.normalized; // Ensure direction is normalized
        while (shakeTimeRemaining > 0)
        {
            float shakeAmount = Mathf.Sin(Time.time * strength) * distance * (shakeTimeRemaining / shakeDuration);
            Vector2 shakeOffset = new Vector3(normalizedDirection.x * shakeAmount, normalizedDirection.y * shakeAmount);
            transform.localPosition = (Vector2)playerScript.transform.position + shakeOffset;

            shakeTimeRemaining -= Time.deltaTime;
            yield return null;
        }

        // Reset to original position after shaking
        cam.transform.position = (Vector2)playerScript.transform.position;
        startShake = false;
    }


}
