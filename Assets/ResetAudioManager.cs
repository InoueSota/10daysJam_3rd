using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAudioManager : MonoBehaviour
{

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] clip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound()
    {
        int num = Random.Range(0, 100);
        int isRare = Random.Range(0, 100);

        if (isRare < 5)
        {
            if (num < 50)
            {
                audioSource.clip = clip[0];
            }
            else
            {
                audioSource.clip = clip[1];
            }

        }
        else
        {
            if (num < 25)
            {
                audioSource.clip = clip[2];
            }
            else if (num < 50)
            {
                audioSource.clip = clip[3];
            }
            else if (num < 70)
            {
                audioSource.clip = clip[4];
            }
            else
            {
                audioSource.clip = clip[5];
            }
        }

        audioSource.Play();
    }
}
