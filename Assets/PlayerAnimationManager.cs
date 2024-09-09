using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{

    PlayerMoveManager player;
    Animator animator;

    [SerializeField] GameObject[] playerSprites;

    int direction = 1;
    int preDirection;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMoveManager>();
        animator = GetComponent<Animator>();
        direction = player.GetDirection();

    }

    // Update is called once per frame
    void Update()
    {
        int direction_ = player.GetDirection();

        if (direction_ != 0 )
        {
            preDirection = direction;
            direction = direction_;

            if (direction != preDirection)
            {
                for (int i = 0; i < playerSprites.Length; i++)
                {
                    playerSprites[i].transform.localScale *= -1;
                    playerSprites[i].transform.localPosition *= -1;
                }
            }
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
    }
}
