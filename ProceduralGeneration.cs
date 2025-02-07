using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] GameObject PlayerCharacter;
    [SerializeField] Transform camTransform;
    [SerializeField] GameObject Frame;

    public int width, height;

    [Range(0.0f, 1.0f)]
    [SerializeField] float ObstacleAmountMultiplier;


    [SerializeField] GameObject[] ObstacleArray;
    GameObject SelectedObstacle;

    [Range(0.0f, 1.0f)]
    [SerializeField] float RewardAmountMultiplier;

    [SerializeField] GameObject[] RewardTypesArray;
    GameObject SelectedReward;

    List<GameObject> Rewards;

    [SerializeField] GameObject inputField;

    [HideInInspector]
    public List<GameObject> GridNodes;

    List<GameObject> Obstacles;


    // Start is called before the first frame update
    void Start()
    {
        Obstacles = new List<GameObject>();
        GridNodes = new List<GameObject> ();
        Rewards = new List<GameObject> ();
        
    }


    public void GenerateMap()
    {
        //Set height and width
        string text = inputField.GetComponent<TMP_InputField>().text;
        int.TryParse(text, out int result);

        height = result;
        width = result;  

        //Destroy all spawned obstacles to generate new ones
        if (Obstacles.Count != 0)
        {
            for (int i = 0; i < GridNodes.Count; i++)
            {
                Destroy(GridNodes[i]);
            }
            GridNodes.Clear();

            for (int i = 0; i < Obstacles.Count; i++)
            {
                Destroy(Obstacles[i]);
            }
            Obstacles.Clear();

        }

        //Spawn the grid
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject var = (GameObject)Instantiate(Frame, new Vector2(x + 0.5f, y + 0.5f), Quaternion.identity);
                GridNodes.Add(var);
            }
        }


        //Spawn minimum amount of obstacles
        SpawnObstacles(2);
        


        //Move the character to a random starting position
        Character CharacterScript = PlayerCharacter.GetComponent<Character>();
        bool positionAvailable = false;
        while (!positionAvailable)
        {
            GameObject SelectedNode = GridNodes[Random.Range(0, width * height)];
            GridNode SelectedNodeScript = SelectedNode.GetComponent<GridNode>();
            if (SelectedNodeScript != null)
            {
                if (SelectedNodeScript.bIsWalkable)
                {
                    positionAvailable = true;
                    CharacterScript.MoveCharacter(SelectedNode);
                }
            }
        }



        //Spawn the rewards of all types
        for (int i = 0; i < RewardTypesArray.Length; i++)
        {
            SelectedReward = RewardTypesArray[i];
            int j = 0;

            //Counts how many loops to prevent the loop to go infinite
            int LoopCount = 0;
            //Spawn the rewards of the given type
            while (j < 5 + (width * RewardAmountMultiplier) && LoopCount < 10000)
            {
                Reward rewardScript;
                GameObject var = (GameObject)Instantiate(SelectedReward, new Vector2(Random.Range(0, width), Random.Range(0, height)), Quaternion.identity);
                rewardScript = var.GetComponent<Reward>();
                if (rewardScript != null) { rewardScript.SetHeightWidth(height); }

                bool CanSpawn = true;

                //If the spawned reward intersects with another object that is already spawned, destroy the new spawned reward
                if (Obstacles.Count != 0)
                {
                    for (int obs = 0; obs < Obstacles.Count; obs++)
                    {
                        if (Obstacles[obs].GetComponent<Renderer>().bounds.Intersects(var.GetComponent<Renderer>().bounds))
                        {
                            CanSpawn = false;
                            Destroy(var);
                        }
                    }
                }
                //If the spawned obstacle is not intersecting with another obstacle, add it to the list
                if (CanSpawn)
                {
                    bool pathAvailable = true;
                    foreach(GameObject node in var.GetComponent<Reward>().Nodes)
                    {
                        if (!CharacterScript.FindPath(node))
                        {
                            pathAvailable = false;
                            break;
                        }
                    }
                    if(pathAvailable) {
                        Obstacles.Add(var);
                        SetRewardNodes(var);
                        Rewards.Add(var);
                        j++;
                    }
                }
                LoopCount++;
            }
        }

        //Spawn additional obstacles
        SpawnObstacles((int)(width * ObstacleAmountMultiplier));

        camTransform.transform.position = PlayerCharacter.transform.position + new Vector3(0,0, -10);
    } 

    //Start the game
    public void StartGame()
    {
        foreach (GameObject node in GridNodes)
        {
            GridNode nodeScript = node.GetComponent<GridNode>();
            nodeScript.AddFog();
        }

        PlayerCharacter.GetComponent<Character>().SearchStart(Rewards);
    }

    //Set nodes that are inside the reward as reward node 
    void SetRewardNodes(GameObject rw)
    {
        float positionX = rw.transform.position.x;
        float positionY = rw.transform.position.y;

        int nodeIndex_TopLeft = ((int)(positionX - 0.5f) * width) + (int)(positionY + 0.5f - positionY % 1);
        int nodeIndex_TopRight = ((int)(positionX + 0.5f) * width) + (int)(positionY + 0.5f - positionY % 1);
        int nodeIndex_BottomLeft = ((int)(positionX - 0.5f) * width) + (int)(positionY - 0.5f - positionY % 1);
        int nodeIndex_BottomRight = ((int)(positionX + 0.5f) * width) + (int)(positionY - 0.5f- positionY % 1);


        GameObject node_TopLeft = GridNodes[nodeIndex_TopLeft];
        node_TopLeft.GetComponent<GridNode>().bRewardNode = true;
        node_TopLeft.GetComponent<GridNode>().reward = rw;

        GameObject node_TopRight = GridNodes[nodeIndex_TopRight];
        node_TopRight.GetComponent<GridNode>().bRewardNode = true;
        node_TopRight.GetComponent<GridNode>().reward = rw;

        GameObject node_BottomLeft = GridNodes[nodeIndex_BottomLeft];
        node_BottomLeft.GetComponent<GridNode>().bRewardNode = true;
        node_BottomLeft.GetComponent<GridNode>().reward = rw;

        GameObject node_BottomRight = GridNodes[nodeIndex_BottomRight];
        node_BottomRight.GetComponent<GridNode>().bRewardNode = true;
        node_BottomRight.GetComponent<GridNode>().reward = rw;

        rw.GetComponent<Reward>().Nodes.Add(node_TopLeft);
        rw.GetComponent<Reward>().Nodes.Add(node_TopRight);
        rw.GetComponent<Reward>().Nodes.Add(node_BottomLeft);
        rw.GetComponent<Reward>().Nodes.Add(node_BottomRight);
    }

    void SpawnObstacles(int amount)
    {
        //Spawn the obstacles of all types
        for (int i = 0; i < ObstacleArray.Length; i++)
        {
            SelectedObstacle = ObstacleArray[i];
            int j = 0;

            //Counts how many loops to prevent the loop to go infinite
            int LoopCount = 0;
            //Spawn the obstacles of the given type
            while (j < amount && LoopCount < 1000)
            {
                Obstacle obstacleScript;
                GameObject var = (GameObject)Instantiate(SelectedObstacle, new Vector2(Random.Range(0, width), Random.Range(0, height)), Quaternion.identity);
                obstacleScript = var.GetComponent<Obstacle>();
                if (obstacleScript != null) { obstacleScript.SetHeightWidth(height); }


                bool CanSpawn = true;

                //If the spawned obstacle intersects with another obstacle that is already spawned, destroy the new spawned obstacle
                if (Obstacles.Count != 0)
                {
                    for (int obs = 0; obs < Obstacles.Count; obs++)
                    {

                        if (var.tag == "Mountain" && Obstacles[obs].tag == "Mountain") { continue; }
                        if (Obstacles[obs].GetComponent<Renderer>().bounds.Intersects(var.GetComponent<Renderer>().bounds))
                        {
                            CanSpawn = false;
                            Destroy(var);
                        }
                    }
                    if (PlayerCharacter.GetComponent<Renderer>().bounds.Intersects(var.GetComponent<Renderer>().bounds))
                    {
                        CanSpawn = false;
                        Destroy(var);
                    }
                }
                //If the spawned obstacle is not intersecting with another obstacle, add it to the list
                if (CanSpawn)
                {
                    Obstacles.Add(var);
                    if (obstacleScript != null) { obstacleScript.SetGridNodesUnwalkable(GridNodes); }
                    j++;
                }
                LoopCount++;
            }
        }
    }
}
