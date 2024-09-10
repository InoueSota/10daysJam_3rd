using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAnimationManager : MonoBehaviour
{

    PlayerMoveManager player;
    Animator animator;
    [SerializeField] CropLineManager cropLineManager;

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



        animator.SetBool("isWalk", player.IsMoving());

        if (player.GetIsJump() == true)
        {
            animator.SetBool("isJump", true);
        }

        if (player.GetIsJump() == false && player.GetIsHovering() == false && player.GetIsGravity() == false)
        {
            animator.SetBool("isJump", false);
        }

        if(cropLineManager.isCroping == true)
        {
            animator.SetTrigger("crop");
        }


        //particle
        if (player.IsMoving() && player.GetIsJump() == false && player.GetIsHovering() == false && player.GetIsGravity() == false)
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
