﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButtons : MonoBehaviour
{
    public GameObject objectiveUI;
    public GameObject questStatusUI;

	void Awake()
	{
		if (name == "Resume")
		{
			GetComponent<Button> ().onClick.AddListener (resume);
		}
		else
		{
			GetComponent<Button> ().onClick.AddListener (quit);
		}
	}

	void resume()
	{
		GameObject.FindGameObjectWithTag ("Player").
		GetComponent<Movement>().startMovement();

        objectiveUI.SetActive(true);

        if (questStatusUI)
        {
            questStatusUI.SetActive(false);
        }

        transform.parent.gameObject.SetActive (false);
	}

	void quit()
	{
		//GameObject.FindGameObjectWithTag ("GameController").
		//GetComponent<LoadGame>().writeToTextFile();
		Application.LoadLevel ("MainMenu");
	}
}
