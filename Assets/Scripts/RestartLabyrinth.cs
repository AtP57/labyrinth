using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RestartLabyrinth : MonoBehaviour
{

    public GameObject labyrinthBuilderPrefab;
    public GameObject wallContainerPrefab;

    private GameObject labyrinthBuilder;
    // Start is called before the first frame update
    void Start()
    {
        StartLabyrinth();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartLabyrinth()
    {
        DestroyAllChildren();


        //reset rotation
        transform.rotation = Quaternion.identity;

        //create a container for the walls
        GameObject wallContainer = Instantiate(wallContainerPrefab, Vector3.zero, Quaternion.identity);
        wallContainer.transform.SetParent(transform);

        //funny GameObject labyrinthBuilder = Instantiate(LabyrinthBuilder, transform.position, transform.rotation);
        GameObject labyrinthBuilder = Instantiate(labyrinthBuilderPrefab, Vector3.zero, Quaternion.identity);
        labyrinthBuilder.transform.SetParent(transform);

        labyrinthBuilder.GetComponent<Builder>().wallContainer = wallContainer;
        labyrinthBuilder.GetComponent<Builder>().Init();
    }

    private void DestroyAllChildren()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
