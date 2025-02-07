using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    public bool bIsWalkable;
    public bool bIsFogged;
    public bool bRewardNode;

    public GameObject parent;
    public GameObject reward;
    public int Cost_G;
    public int Cost_H;
    

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite FoggedNodeSprite;
    [SerializeField] Sprite WalkableSprite;
    [SerializeField] Sprite UnWalkableSprite;
    [SerializeField] Sprite EndGoalSprite;
    [SerializeField] Sprite ClosedNodeSprite;
    [SerializeField] Sprite OpenNodeSprite;
 

    public int Cost_F{
        get{ return Cost_G + Cost_H;}
    }

    public void SetUnwalkable()
    {
        spriteRenderer.sprite = UnWalkableSprite;
        bIsWalkable = false;
    }

    public void SetWalkable()
    {
        spriteRenderer.sprite = WalkableSprite;
        bIsWalkable = true;
    }

    public void SetEndGoal()
    {
        spriteRenderer.sprite = EndGoalSprite;
    }

    public void ClosedNode()
    {
        if (spriteRenderer.sprite != OpenNodeSprite)
        {
            spriteRenderer.sprite = ClosedNodeSprite;
        }
    }

    public void OpenNode()
    {
        spriteRenderer.sprite = OpenNodeSprite;
    }

    public void AddFog()
    {
        spriteRenderer.sprite = FoggedNodeSprite;
        bIsFogged = true;
    }

    public void RemoveFog()
    {
        bIsFogged = false;
        if(bIsWalkable) { SetWalkable(); }
        else { SetUnwalkable(); }
    }

    public void CollectReward()
    {

        List<GameObject> RewardNodes = reward.GetComponent<Reward>().Nodes;

        foreach(GameObject node in RewardNodes)
        {
            node.GetComponent<GridNode>().bRewardNode = false;
        }
        spriteRenderer.sprite = OpenNodeSprite;
        reward.GetComponent<Renderer>().enabled = false;
    }
}
