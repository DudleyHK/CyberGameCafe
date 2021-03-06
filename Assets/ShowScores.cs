﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowScores : MonoBehaviour
{
    int totalScore;

    public int getScore()
    {
        return totalScore;
    }

    // Use this for initialization
    void Start()
    {
        totalScore = 0;

        GameObject scorer = GameObject.Find("EternalObject");
        if (scorer)
        {
            EternalScript s = scorer.GetComponent<EternalScript>();
            ConvertScore c = scorer.GetComponent<ConvertScore>();

            int[] score = new int[6];
            score[0] = (s.phishingScore == -1) ? 0 : c.getRealScore(s.phishingScore, 0, s.phishingMax);
            score[1] = (s.passwordScore == -1) ? 0 : c.getRealScore(s.passwordScore, 0, 9, true);
            score[2] = (s.encryptionScore == -1) ? 0 : c.getRealScore(s.encryptionScore, 15, 160, true);
            score[3] = (s.sharingScore == -1) ? 0 : c.getRealScore(s.sharingScore, 0, 5, true);
            score[4] = (s.publicPCScore == -1) ? 0 : c.getRealScore(s.publicPCScore, 40, 120, true);
            score[5] = (s.USBScore == -1) ? 0 : c.getRealScore(s.USBScore, 15, 90);

            for (int i = 0; i < 6; i++)
            {
                totalScore += score[i];
            }

            if (gameObject.name != "PauseScores")
            {
                GetComponent<Text>().text =
                    "Congratulations you beat the game!";
            }

            GetComponent<Text>().text += "\nPhishing Spam Score: " + score[0] +
                "\nSecure Password Score: " + score[1] +
                "\nEncryption Score: " + score[2] +
                "\nPassword Sharing Score: " + score[3] +
                "\nPublic PCs Score: " + score[4] +
                "\nUSB Score: " + score[5] +
                "\n\nTotal Score: " + totalScore;

            if (gameObject.name != "PauseScores")
            {
                GetComponent<Text>().text += "\n\nPress select to return to the main menu";
            }
        }
    }
}