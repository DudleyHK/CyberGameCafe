﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnparentCamera : MonoBehaviour {

    private GameObject camera;
    private GameObject player;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        if (camera.transform.parent != null)
        {
            camera.transform.parent = null;
            camera.transform.Translate(10, 0, 0);

            player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Movement>().newPhishingQuestSpeed();
        }
    }
}
