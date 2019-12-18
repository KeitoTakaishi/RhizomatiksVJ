using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAnimation : MonoBehaviour
{

    private Animator animator;
    [SerializeField] DoomScene.SceneManager sceneManager;

    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        animator.SetBool("isWalking", true);
        animator.SetBool("isFloating", false);
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.N))
        if(sceneManager.curSectionID == 4)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isFloating", false);
        } else if(sceneManager.curSectionID == 2)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isFloating", true);
        }
    }
}