using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] 
    protected float positionX, positionY;

    float ObjectScaleX, ObjectScaleY;
    [SerializeField] bool IsStatic;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] Sprite winterSprite;


    protected int height, width;


    public void SetHeightWidth(int h)
    {
        width = h;
        height = h;
        Check();
    }

    /*protected void Awake()
    {

    }*/

    protected void Check()
    {
        ObjectScaleX = this.transform.localScale.x;
        ObjectScaleY = this.transform.localScale.y;

        if (ObjectScaleX % 1 == 0.5)
        {
            this.transform.position += new Vector3(0.5f, 0, 0);
        }

        if (ObjectScaleY % 1 == 0.5)
        {
            this.transform.position += new Vector3(0, 0.5f, 0);
        }

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

        if (this.transform.position.x < width/2 && IsStatic) { spriteRenderer.sprite = winterSprite; }


        


    }


    public void SetGridNodesUnwalkable(List<GameObject> GridNodes)
    {
        //Set grid nodes to unwalkable
        Vector3 cornerPosition_TopLeft = new Vector3(spriteRenderer.bounds.min.x, spriteRenderer.bounds.max.y, 0);
        Vector3 cornerPosition_TopRight = spriteRenderer.bounds.max;
        Vector3 cornerPosition_BottomLeft = spriteRenderer.bounds.min;
        Vector3 cornerPosition_BottomRight = new Vector3(spriteRenderer.bounds.max.x, spriteRenderer.bounds.min.y, 0);
        
        for(int i = (int)cornerPosition_TopLeft.x; i < (int)cornerPosition_TopRight.x; i++)
        {
            for (int j = (int)cornerPosition_BottomLeft.y; j < (int)cornerPosition_TopLeft.y; j++)
            {
                GameObject selectedNode = GridNodes[(i * height) + (int)(j + 0.5f)];
                if (selectedNode != null)
                {
                    GridNode nodeScript = selectedNode.GetComponent<GridNode>();
                    nodeScript.SetUnwalkable();
                }
            }
        }
  
    }

}
