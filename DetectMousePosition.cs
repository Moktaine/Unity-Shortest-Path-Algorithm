using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectMousePosition : MonoBehaviour
{
    [SerializeField] GameObject Generator;
    [SerializeField] Character PlayerCharacter;

    public List<GameObject> GridNodeList;
    int width;
    GameObject selectedNode;
 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ProceduralGeneration generationScript = Generator.GetComponent<ProceduralGeneration>();
            GridNodeList = generationScript.GridNodes;
            width = generationScript.width;
        }

        if(Input.GetMouseButtonDown(0))
        {
            if( !(GridNodeList.Count > 0) ) { return; }
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            int index = ((int)mouseWorldPos.x * width) + (int)(mouseWorldPos.y - mouseWorldPos.y % 1);
            

            if(selectedNode != null)
            {
                GridNode nodeScript = selectedNode.GetComponent<GridNode>();

                if (nodeScript != null)
                {
                    nodeScript.SetWalkable();
                }
            }
            selectedNode = GridNodeList[index];

            if (selectedNode != null)
            {
                GridNode nodeScript = selectedNode.GetComponent<GridNode>();

                if(nodeScript != null ) {
                    if( nodeScript.bIsWalkable) {
                        if( nodeScript.bRewardNode) { Debug.Log("Yes!"); }
                        //nodeScript.SetEndGoal();
                    }
                }

            }

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(selectedNode == null) {  return; }

            //PlayerCharacter.FindPath(selectedNode);
        }

    }
}
