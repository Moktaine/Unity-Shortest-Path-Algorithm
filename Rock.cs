using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Obstacle_Static
{
    float ObjectScale;

    [SerializeField] Sprite BigRock_winterSprite;
    [SerializeField] Sprite BigRock_summerSprite;
    [SerializeField] Sprite SmallRock_winterSprite;
    [SerializeField] Sprite SmallRock_summerSprite;

    void Awake()
    {
        //Determine the scale
        int randomNumber = Random.Range(2, 4);
        ObjectScale = (float)randomNumber / 2;
        this.transform.localScale = new Vector3(ObjectScale, ObjectScale, 0);
  
        //Change the sprite based on scale of the rock
        if(ObjectScale == 1)
        {
            base.spriteRenderer.sprite = SmallRock_summerSprite;
            if (this.transform.position.x < base.width) { base.spriteRenderer.sprite = SmallRock_winterSprite; }
        }
        else
        {
            base.spriteRenderer.sprite = BigRock_summerSprite;
            if (this.transform.position.x < base.width) { base.spriteRenderer.sprite = BigRock_winterSprite; }
        }
    }


}
