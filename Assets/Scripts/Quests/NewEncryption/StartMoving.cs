﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMoving : MonoBehaviour {

    float direction;
    float speed;

    void Start()
    {
        speed = 2f;
        direction = Random.Range(0f, Mathf.PI * 2);
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        switch(c.gameObject.tag)
        {
            case "xWall":
                direction = ((Mathf.PI / 2) - direction) + (Mathf.PI / 2);
                break;
            case "yWall":
                direction = ((Mathf.PI * 2) - direction);
                break;
            case "Player":
                if (gameObject.tag == "CorrectLetter")
                {
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<Spawn>().newLetter();
                    GameObject.FindGameObjectWithTag("GreenLetter").GetComponent<CorrectLetter>().advance();
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>().penalty(5f);
                    Destroy(gameObject);
                }
                break;
            default:
                speed *= -1;
                break;
        }
    }

    void FixedUpdate()
    {
        float x = speed * Mathf.Cos(direction);
        float y = speed * Mathf.Sin(direction);
        transform.Translate(x, y, 0);
    }

    void setSpeed(float s)
    {
        speed = s;
    }
}