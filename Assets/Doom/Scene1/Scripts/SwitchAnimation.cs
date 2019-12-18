using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAnimation : MonoBehaviour
{
   
    private Animator animator;

    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        animator.SetBool("isWalking", false);
        animator.SetBool("isFloating", true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isFloating", false);
        }else if (Input.GetKeyDown(KeyCode.F)){
            animator.SetBool("isWalking", false);
            animator.SetBool("isFloating", true);
        }
    }
}
