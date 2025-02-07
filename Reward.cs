using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    float ObjectScaleX, ObjectScaleY;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    public List<GameObject> Nodes;

    int height, width;

    public void SetHeightWidth(int h)
    {
        width = h;
        height = h;
        Check();
    }

    protected void Check()
    {
        ObjectScaleX = this.transform.localScale.x;
        ObjectScaleY = this.transform.localScale.y;

        //Check if the object is in bounds


        Vector3 cornerPosition;

        //Top Left Corner
        cornerPosition = new Vector3(spriteRenderer.bounds.min.x, spriteRenderer.bounds.max.y, 0);
        if (cornerPosition.x < 0 || cornerPosition.y > height)
        {
            this.transform.position -= new Vector3(cornerPosition.x, cornerPosition.y - height, 0);
        }

        //Top Right Corner
        cornerPosition = spriteRenderer.bounds.max;
        if (cornerPosition.x > width || cornerPosition.y > height)
        {
            this.transform.position -= new Vector3(cornerPosition.x - width, cornerPosition.y - height, 0);
        }

        //Bottom Left Corner
        cornerPosition = spriteRenderer.bounds.min;
        if (cornerPosition.x < 0 || cornerPosition.y < 0)
        {
            this.transform.position -= new Vector3(cornerPosition.x, cornerPosition.y, 0);
        }

        //Bottom Right Corner
        cornerPosition = new Vector3(spriteRenderer.bounds.max.x, spriteRenderer.bounds.min.y, 0);
        if (cornerPosition.x > width || cornerPosition.y < 0)
        {
            this.transform.position -= new Vector3(cornerPosition.x - width, cornerPosition.y, 0);
        }

    }




}
