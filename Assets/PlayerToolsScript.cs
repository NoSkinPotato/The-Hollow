using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerToolsScript : MonoBehaviour
{

    [SerializeField] private Light2D cone;
    [SerializeField] private Light2D area;
    [SerializeField] private float maximumFallOff;
    [SerializeField] private float extendTime;


    private float timer = 1f;


    private void Start()
    {
        cone.enabled = true;
        area.falloffIntensity = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (cone.isActiveAndEnabled)
            {
                cone.enabled = false;
                
            }
            else
            {
                cone.enabled = true;
                area.falloffIntensity = 1f;
            }
            timer = 1f;
        }


        if (cone.isActiveAndEnabled == false)
        {
            
            if(timer > maximumFallOff)
            {
                timer -= Time.deltaTime / extendTime;

            }

            area.falloffIntensity = timer;
        }



    }


}
