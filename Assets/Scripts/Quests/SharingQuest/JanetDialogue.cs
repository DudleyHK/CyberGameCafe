﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JanetDialogue : MonoBehaviour
{
	private Mission thisQuest;
	private GameObject player;
	private bool playerInBox;
	private bool interacting;

	GameObject textBox;

	void Start()
	{
	textBox = GameObject.FindGameObjectWithTag ("TextBox");
	}
		
	void Awake()
	{
		thisQuest = GetComponentInParent<Mission> ();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		player = GameObject.FindGameObjectWithTag("Player");
		if (col.gameObject == player)
		{
			playerInBox = true;
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		player = GameObject.FindGameObjectWithTag("Player");
		if (col.gameObject == player)
		{
			playerInBox = false;
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.E) && playerInBox)
		{
			dialogue ();
		}
	}

	void dialogue()
	{
		GameObject textBox = GameObject.FindGameObjectWithTag ("TextBox");

		DialogueMessages d = textBox.GetComponent<DialogueMessages> ();
		if (thisQuest.getActiveObjective () == null) {
			d.spawnTextBox ("Oh my goodness, this morning I woke up to find that someone had posted to my Facebook without my permission", "Janet");
			d.spawnTextBox ("At first I thought it could only have been Marlon, as he was the only person I'd ever given my password to", "Janet");
			d.spawnTextBox ("But then I found out that Marlon gave it to Tito and Michael, and Michael gave it to Jackie and Jermaine...", "Janet");
			d.spawnTextBox ("I gathered them all together in this room, and I know for sure that one of them is guilty and the other four are innocent", "Janet");
			d.spawnTextBox ("The problem is that every time one of them speaks they always say one true statement and one false statement, and I never know which is which", "Janet");
			d.spawnTextBox ("Please could you try speaking to them and see if you can figure out which of them did it?", "Janet");

			player.GetComponent<QuestSystem> ().assignMission (thisQuest, gameObject);
			thisQuest.startMission ();
		} else {
			string thisTag = thisQuest.getActiveObjective ().objectiveTag;
			if (thisTag == "questionSusps") {
				d.spawnTextBox ("You should question all five of them before you make an accusation. Remember, I'm certain that:"
					+ "\nOnly one of them is guilty\nand\nEach of them is telling one truthful statement and one lie", "Janet");
			} else if (thisTag == "jaccuse") {
				d.spawnTextBox ("So who do you think it was?", "Janet");

				d.switchButton (false);
				textBox.transform.GetChild (2).gameObject.SetActive (true);
			}
		}
	}
}