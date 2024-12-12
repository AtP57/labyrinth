using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LabyrinthGenerator;
using System;
using Random = System.Random;

public class Builder : MonoBehaviour
{
    public GameObject cubeBlack;
    public GameObject cubeWhite;
    public GameObject platform;
    public GameObject goalPrefab;
    public GameObject ball;

    public GameObject wallContainer;


    private int generatorAlgorithm;
    private int heightNodes;
    private int widthNodes;

    private int[,] _labyrinthData;
    private GameObject playerBall;

    public void Init()
    {
        this.generatorAlgorithm = GlobalVariables.selectedAlgorithm;
        this.widthNodes = GlobalVariables.selectedWidthNodes;
        this.heightNodes = GlobalVariables.selectedHeightNodes;

        SelectAlgorithm(generatorAlgorithm);
        PlaceGoal();
        PlaceBall();
        Build();
        wallContainer.GetComponent<MeshMerger>().MergeMeshes();
    }

    private void Build()
    {
        // Declaring an offset, so the middle of the labyrinth will be at 0,0
        Vector3 offsetValue = new Vector3(-(float)_labyrinthData.GetLength(0) / 2 + 0.5f, 0, -(float)_labyrinthData.GetLength(1) / 2 + 0.5f);

        // Offset builder, so the center of the maze will align with the center of rotation
        transform.position = offsetValue;

        // Placing game objects
        for (int i = 0; i < _labyrinthData.GetLength(0); i++)
        {
            for (int j = 0; j < _labyrinthData.GetLength(1); j++)
            {
                transform.position = new Vector3(i, 0.6f, j) + offsetValue;
                PlaceElement(_labyrinthData[i, j]);
            }
        }

        PlacePlatform();
    }

    private void PlacePlatform()
    {
        // Building platform
        GameObject platformPlaced = Instantiate(platform, Vector3.zero, Quaternion.identity);
        platformPlaced.transform.SetParent(transform.parent);

        //Scaling platform
        Vector3 scale = new Vector3(_labyrinthData.GetLength(0), 0.2f, _labyrinthData.GetLength(1));
        platformPlaced.transform.localScale = scale;
    }

    private string to_string()
    {
        string to_return = string.Empty;

        for (int i = 0; i < _labyrinthData.GetLength(0); i++)
        {
            for (int j = 0; j < _labyrinthData.GetLength(1); j++)
            {
                if (_labyrinthData[i, j] == 1)
                {
                    to_return += "#";
                } else
                {
                    to_return += "@";
                }
            }
            to_return += "\n";
        }

        return to_return;
    }

    private void PlaceBall()
    {
        if (_labyrinthData[1, (_labyrinthData.GetLength(1) - 1) / 2] == 0)
        {
            _labyrinthData[1, (_labyrinthData.GetLength(1) - 1) / 2] = 3;
        }
        else
        {
            _labyrinthData[1, (_labyrinthData.GetLength(1) - 3) / 2] = 3;
        }
    }


    private void PlaceGoal()
    {
        if (_labyrinthData[_labyrinthData.GetLength(0)-2, (_labyrinthData.GetLength(1) - 1) / 2] == 0)
        {
            _labyrinthData[_labyrinthData.GetLength(0)-1, (_labyrinthData.GetLength(1)-1)/2] = 2;
        }
        else
        {
            _labyrinthData[_labyrinthData.GetLength(0)-1, (_labyrinthData.GetLength(1)-3)/2] = 2;
        }
    }

    private void SelectAlgorithm(int generatorAlgorithm)
    {
        int algorithmId = generatorAlgorithm;

        // If random algorithm was selected one randomly
        if (algorithmId == 1) {
            Random rand = new Random();
            algorithmId = rand.Next(2,7);
        }

        // Select the algorithm corresponding to the id
        switch (algorithmId)
        {
            case 2:
                _labyrinthData = new GeneratorDepthFirstSearch(heightNodes, widthNodes).labyrinthData;
                break;
            case 3:
                _labyrinthData = new GeneratorWidthFirstSearch(heightNodes, widthNodes).labyrinthData;
                break;
            case 4:
                _labyrinthData = new GeneratorIterativeRandomizedPrim(heightNodes, widthNodes).labyrinthData;
                break;
            case 5:
                _labyrinthData = new GeneratorTessellation(heightNodes, widthNodes).labyrinthData;
                break;
            case 6:
                _labyrinthData = new GeneratorAldousBroder(heightNodes, widthNodes).labyrinthData;
                break;                    
            default:
                Debug.Log("Invalid generator id"); 
                throw new Exception("Generator Algortihm not selected");
        }
    }

    private void PlaceElement(int element)
    {
        switch (element)
        {
            //Place Wall
            case 1:
                GameObject wall = Instantiate(cubeBlack, transform.position, transform.rotation);
                wall.transform.SetParent(wallContainer.transform);
                break;
            //Place Goal
            case 2:
                GameObject goal = Instantiate(goalPrefab, transform.position, transform.rotation);
                goal.transform.SetParent(transform.parent);
                goal.GetComponent<GoalBehaviour>().ball = playerBall;
                break;
            //Place Ball
            case 3:
                playerBall = Instantiate(ball, transform.position, transform.rotation);
                playerBall.transform.SetParent(transform.parent);
                break;
        }
    }
}
