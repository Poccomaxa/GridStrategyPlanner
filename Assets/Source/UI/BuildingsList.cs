using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingsList : MonoBehaviour
{
    public List<Foundation> buildingList;
    public GameObject buttonPrefab;
    public Controller controller;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Foundation foundation in buildingList)
        {
            GameObject newButton = GameObject.Instantiate(buttonPrefab, gameObject.transform);
            Text buttonText = newButton.GetComponentInChildren<Text>();
            buttonText.text = foundation.name;

            newButton.GetComponent<Button>().onClick.AddListener(() => controller.OnSelectBuilding(foundation));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
