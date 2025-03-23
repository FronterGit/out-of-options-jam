using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> openingClips = new List<AudioClip>();
    private bool gameStarted = false;
    [SerializeField] private PlayerControls playerControls;

    private void Update()
    {
        if (!gameStarted)
        {
            // listen for mouse movement
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                // start the game
                gameStarted = true;
                StartCoroutine(PlayOpeningClips());
            }
        }
    }
    
    [SerializeField] int openingClipIndex = 0;
    
    private IEnumerator PlayOpeningClips()
    {
        //play the clip
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = openingClips[openingClipIndex];
        
        //get the first clip length
        float clipLength = openingClips[openingClipIndex].length;
        
        //wait for the clip to finish
        audioSource.Play();
        yield return new WaitForSeconds(clipLength + 1);
        
        //increment the index
        openingClipIndex++;
        
        //if there are more clips, play the next one
        if (openingClipIndex < openingClips.Count)
        {
            StartCoroutine(PlayOpeningClips());
        }
        else
        {
            playerControls.enabled = true;
        }
    }
}
