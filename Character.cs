using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] ProceduralGeneration Generator;
    [SerializeField] LineRenderer lineRenderer;
    List<GameObject> GridNodeList;
    List<GameObject> VisibleNodes;
    List<GameObject> RewardsToGo;
    int width;
    int linePoints;
    GameObject StartNode;
    [SerializeField] bool CanSeeRewards;
    [SerializeField] Container container;
    int count;

    public void MoveCharacter(GameObject StartNode)
    {
        this.transform.position = StartNode.transform.position;
        StartNode.GetComponent<GridNode>().ClosedNode();
    }


    void MoveOnPath(List<GameObject> path)
    {
        
        foreach(GameObject node in path)
        {
            linePoints++;
            lineRenderer.positionCount = linePoints;
            lineRenderer.SetPosition(linePoints - 1, this.transform.position);
            this.transform.position = node.transform.position;
            count++;
            RemoveFogFromArea();
            if (node.GetComponent<GridNode>().bRewardNode) {
                container.AddToDictionary(node.GetComponent<GridNode>().reward);
                node.GetComponent<GridNode>().CollectReward(); 
            }
            StartCoroutine(WaitForTime(1));
        }
        
    }

    private IEnumerator WaitForTime(int second)
    {
        yield return new WaitForSeconds(second);
    }


    public bool FindPath(GameObject EndGoalNode)
    {
        StartNode = GetNodeInPosition(this.transform.position.x, this.transform.position.y);

        List<GameObject> List_Open = new List<GameObject>();
        List<GameObject> List_Closed = new List<GameObject>();

        List_Open.Add(StartNode);

        while(List_Open.Count > 0)
        {
            GameObject node = List_Open[0];

            for(int i = 1; i < List_Open.Count; i++)
            {
                GridNode nodeScript = node.GetComponent<GridNode>();
                GridNode nodeScript_List = List_Open[i].GetComponent<GridNode>();
                if (nodeScript_List.Cost_F <= nodeScript.Cost_F)
                {
                    if(nodeScript_List.Cost_H < nodeScript.Cost_H)
                    {
                        node = List_Open[i];
                    }
                }
            }

            List_Open.Remove(node);
            List_Closed.Add(node);

            if(node == EndGoalNode)
            {
                //Start Moving
                MoveOnPath(ReTracePath(StartNode, EndGoalNode));
                return true;
            }

            foreach(GameObject neighbour in GetNeighboursOf(node))
            {
                GridNode nodeScript = node.GetComponent<GridNode>();
                GridNode neighbourScript = neighbour.GetComponent<GridNode>();

                if(!neighbourScript.bIsWalkable || List_Closed.Contains(neighbour) ) { continue; }

                if(!CanSeeRewards && neighbourScript.bIsFogged) { continue; }

                int newCostToNeighbour = nodeScript.Cost_G + GetDistanceBetweenNodes(node, neighbour);

                if(newCostToNeighbour < neighbourScript.Cost_G || !List_Open.Contains(neighbour))
                {
                    neighbourScript.Cost_G = newCostToNeighbour;
                    neighbourScript.Cost_H = GetDistanceBetweenNodes(neighbour, EndGoalNode);
                    neighbourScript.parent = node;

                    if (!List_Open.Contains(neighbour)) { 
                        List_Open.Add(neighbour);
                    }
                }
            }


        }

        return false;
    }


    GameObject GetNodeInPosition(float positionX, float positionY)
    {
        int nodeIndex = ((int)positionX * width) + (int)(positionY - positionY % 1);
        if(nodeIndex >= 0 && nodeIndex <= width * width)
        {
            GameObject node = GridNodeList[nodeIndex];
            return node;
        }
        else { return null; }

    }


    int GetDistanceBetweenNodes(GameObject node1, GameObject node2)
    {
        int distanceX = (int)Mathf.Abs(node1.transform.position.x - node2.transform.position.x);
        int distanceY = (int)Mathf.Abs(node1.transform.position.y - node2.transform.position.y);
        return distanceX + distanceY;
    }

    List<GameObject> GetNeighboursOf(GameObject node)
    {
        List<GameObject> Neighbours = new List<GameObject> ();
        int nodeIndex = ((int)node.transform.position.x * width) + (int)(node.transform.position.y - node.transform.position.y % 1);

        if (nodeIndex + 1 < width * width && nodeIndex + 1 >= 0 && nodeIndex % width != width - 1)
        {
            Neighbours.Add(GridNodeList[nodeIndex + 1]);
        }

        if (nodeIndex - 1 < width * width && nodeIndex - 1 >= 0 && nodeIndex % width != 0)
        {
            Neighbours.Add(GridNodeList[nodeIndex - 1]);
        }

        if (nodeIndex + width < width * width && nodeIndex + width >= 0)
        {
            Neighbours.Add(GridNodeList[nodeIndex + width]);
        }

        if (nodeIndex - width < width * width && nodeIndex - width >= 0)
        {
            Neighbours.Add(GridNodeList[nodeIndex - width]);
        }

        return Neighbours;
    }

    List<GameObject> ReTracePath(GameObject node_Start, GameObject node_End)
    {
        List<GameObject> path = new List<GameObject>();
        GameObject current = node_End;

        while(current != node_Start)
        {
            path.Add(current);
            GridNode currentNodeScript = current.GetComponent<GridNode>();
            current = currentNodeScript.parent;
        }

        path.Reverse();

        return path;
    }

    void RemoveFogFromArea()
    {
        float positionX = this.transform.position.x;
        float positionY = this.transform.position.y;

        for(float i = positionX - 3; i < positionX + 4; i++)
        {
            for(float j = positionY - 3; j < positionY + 4; j++)
            {   
                if(i < width && i >= 0 && j < width && j >= 0)
                {
                    GameObject node = GetNodeInPosition(i, j);

                    if (node.GetComponent<GridNode>().bIsFogged)
                    {
                        node.GetComponent<GridNode>().RemoveFog();
                        if (node.GetComponent<GridNode>().bIsWalkable)
                        {
                            VisibleNodes.Add(node);
                        }
                        /*
                        if (node.GetComponent<GridNode>().bRewardNode)
                        {
                            RewardsToGo.Add(node);
                        }*/
                    }
                }
            }
        }
    }

    public void SearchStart(List<GameObject> Rewards)
    {
        lineRenderer.positionCount = 0;
        count = 0;
        VisibleNodes = new List<GameObject>();
        RewardsToGo = new List<GameObject>();
        GridNodeList = Generator.GridNodes;
        width = Generator.width;
        RemoveFogFromArea();

        if (CanSeeRewards)
        {
            foreach(GameObject reward in Rewards)
            {
                foreach(GameObject node in reward.GetComponent<Reward>().Nodes)
                {
                    RewardsToGo.Add(node);
                }
            }
        }

        int loopcount = 0;

        while(Rewards.Count > 0 && loopcount < 10000)
        {
            if (RewardsToGo.Count > 0)
            {
                GameObject selectedReward = RewardsToGo[0];
                int SelectedDistance = GetDistanceBetweenNodes(this.gameObject, selectedReward);
                for(int i = 0; i < RewardsToGo.Count; i++)
                {
                    GameObject node = RewardsToGo[i];
                    if (!node.GetComponent<GridNode>().bRewardNode)
                    {
                        RewardsToGo.Remove(node);
                        continue;
                    }
                    if(GetDistanceBetweenNodes(this.gameObject, node) < SelectedDistance)
                    {
                        selectedReward = node;
                        SelectedDistance = GetDistanceBetweenNodes(this.gameObject, selectedReward);
                    }
                }

                FindPath(selectedReward);
                RewardsToGo.Remove(selectedReward);
            }
            loopcount++;
        }

        container.Print();
        Debug.Log(count);
        Rewards.Clear();
        RewardsToGo.Clear();
    }


}
