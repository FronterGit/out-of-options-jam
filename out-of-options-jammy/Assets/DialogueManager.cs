using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> openingClips = new List<AudioClip>();
    [SerializeField] private List<String> openingText = new List<String>();
    
    private bool gameStarted = false;
    [SerializeField] private PlayerControls playerControls;
    [SerializeField] private TMPro.TMP_Text dialogueText;
    [SerializeField] private float fadeSpeed = 1;
    private bool fadeTextIn = false;
    private bool fadeTextOut = false;
    
    [SerializeField] private AudioClip squareCorrectClip;
    [SerializeField] private string squareCorrectText;
    
    [SerializeField] private AudioClip firstHoleUsedClip;
    [SerializeField] private string firstHoleUsedText;
    
    [SerializeField] private List<AudioClip> squareClips = new List<AudioClip>();
    [SerializeField] private List<String> squareText = new List<String>();
    private int squareClipIndex = 0;

    private void OnEnable()
    {
        SquareHole.OnSquarePlacedRight += OnSquareUsedCorrect;
        SquareHole.OnSquarePlacedWrong += OnSquareUsedWrong;
        
        Hole.firstUsedEvent += OnFirstHoleUsed;
    }
    
    private void OnDisable()
    {
        SquareHole.OnSquarePlacedRight -= OnSquareUsedCorrect;
        SquareHole.OnSquarePlacedWrong -= OnSquareUsedWrong;
        
        Hole.firstUsedEvent -= OnFirstHoleUsed;
    }

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

        if (fadeTextIn)
        {
            dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, dialogueText.color.a + Time.deltaTime * fadeSpeed);
            if (dialogueText.color.a >= 1)
            {
                fadeTextIn = false;
            }
        }
        
        if (fadeTextOut)
        {
            dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, dialogueText.color.a - Time.deltaTime * fadeSpeed);
            if (dialogueText.color.a <= 0)
            {
                fadeTextOut = false;
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
        
        //set the text
        dialogueText.text = openingText[openingClipIndex];
        
        //fade the text in
        fadeTextIn = true;
        
        yield return new WaitForSeconds(clipLength);
        
        fadeTextOut = true;
        
        yield return new WaitForSeconds(1);
        
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
    
    public void OnSquareUsedCorrect()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = squareCorrectClip;
        audioSource.Play();
        dialogueText.text = squareCorrectText;
        fadeTextIn = true;
    }
    
    public void OnSquareUsedWrong()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = squareClips[squareClipIndex];
        audioSource.Play();
        dialogueText.text = squareText[squareClipIndex];
        fadeTextIn = true;
        
        StartCoroutine(WaitForClipToEnd(squareClips[squareClipIndex].length));
        squareClipIndex++;
    }
    
    public void OnFirstHoleUsed()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = firstHoleUsedClip;
        audioSource.Play();
        dialogueText.text = firstHoleUsedText;
        fadeTextIn = true;
        StartCoroutine(WaitForClipToEnd(firstHoleUsedClip.length));
    }

    private IEnumerator WaitForClipToEnd(float wait)
    {
        yield return new WaitForSeconds(wait);
        fadeTextOut = true;
    }
}
