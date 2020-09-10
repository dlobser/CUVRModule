using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyManager : MonoBehaviour
{
    public int amount = 10;
    public GameObject butterfly;
    public GameObject[] butterflies;
    public List<GameObject> butterFlyList;
    GameObject container;


    public float speedOffset = 5;

    void Start()
    {
        butterflies = new GameObject[amount];
        butterFlyList = new List<GameObject>();
        container = new GameObject();

        for (int i = 0; i < amount; i++)
        {
            butterflies[i] = Instantiate(butterfly,container.transform);
            butterflies[i].transform.GetChild(0).transform.localPosition = new Vector3(i,0,0);
            butterflies[i].GetComponent<ButterflyController_Generic>().speed = speedOffset * i;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            butterFlyList.Add(Instantiate(butterfly));
        }
        for (int i = 0; i < amount; i++)
        {
            butterflies[i].GetComponent<ButterflyController_Generic>().speed = speedOffset * i;
        }
    }
}
