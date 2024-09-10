using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    private float coolTime;
    // Start is called before the first frame update
    void Start()
    {
        coolTime = 3.0f;
        //audioSource.PlayOneShot(audioClip);
        AudioSource.PlayClipAtPoint(audioClip,Vector3.zero);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
