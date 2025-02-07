using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Container : MonoBehaviour
{
    Dictionary<int, GameObject> rewardList;
    int index;

    void Start()
    {
        rewardList = new Dictionary<int, GameObject>();
        index = 0;
    }


    public void AddToDictionary(GameObject obj)
    {
        //rewardList.Add(index, obj);
        index++;
        //Debug.Log(object.name + " Bulundu! :" + position + "Konumunda.");
    }

    public void Print()
    {
        for(int i = 0; i < 4; i++)
        {
            for (int j = 0; j < index; j++)
            {
                switch (i)
                {
                    case 0:
                        if (rewardList[j].name == "Gold(Clone)")
                        {
                            Debug.Log("Gold Bulundu!: " + rewardList[j].transform.position);
                            Destroy(rewardList[j]);
                        }
                        break;
                    case 1:
                        if (rewardList[j].name == "Emerald(Clone)")
                        {
                            Debug.Log("Emerald Bulundu!: " + rewardList[j].transform.position);
                            Destroy(rewardList[j]);
                        }
                        break;
                    case 2:
                        if (rewardList[j].name == "Silver(Clone)")
                        {
                            Debug.Log("Silver Bulundu!: " + rewardList[j].transform.position);
                            Destroy(rewardList[j]);
                        }
                        break;
                    case 3:
                        if (rewardList[j].name == "Copper(Clone)")
                        {
                            Debug.Log("Copper Bulundu!: " + rewardList[j].transform.position);
                            Destroy(rewardList[j]);
                        }
                        break;
                }
                
            }
        }
        rewardList.Clear();
        index = 0;
        
    }
}
