using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class scoreCounter : MonoBehaviour
{   
    public Text displayedScore;
    public static int score = 0;
    public GameObject canvas;
    public SteamVR_Action_Boolean displayScoreboard;

    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        displayedScore.text = score.ToString() + " points";

        if (displayScoreboard.stateDown)
         {
             canvas.SetActive(true);
         }
      
    }

}

//Update UI - put these lines of codes in the scripts whenever we want to update the score. So for shooting down aliens

// scoreCounter.score += X; whew x is the amount of score