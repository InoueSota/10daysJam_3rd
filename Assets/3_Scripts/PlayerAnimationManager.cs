using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAnimationManager : MonoBehaviour
{

    PlayerMoveManager player;
    Animator animator;
    [SerializeField] CropLineManager cropLineManager;
    GameManager gameManager;

    [SerializeField] GameObject playerGraphic;

    Vector3 scale = Vector3.one;

    ParticleInstantiateScript particle;
    [SerializeField] float walkParticleTimeMax = 0.5f;
    float walkParticleTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMoveManager>();
        animator = GetComponent<Animator>();
        particle = GetComponent<ParticleInstantiateScript>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        int direction = player.GetDirection();

        if (direction != 0)
        {
            scale.x = direction;
            playerGraphic.transform.localScale = scale;
        }

        bool isWalk= player.IsMoving();
        bool isJump = player.GetIsJump();
        bool isHovering = player.GetIsHovering();
        bool isGravity = player.GetIsGravity();
        bool isCropping = cropLineManager.GetIsCropping();
        bool isCactus = player.GetIsCactus();
        bool isClear = gameManager.GetIsClear();

        animator.SetBool("isWalk", isWalk);

        if (isJump == true)
        {
            animator.SetBool("isJump", true);
        }

        if (isJump == false )
        {
            animator.SetBool("isJump", false);
        }

        if (isHovering == true || isGravity == true)
        {
            animator.SetBool("isFall", true);
        }
        else
        {
            animator.SetBool("isFall", false);
        }

        if(isCropping == true)
        {
            animator.SetTrigger("crop");
        }

        if(isCactus == true)
        {
            animator.SetBool("isDamage", true);
        }
        else
        {
            animator.SetBool("isDamage", false);
        }

        animator.SetBool("isClear", isClear);



        //particle
        if (isWalk == true && isJump == false && isHovering == false && isGravity == false)
        {
            if (walkParticleTime < 0)
            {
                particle.RunParticle(0, this.transform.position + Vector3.down * 0.5f);
                walkParticleTime = walkParticleTimeMax;
            }
            else
            {
                walkParticleTime -= Time.deltaTime;
            }
        }
        else
        {
            walkParticleTime = 0;
        }

    }
}
