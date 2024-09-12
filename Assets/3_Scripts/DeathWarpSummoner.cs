using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWarpSummoner : MonoBehaviour
{

    PlayerAnimationManager playerAnimation;
    [SerializeField] GameObject deathWarp;
    GameObject deathWarp_;
    [SerializeField] Transform parent;

    // Start is called before the first frame update

    void Start()
    {
        playerAnimation = GetComponent<PlayerAnimationManager>();
    }

    void Update()
    {

        if(playerAnimation.GetIsDeathWarp() == true) {
            SpawnDeathWarp();
        }
        else
        {
            DestroyDeathWarp();
        }

    }


    public void SpawnDeathWarp()
    {
        if (deathWarp_ == null)
        {
            deathWarp_ = Instantiate(deathWarp, this.transform.position + Vector3.up * 0.93f, Quaternion.identity);
            deathWarp_.transform.parent = parent.transform;
        }
    }

    public void DestroyDeathWarp()
    {
        if (deathWarp_ != null)
        {
            Destroy(deathWarp_);
        }
    }

}
