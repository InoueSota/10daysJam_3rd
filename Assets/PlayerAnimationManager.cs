using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAnimationManager : MonoBehaviour
{

    PlayerMoveManager player;
    Animator animator;

    [SerializeField] GameObject playerGraphic;

    Vector3 scale = Vector3.one;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMoveManager>();
        animator = GetComponent<Animator>();

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
    }
}
