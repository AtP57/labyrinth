using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace LabyrinthGenerator
{
    public abstract class LabyrinthGenerator
    {
        public int heightNodeCount;
        public int widthNodeCount;
        public int[,] labyrinthData;
        protected System.Random random = new System.Random();

        public LabyrinthGenerator(int heightNodeCount, int widthNodeCount)
        {
            this.heightNodeCount = heightNodeCount;
            this.widthNodeCount = widthNodeCount;
            if (heightNodeCount > 0 && widthNodeCount >0)
            {
                InitLabyrinthData();
            }
        }

        protected void InitLabyrinthData()
        {
            this.labyrinthData = new int[(2 * heightNodeCount) + 1, (2 * widthNodeCount) + 1];

            for (int i = 0; i < (2 * heightNodeCount) + 1; i++)
            {
                for (int j = 0; j < (2 * widthNodeCount) + 1; j++)
                {
                    if (i % 2 == 1 && j % 2 == 1)
                    {
                        labyrinthData[i, j] = 0;
                    }
                    else
                    {
                        labyrinthData[i, j] = 1;
                    }
                }
            }

            BuildLabyrinth();
        }

        protected abstract void BuildLabyrinth();

        protected static int[] GenerateRandomArray(int minValue, int maxValue)
        {
            int[] array = new int[(maxValue - minValue) + 1];

            // Fill the array with integers from minValue to maxValue
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = minValue + i;
            }

            // Use Fisher-Yates shuffle algorithm to shuffle the array
            System.Random random = new System.Random();
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }

            return array;
        }

        protected static string MatrixToString(int[,] matrix)
        {
            string to_return = string.Empty;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == 1)
                    {
                        to_return += "#";
                    }
                    else
                    {
                        to_return += "@";
                    }
                }
                to_return += "\n";
            }

            return to_return;
        }

        //carves path between 2 neighbouring nodes
        protected void carvePath(Node a, Node b)
        {
            // Carve path betwen two neighboring nodes, tranlate node position to the labyrinth matrix
            this.labyrinthData[(((2 * a.x) + 1) + ((2 * b.x) + 1)) / 2, (((2 * a.y) + 1) + ((2 * b.y) + 1)) / 2] = 0;
        }

        protected class Node
        {
            public int x;
            public int y;
            public Boolean visited;

            public Node(int i, int j)
            {
                this.x = i;
                this.y = j;
                this.visited = false;
            }
        }
    }

    public class GeneratorDepthFirstSearch : LabyrinthGenerator
    {
        public GeneratorDepthFirstSearch(int heightNodes,  int widthNodes) : base(heightNodes, widthNodes) {}

        protected override void BuildLabyrinth()
        {
            Stack<Node> stack = new Stack<Node>();
            Node[,] nodes = new Node[heightNodeCount, widthNodeCount];

            for (int i = 0;i < heightNodeCount; i++)
            {
                for (int j = 0;j < (widthNodeCount); j++)
                {
                    nodes[i, j] = new Node(i, j);
                }
            }

            //put random starting node in the stack
            stack.Push(nodes[random.Next(heightNodeCount),random.Next(widthNodeCount)]);
            stack.Peek().visited = true;

            //4 direction(up, right, down, left)
            int[] directions;

            while (stack.Count > 0)
            {

                int node_i = stack.Peek().x;
                int node_j = stack.Peek().y;

                //shuffle directions
                directions = GenerateRandomArray(1, 4);
                int[] directions_copy = directions;

                directions = new int[] { 1, 2, 3, 4, 5 };
                for (int i = 0; i < directions_copy.Length; i++)
                {
                    directions[i] = directions_copy[i];
                }

                //try move to another node
                for (int i = 0; i < directions.Length; i++)
                {
                    switch (directions[i])
                    {
                        case 1:
                            //check if we can visit the node above
                            //first: check if there is a node above
                            if (node_i-1 >= 0)
                            {
                                //check if it's already visited
                                if (!nodes[node_i - 1, node_j].visited)
                                {
                                    //if not, then add it to the stack and carve path to it
                                    stack.Push(nodes[node_i - 1, node_j]);
                                    stack.Peek().visited = true;
                                    //carve path
                                    labyrinthData[(2 * node_i), 2 * (node_j) + 1] = 0;
                                    i = directions.Length;
                                }
                            }
                            break;

                        case 2:
                            //check if we can visit the node to the right
                            //first: check if there is a node to the right
                            if (node_j + 1 < widthNodeCount)
                            {
                                //check if it's already visited
                                if (!nodes[node_i, node_j + 1].visited)
                                {
                                    //if not, then add it to the stack and carve path to it
                                    stack.Push(nodes[node_i, node_j + 1]);
                                    stack.Peek().visited = true;
                                    //carve path
                                    labyrinthData[(2 * node_i) + 1, 2 * (node_j) + 2] = 0;
                                    i = directions.Length;
                                }
                            }
                            break;

                        case 3:
                            //check if we can visit the node below
                            //first: check if there is a node below
                            if (node_i + 1 < heightNodeCount)
                            {
                                //check if it's already visited
                                if (!nodes[node_i + 1, node_j].visited)
                                {
                                    //if not, then add it to the stack and carve path to it
                                    stack.Push(nodes[node_i + 1, node_j]);
                                    stack.Peek().visited = true;
                                    //carve path
                                    labyrinthData[(2 * node_i) + 2, 2 * (node_j) + 1] = 0;
                                    i = directions.Length;
                                }
                            }
                            break;

                        case 4:
                            //check if we can visit the node to the left
                            //first: check if there is a node to the left
                            if (node_j - 1 >= 0)
                            {
                                //check if it's already visited
                                if (!nodes[node_i, node_j - 1].visited)
                                {
                                    //if not, then add it to the stack and carve path to it
                                    stack.Push(nodes[node_i, node_j - 1]);
                                    stack.Peek().visited = true;
                                    //carve path
                                    labyrinthData[(2 * node_i) + 1, 2 * (node_j)] = 0;
                                    i = directions.Length;
                                }
                            }
                            break;

                        default:
                            stack.Pop();
                            break;

                    }
                }
            }
        }
    }

    public class GeneratorWidthFirstSearch : LabyrinthGenerator
    {
        public GeneratorWidthFirstSearch(int heightNodes, int widthNodes) : base(heightNodes, widthNodes) {}

        protected override void BuildLabyrinth()
        {
            LinkedList<Node> stack = new LinkedList<Node>();
            Node[,] nodes = new Node[heightNodeCount, widthNodeCount];

            for (int i = 0; i < heightNodeCount; i++)
            {
                for (int j = 0; j < (widthNodeCount); j++)
                {
                    nodes[i, j] = new Node(i, j);
                }
            }

            //put random starting node in the stack
            stack.AddLast(nodes[random.Next(heightNodeCount), random.Next(widthNodeCount)]);
            stack.Last.Value.visited = true;

            //4 direction(up, right, down, left)
            int[] directions;

            while (stack.Count > 0)
            {

                int node_i = stack.First.Value.x;
                int node_j = stack.First.Value.y;

                //shuffle directions
                directions = GenerateRandomArray(1, 4);
                int[] directions_copy = directions;

                directions = new int[] { 1, 2, 3, 4, 5 };
                for (int i = 0; i < directions_copy.Length; i++)
                {
                    directions[i] = directions_copy[i];
                }

                //try move to another node
                for (int i = 0; i < directions.Length; i++)
                {
                    switch (directions[i])
                    {
                        case 1:
                            //check if we can visit the node above
                            //first: check if there is a node above
                            if (node_i - 1 >= 0)
                            {
                                //check if it's already visited
                                if (!nodes[node_i - 1, node_j].visited)
                                {
                                    //if not, then add it to the stack and carve path to it
                                    stack.AddLast(nodes[node_i - 1, node_j]);
                                    stack.Last.Value.visited = true;
                                    //carve path
                                    labyrinthData[(2 * node_i), 2 * (node_j) + 1] = 0;
                                    i = directions.Length;
                                }
                            }
                            break;

                        case 2:
                            //check if we can visit the node to the right
                            //first: check if there is a node to the right
                            if (node_j + 1 < widthNodeCount)
                            {
                                //check if it's already visited
                                if (!nodes[node_i, node_j + 1].visited)
                                {
                                    //if not, then add it to the stack and carve path to it
                                    stack.AddLast(nodes[node_i, node_j + 1]);
                                    stack.Last.Value.visited = true;
                                    //carve path
                                    labyrinthData[(2 * node_i) + 1, 2 * (node_j) + 2] = 0;
                                    i = directions.Length;
                                }
                            }
                            break;

                        case 3:
                            //check if we can visit the node below
                            //first: check if there is a node below
                            if (node_i + 1 < heightNodeCount)
                            {
                                //check if it's already visited
                                if (!nodes[node_i + 1, node_j].visited)
                                {
                                    //if not, then add it to the stack and carve path to it
                                    stack.AddLast(nodes[node_i + 1, node_j]);
                                    stack.Last.Value.visited = true;
                                    //carve path
                                    labyrinthData[(2 * node_i) + 2, 2 * (node_j) + 1] = 0;
                                    i = directions.Length;
                                }
                            }
                            break;

                        case 4:
                            //check if we can visit the node to the left
                            //first: check if there is a node to the left
                            if (node_j - 1 >= 0)
                            {
                                //check if it's already visited
                                if (!nodes[node_i, node_j - 1].visited)
                                {
                                    //if not, then add it to the stack and carve path to it
                                    stack.AddLast(nodes[node_i, node_j - 1]);
                                    stack.Last.Value.visited = true;
                                    //carve path
                                    labyrinthData[(2 * node_i) + 1, 2 * (node_j)] = 0;
                                    i = directions.Length;
                                }
                            }
                            break;

                        default:
                            stack.RemoveFirst();
                            break;

                    }
                }
            }
        }
    }

    public class GeneratorIterativeRandomizedPrim : LabyrinthGenerator
    {
        public GeneratorIterativeRandomizedPrim(int heightNodes, int widthNodes) : base(heightNodes, widthNodes) { }

        protected override void BuildLabyrinth()
        {
            LinkedList<Node> frontierCells = new LinkedList<Node>();
            Node[,] nodes = new Node[heightNodeCount, widthNodeCount];

            for (int i = 0; i < heightNodeCount; i++)
            {
                for (int j = 0; j < (widthNodeCount); j++)
                {
                    nodes[i, j] = new Node(i, j);
                }
            }

            //put random starting node in the frontierCells
            ref Node startingNode = ref nodes[random.Next(heightNodeCount), random.Next(widthNodeCount)];

            //MonoBehaviour.print("Starting node: " + startingNode.i + "," + startingNode.j);

            //set the first node to visited
            startingNode.visited = true;
            //MonoBehaviour.print("Node " + startingNode.i + "," + startingNode.j + " is set to visited");
            AddFrontierCells(ref frontierCells, nodes, startingNode);

            //generate the maze until the list is empty
            while (frontierCells.Count > 0)
            {
                int randomIndex = random.Next(frontierCells.Count);
                //pick random frontier cell
                Node pickedCell = frontierCells.ElementAt(randomIndex);
                frontierCells.Remove(pickedCell);

                //set it to visited
                nodes[pickedCell.x, pickedCell.y].visited = true;
                //MonoBehaviour.print("Node " + pickedCell.i + "," + pickedCell.j + " is set to visited");

                //carve path between the picked cell and the maze
                int[] directions = GenerateRandomArray(1, 4);
                for (int i = 0; i < directions.Length; i++)
                {
                    switch (directions[i])
                    {
                        //check if path can be made to the upper neighbour
                        case 1:
                            if (pickedCell.x - 1 >= 0 && nodes[pickedCell.x - 1, pickedCell.y].visited)
                            {
                                carvePath(pickedCell, nodes[pickedCell.x - 1, pickedCell.y]);
                                i = directions.Length;
                            }
                            break;

                        //check if path can be made to the lower neighbour
                        case 2:
                            if (pickedCell.x + 1 < nodes.GetLength(0) && nodes[pickedCell.x + 1, pickedCell.y].visited)
                            {
                                carvePath(pickedCell, nodes[pickedCell.x + 1, pickedCell.y]);
                                i = directions.Length;
                            }
                            break;

                        //check if path can be made to the leftern neighbour
                        case 3:
                            if (pickedCell.y - 1 >= 0 && nodes[pickedCell.x, pickedCell.y - 1].visited)
                            {
                                carvePath(pickedCell, nodes[pickedCell.x, pickedCell.y - 1]);
                                i = directions.Length;
                            }
                            break;

                        //check if path can be made to the rightern neighbour
                        case 4:
                            if (pickedCell.y + 1 < nodes.GetLength(1) && nodes[pickedCell.x, pickedCell.y + 1].visited)
                            {
                                carvePath(pickedCell, nodes[pickedCell.x, pickedCell.y + 1]);
                                i = directions.Length;
                            }
                            break;
                    }

                    //refrest the frontier cell list
                    AddFrontierCells(ref frontierCells, nodes, pickedCell);
                }
            }


        }

        private void AddFrontierCells(ref LinkedList<Node> frontierCells, Node[,] nodes, Node node)
        {
            //Add upper node to frontier cells
            //check if it's eligible
            if (node.x - 1 >= 0 && !nodes[node.x - 1, node.y].visited && !frontierCells.Contains(nodes[node.x - 1, node.y]))
            {
                //MonoBehaviour.print("Node " + (node.i-1) + "," + node.j + " is added to frontier cells");
                frontierCells.AddLast(nodes[node.x - 1, node.y]);
            }

            //Add lower node to frontier cells
            //check if it's eligible
            if (node.x + 1 < nodes.GetLength(0) && !nodes[node.x + 1, node.y].visited && !frontierCells.Contains(nodes[node.x + 1, node.y]))
            {
                //MonoBehaviour.print("Node " + (node.i+1) + "," + node.j + " is added to frontier cells");
                frontierCells.AddLast(nodes[node.x + 1, node.y]);
            }

            //Add leftern node to frontier cells
            //check if it's eligible
            if (node.y - 1 >= 0 && !nodes[node.x, node.y - 1].visited && !frontierCells.Contains(nodes[node.x, node.y - 1]))
            {
                //MonoBehaviour.print("Node " + node.i + "," + (node.j-1) + " is added to frontier cells");
                frontierCells.AddLast(nodes[node.x, node.y - 1]);
            }

            //Add leftern node to frontier cells
            //check if it's eligible
            if (node.y + 1 < nodes.GetLength(1) && !nodes[node.x, node.y + 1].visited && !frontierCells.Contains(nodes[node.x, node.y + 1]))
            {
                //MonoBehaviour.print("Node " + node.i + "," + (node.j+1) + " is added to frontier cells");
                frontierCells.AddLast(nodes[node.x, node.y + 1]);
            }
        }

    }

    public class GeneratorTessellation : LabyrinthGenerator
    {
        private int iterations;
        public GeneratorTessellation(int heightNodes, int widthNodes) : base(-1,-1) { 
            this.iterations = (int)Math.Ceiling((double)Math.Min(heightNodes, widthNodes)/3);
            int[,] labyrinth = { { 0, 1, 0 }, { 1, 0, 1 }, { 0, 1, 0 } };
            this.labyrinthData = labyrinth;
            BuildLabyrinth();
        }

        protected override void BuildLabyrinth()
        {
            //init first iteration
            int[,] labyrinth = { { 1, 1, 1 }, { 1, 0, 1 }, { 1, 1, 1 } };
            for (int i = 0; i < this.iterations; ++i)
            {
                int[,] newLabyrinth = new int[(2*labyrinth.GetLength(0))-1, (2 * labyrinth.GetLength(0)) - 1];

                //make the left and top side of the labyrinth a wall
                for (int j = 0; j < newLabyrinth.GetLength(0); j++)
                {
                    newLabyrinth[j, 0] = 1;
                    newLabyrinth[0, j] = 1;
                }

                //copy the smaller labyrinth 4 times into the bigger labyrinth
                for (int k = 0; k <= 1; k++)
                {
                    for (int l = 0; l <= 1; l++)
                    {
                        //k & l give the starter points for the matrix copying, abd m & n is the 2 for cycle the copies the matrix
                        for (int m = 1; m < labyrinth.GetLength(0); m++)
                        {
                            for (int n = 1; n < labyrinth.GetLength(0); n++)
                            {
                                newLabyrinth[(k*(labyrinth.GetLength(0)-1))+m, (l * (labyrinth.GetLength(0) - 1))+n] = labyrinth[m,n];
                            }
                        }
                    }
                }

                //connect the 4 smaller sections
                int indexOfSkippedWall = random.Next(4);

                //remove wall on the left side of the x axis
                if (indexOfSkippedWall != 0)
                {
                    newLabyrinth[(newLabyrinth.GetLength(0) - 1) / 2, ((random.Next(0, i) * 2) + 1)] = 0;
                }
                //remove wall on the right side of the x axis
                if (indexOfSkippedWall != 1)
                {
                    newLabyrinth[(newLabyrinth.GetLength(0) - 1) / 2, ((random.Next(0, i) * 2) + ((newLabyrinth.GetLength(0)-1)/2)+1)] = 0;
                }
                //remove wall on the left side of the y axis
                if (indexOfSkippedWall != 2)
                {
                    newLabyrinth[((random.Next(0, i) * 2) + 1), (newLabyrinth.GetLength(0) - 1) / 2] = 0;
                }
                //remove wall on the right side of the y axis
                if (indexOfSkippedWall != 3)
                {
                    newLabyrinth[((random.Next(0, i) * 2) + ((newLabyrinth.GetLength(0) - 1) / 2) + 1), (newLabyrinth.GetLength(0) - 1) / 2] = 0;
                }

                labyrinth = newLabyrinth;
            }

            this.labyrinthData = labyrinth;
        }
    }

    public class GeneratorAldousBroder : LabyrinthGenerator
    {
        public GeneratorAldousBroder(int heightNodes, int widthNodes) : base(heightNodes, widthNodes) { }

        protected override void BuildLabyrinth()
        {
            Node[,] nodes = new Node[heightNodeCount, widthNodeCount];
            int unvisitedNodes = heightNodeCount*widthNodeCount;
            for (int i = 0; i < heightNodeCount; i++)
            {
                for (int j = 0; j < (widthNodeCount); j++)
                {
                    nodes[i, j] = new Node(i, j);
                }
            }

            //Select a random starting node
            Node selectedNode = nodes[random.Next(heightNodeCount), random.Next(widthNodeCount)];
            selectedNode.visited = true;
            unvisitedNodes--;

            while (unvisitedNodes > 0)
            {

                //shuffle directions
                int[] directions = GenerateRandomArray(1, 4);

                //try move to another node
                for (int i = 0; i < directions.Length; i++)
                {
                    switch (directions[i])
                    {
                        case 1:
                            //check if we can visit the node above
                            //first: check if there is a node above
                            if (selectedNode.x - 1 >= 0)
                            {
                                //check if it's already visited
                                if (nodes[selectedNode.x - 1, selectedNode.y].visited)
                                {
                                    selectedNode = nodes[selectedNode.x - 1, selectedNode.y];
                                    i = directions.Length;
                                } else
                                {
                                    carvePath(selectedNode, nodes[selectedNode.x - 1, selectedNode.y]);
                                    selectedNode = nodes[selectedNode.x - 1, selectedNode.y];
                                    selectedNode.visited = true;
                                    unvisitedNodes--;
                                    i = directions.Length;
                                }
                            }
                            break;

                        case 2:
                            //check if we can visit the node to the right
                            //first: check if there is a node to the right
                            if (selectedNode.y + 1 < widthNodeCount)
                            {
                                //check if it's already visited
                                if (nodes[selectedNode.x, selectedNode.y + 1].visited)
                                {
                                    selectedNode = nodes[selectedNode.x, selectedNode.y + 1];
                                    i = directions.Length;
                                } else
                                {
                                    carvePath(selectedNode, nodes[selectedNode.x, selectedNode.y + 1]);
                                    selectedNode = nodes[selectedNode.x, selectedNode.y + 1];
                                    selectedNode.visited = true;
                                    unvisitedNodes--;
                                    i = directions.Length;
                                }
                            }
                            break;

                        case 3:
                            //check if we can visit the node below
                            //first: check if there is a node below
                            if (selectedNode.x + 1 < heightNodeCount)
                            {
                                //check if it's already visited
                                if (nodes[selectedNode.x + 1, selectedNode.y].visited)
                                {
                                    selectedNode = nodes[selectedNode.x + 1, selectedNode.y];
                                    i = directions.Length;
                                } else
                                {
                                    carvePath(selectedNode, nodes[selectedNode.x + 1, selectedNode.y]);
                                    selectedNode = nodes[selectedNode.x + 1, selectedNode.y];
                                    selectedNode.visited = true;
                                    unvisitedNodes--;
                                    i = directions.Length;
                                }
                            }
                            break;

                        case 4:
                            //check if we can visit the node to the left
                            //first: check if there is a node to the left
                            if (selectedNode.y - 1 >= 0)
                            {
                                //check if it's already visited
                                if (nodes[selectedNode.x, selectedNode.y - 1].visited)
                                {
                                    selectedNode = nodes[selectedNode.x, selectedNode.y - 1];
                                    i = directions.Length;
                                } else
                                {
                                    carvePath(selectedNode, nodes[selectedNode.x, selectedNode.y - 1]);
                                    selectedNode = nodes[selectedNode.x, selectedNode.y - 1];
                                    selectedNode.visited = true;
                                    unvisitedNodes--;
                                    i = directions.Length;
                                }
                            }
                            break;
                    }
                }
            }
        }
    }
}
