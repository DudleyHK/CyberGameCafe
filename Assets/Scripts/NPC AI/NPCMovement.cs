﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class NPCMovement : MonoBehaviour
{
    private List<Node> currentPath = null;
    public  Node currentNode = null;
    public int currentNodeID = 0;
    public  Vector3 currentDir = Vector3.zero;
    public Transform parentTransform = null;
    public bool travellingBegun = false;
    public bool pathComplete = false;
    public Direction CurrentDirection { get; internal set; }



    public enum Direction
    {
        UP,
        UP_RIGHT,
        UP_LEFT,
        DOWN, 
        DOWN_RIGHT,
        DOWN_LEFT,
        LEFT,
        RIGHT,
        NONE
    }



    public int CurrentNodeID
    {
        get
        {
            return currentNodeID;
        }

        set
        {
            currentNodeID = value;
        }
    }



    public bool JourneyToTarget(List<Node> path)
    {
        if(path.Count <= 0)
        {
            print("nothing in the path list");
        }

        if (!travellingBegun)
        {
            currentPath = path;
            currentNode = currentPath[0];
            StartCoroutine(CompletePath());
            travellingBegun = true;
        }
        // FOR DEBUGGING this could be used for all objects when the game is loading to snap all characters to the closest Node.Centre
        // parentTransform.position = currentNode.Centre;

        Debug.Log("TravellingBegun is " + travellingBegun);
        Debug.Log("Path complete is " + pathComplete);
        if (pathComplete)
        {
            travellingBegun = false;
            return true;
        }
        return false;
    }




    /// <summary>
    /// Get the direction of the next node in the list. 
    /// </summary>
    /// <param name="nextNode"></param>
    /// <returns></returns>
    private Direction GetDirection(Node nextNode)
    {
        Direction direction = Direction.NONE;
        bool upFlag = false;
        bool downFlag = false;
        bool leftFlag = false;
        bool rightFlag = false;

        //Debug.Log("current centre: " + currentNode.Centre + " next node centre: " + nextNode.Centre);
        if(currentNode.Centre.x > nextNode.Centre.x)
        {
            leftFlag = true;
            direction = Direction.LEFT;
        }
        else if(currentNode.Centre.x < nextNode.Centre.x)
        {
            rightFlag = true;
            direction = Direction.RIGHT;
        }


        if (currentNode.Centre.y > nextNode.Centre.y)
        {
            downFlag = true;
            direction = Direction.DOWN;
        }
        else if (currentNode.Centre.y < nextNode.Centre.y)
        {
            upFlag = true;
            direction = Direction.UP;
        }

        if(upFlag)
        {
            if(rightFlag)
            {
                direction = Direction.UP_RIGHT; 
            }

            if(leftFlag)
            {
                direction = Direction.UP_LEFT;
            }
        }

        if (downFlag)
        {
            if (rightFlag)
            {
                direction = Direction.DOWN_RIGHT;
            }
            
            if(leftFlag)
            {
                direction = Direction.DOWN_LEFT;
            }
        }
        return direction;
    }



    /// <summary>
    /// Use the current Direction to create a new Vector3 Direction. 
    /// </summary>
    /// <param name="dir"></param>
    private void SetDirectionVec(Direction dir)
    {
        float nodeWidth = GridManager.Instance.NodeWidth;
        float nodeHeight = GridManager.Instance.NodeHeight;

        Vector3 up    = new Vector3(0f, nodeHeight,  0f);
        Vector3 down  = new Vector3(0f, -nodeHeight, 0f);
        Vector3 right = new Vector3(nodeWidth,  0f, 0f);
        Vector3 left  = new Vector3(-nodeWidth, 0f, 0f);


        switch (dir)
        {
            case Direction.UP:
                currentDir = up;
                break;
            case Direction.UP_LEFT:
                currentDir = up + left;
                break;
            case Direction.UP_RIGHT:
                currentDir = up + right;
                break;
            case Direction.DOWN:
                currentDir = down;
                break;
            case Direction.DOWN_LEFT:
                currentDir = down + left;
                break;
            case Direction.DOWN_RIGHT:
                currentDir = down + right;
                break;
            case Direction.RIGHT:
                currentDir = right;
                break;
            case Direction.LEFT:
                currentDir = left;
                break;

            default:
                Debug.Log("No current direction");
                break;
        }
    }


    /// <summary>
    /// Run this loop until the last ID has been hit.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CompletePath()
    {
        int idx = 0;

        int targetID = currentPath[currentPath.Count - 1].ID;
        Debug.Log("TARGET ID : " + targetID);
        while(currentNode.ID != targetID)
        {

            CurrentNodeID = currentNode.ID; 
            Debug.Log("In travelling coroutine next nodeID is " + currentPath[idx + 1].ID);

            Node nextNode = currentPath[idx + 1];
            if ((idx + 1) < currentPath.Count)
            {
                // Check if the next node is occupied
                //if(NodeOccupied(nextNode))
                //{
                                     

                //}


                // Get Direction of nextNode.
                CurrentDirection = GetDirection(nextNode);
                SetDirectionVec(CurrentDirection);

                // If the player is very close to the nextNode, increment Node counter (idx).
                if ((Mathf.Abs(parentTransform.position.x - nextNode.Centre.x) <= 0.05f) &&
                    (Mathf.Abs(parentTransform.position.y - nextNode.Centre.y) <= 0.05f))
                {
                    // This may be too close but can be altered later.
                  //  nextNode.Occupied = true;
                    idx++;
                }
                //nextNode.Occupied = false;
            }


            // Move
            parentTransform.position += currentDir * Time.deltaTime;

            // Set current new Node. 
            currentNode = currentPath[idx];
            //GridManager.Instance.GetNode(currentNode.ID).Occupied = true;
            //Debug.Log("From Node " + currentNode.ID + " to node " + currentPath[idx + 1].ID + " the current direction is " + CurrentDirection);
            // Debug.Log("Moving vector position: " + tranform.position);
            pathComplete = false;
            yield return null;
        }

        pathComplete = true;
        yield return true;
    }



    /// <summary>
    /// Pass in the next node and check if it is occupied. 
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private bool NodeOccupied(Node node)
    {
        if(node.Occupied)
        {
            // either, change the current state to wait and disregard this path. Or Wait for X seconds before continuing.
        }
        return false;
    }
}
