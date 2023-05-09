using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSounds : MonoBehaviour
{
    [SerializeField] AudioClip[] moveSounds;

    AudioSource audioSuorce;
    void Start()
    {
      audioSuorce = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetMouseButtonDown(0)) 
        //{
        //    Sounds();
        //}
    }

    public void Sounds() 
    {
    
        AudioClip clip = moveSounds[UnityEngine.Random.Range(0, moveSounds.Length)];
        audioSuorce.PlayOneShot(clip);
    }
}
