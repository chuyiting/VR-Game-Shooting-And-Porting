using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreCounter : MonoBehaviour
{   
    public Text displayedScore;
    public static int score = 0;
    //public GameObject camera2;
    //private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
       //offset = transform.position - camera2.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        displayedScore.text = score.ToString() + " points";
    }

   // void LateUpdate() {
        //transform.position = camera2.transform.position + offset;
   // }
}

//Update UI - put these lines of codes in the scripts whenever we want to update the score. So for shooting down aliens

// scoreCounter.score += X; whew x is the amount of score