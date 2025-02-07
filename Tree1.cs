using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree1 : Obstacle_Static
{
    float ObjectScale;

    void Awake()
    {
        //Determine the scale of the tree and adjust position accordingly
        int randomNumber = Random.Range(2, 6);
        ObjectScale = (float)randomNumber / 2;
        this.transform.localScale *= ObjectScale;
    }

}
