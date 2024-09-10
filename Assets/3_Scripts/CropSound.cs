using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropSound : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource audioSource2;
    [SerializeField] AudioClip soundCrop; 
    [SerializeField] AudioClip soundCropMiss; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SoundCrop()
    {
        audioSource.PlayOneShot(soundCropMiss);
        //audioSource2.PlayOneShot(soundCrop);
    }
    public void SoundCropMiss()
    {
        //audioSource.PlayOneShot(soundCrop);
        audioSource.PlayOneShot(soundCropMiss);
    }

}
