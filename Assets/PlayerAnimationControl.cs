using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour
{
    public static PlayerAnimationControl Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public Animator playerAnimator;

    public void PlayAnimation(string name, int index)
    {
        if (playerAnimator.GetBool("StopAnimation") == false)
        {
            playerAnimator.SetInteger(name, index);
        }
    }

    public void StopAnimation()
    {
        playerAnimator.SetBool("StopAnimation", true);
    }

    public void EndAnimation()
    {
        playerAnimator.SetBool("StopAnimation", false);
    }

    public bool GetStopAnimation()
    {
        return playerAnimator.GetBool("StopAnimation");
    }

}
