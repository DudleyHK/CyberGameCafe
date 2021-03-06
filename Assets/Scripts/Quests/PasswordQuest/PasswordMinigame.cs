﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Set of password constants that the game will choose from using RNG as the selection for the users passwords.
// Bad and good phrases should be included in here.
// When password options are available we shopuld also be able to choose one of the previous passwords we created. (Doing so causes the quest to fail.)

public enum PassphraseType
{
    TYPE_NULL = 0,
    TYPE_NAME,
    TYPE_COMMONPASSWORD,
    TYPE_STRONG,
    TYPE_PASSWORD,
    TYPE_SHORT,
    TYPE_REPETATIVE
}

public struct passphrase
{
    public string phrase;
    public PassphraseType phraseType;

    public passphrase(string newPhrase, PassphraseType wordType)
    {
        phrase = newPhrase;
        phraseType = wordType;
    }
}

public class PasswordPhrases
{
    // TODO - Add some passphrases based on the criteria on the class comment. 
    public static passphrase[] passphrases =
    {
        new passphrase("Password",PassphraseType.TYPE_PASSWORD),

        // passwords with names
        new passphrase("Peter098", PassphraseType.TYPE_NAME),
        new passphrase("JamesFoot76", PassphraseType.TYPE_NAME),
        new passphrase("micheal", PassphraseType.TYPE_NAME),
        new passphrase("Tommy765", PassphraseType.TYPE_NAME),

        // common passwords
        new passphrase("Welc0me", PassphraseType.TYPE_COMMONPASSWORD),
        new passphrase("football", PassphraseType.TYPE_COMMONPASSWORD),
        new passphrase("monkey", PassphraseType.TYPE_COMMONPASSWORD),
        new passphrase("football", PassphraseType.TYPE_COMMONPASSWORD),

        // short passwords
        new passphrase("abc123", PassphraseType.TYPE_SHORT),
        new passphrase("adobe123", PassphraseType.TYPE_SHORT),
        new passphrase("123", PassphraseType.TYPE_SHORT),
        new passphrase("letmein", PassphraseType.TYPE_SHORT),

        // repeative passwords
        new passphrase("gamesgames77",PassphraseType.TYPE_REPETATIVE),
        new passphrase("123321", PassphraseType.TYPE_REPETATIVE),

        // good passwords
        new passphrase("theKingdom5areVeryG00dT0day!", PassphraseType.TYPE_STRONG),
        new passphrase("It'sABeauti4lDayT0G0Walking", PassphraseType.TYPE_STRONG),
        new passphrase("24cc78a7f6ff3546e", PassphraseType.TYPE_STRONG)
    };
}

public class PasswordMinigame : MonoBehaviour {

    [SerializeField]
    GameObject selecter;

    public GameObject textButtonToSpawn;
    public GameObject notificationWindow;
    public const int numberOfPassphraseOptions = 4;

	private passphrase thePassword;
    passphrase[] passwordPhrases;
    int passwordSelection = 0;

    private string passwordHints = "\0";

    private GameObject passwordMiniGameCollider;

	void Start()
	{
		
	}

    void Awake()
    {
        GameObject buttonToSpawn;
        passphrase phraseToSet;
        passphrase[] passwordPhrases = new passphrase[numberOfPassphraseOptions];
        setPasswordPhrases(passwordPhrases);

        GameObject x = null;
        for(int i = 0; i < numberOfPassphraseOptions; i++)
        {
            buttonToSpawn = Instantiate(textButtonToSpawn, gameObject.transform.GetChild(2));
            if(i == 0)
            {
                x = buttonToSpawn;
            }
            phraseToSet = passwordPhrases[i]; // TODO - make this a function to ensure the phrase choices are diverse.
            buttonToSpawn.GetComponent<Text>().text = phraseToSet.phrase;
            buttonToSpawn.GetComponent<TextButton>().setPassphrase(phraseToSet);
            buttonToSpawn.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-300 + (200 * i), 10, 0);
        }
        if (x)
        {
            GameObject s = Instantiate(selecter, x.transform);
        }
    }
	
    public void addPassphraseToPassword(passphrase thePhrase)
    {
        GetComponentInChildren<InputField>().text = thePhrase.phrase;
        thePassword = thePhrase;
    }

    // Here we check the password and score the users efforts.
    // If its good we can destroy ourselves and allow the user to move again as well as allow the game to continue.
    // else we display an error message and some advice.
    public void evaluatePassword()
    {
        GameObject windowToSpawn;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        // TODO - evaluate password score and then rispond accordigly.
        if (checkPasswordStrength() != PassphraseType.TYPE_STRONG)
        {
            windowToSpawn = Instantiate(notificationWindow, gameObject.transform);
             GameObject.FindGameObjectWithTag("UIMinigameErrorWindow")
                 .GetComponentInChildren<Text>().text = passwordHints;
        }
        else
        {
            player.GetComponent<QuestSystem>()
                .updateMissionState(MissionObjectiveTypes.OBJ_EVENT, "passwordCreate");
            player.GetComponent<Movement>().startMovement();
            passwordMiniGameCollider.GetComponent<PasswordMinigameWindow>().isGameComplete(true);
            Destroy(gameObject);

            GameObject scorer = GameObject.Find("EternalObject");
            if (scorer)
            {
                EternalScript e = scorer.GetComponent<EternalScript>();
                e.passwordScore = GameObject.Find("SecurePasswordQuest").
                    GetComponent<SecurePasswordScore>().getScore();
                e.passwords += 1;
            }
        }
    }

    PassphraseType checkPasswordStrength()
    {
        string helpMessage = "";
        switch (thePassword.phraseType)
        {
            case PassphraseType.TYPE_NAME:
                helpMessage = "You shouldnt use names. \n Those passwords can be guessed easily.";
                break;
            case PassphraseType.TYPE_PASSWORD:
                helpMessage = "NEVER EVER EVER \n USE 'PASSWORD' AS YOUR PASSWORD!";
                break;
            case PassphraseType.TYPE_COMMONPASSWORD:
                helpMessage = "Common words can be easily guessed by password crackers. \n Consider using something else.";
                break;
            case PassphraseType.TYPE_SHORT:
                helpMessage = "The password is quite short. You should choose a password that quite long and contains a few charaters.";
                break;
            case PassphraseType.TYPE_REPETATIVE:
                helpMessage = "The password is a little repetitive which makes it easy to work out the password. \n" +
                    "Try to create a password that cosent contain similar patterns of words or characters.";
                break;
           default:
                helpMessage = "You need to set a password for this system!";
                break;
        }
            
        if(helpMessage != "")
        {
            updatePasswordHints(helpMessage);
        }

        return thePassword.phraseType;
    }

    void updatePasswordHints(string hintToAdd)
    {
        passwordHints = hintToAdd;
    }


    void setPasswordPhrases(passphrase[] phraseBuffer)
    {
		for(int i = 0; i < phraseBuffer.Length; i++)
		{
			// phraseHeap = Random.Range(1,4);
			phraseBuffer[i] =  PasswordPhrases.passphrases[Random.Range(0, 14)];

			if (i > 0) 
			{
				for (int j = 0; j < i; j++) 
				{
					if (phraseBuffer [j].phrase == phraseBuffer [i].phrase) 
					{
						phraseBuffer[i] =  PasswordPhrases.passphrases[Random.Range(0, 14)];
						j = 0;
					}	
				}
			}
		}

		phraseBuffer[Random.Range(0, phraseBuffer.Length)] = PasswordPhrases.passphrases[Random.Range(15,(PasswordPhrases.passphrases.Length - 1))];
    }


	void addPassphrases()
	{
	}

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {

#if UNITY_EDITOR // Only execute the code below if we are testing.
            GameObject.FindGameObjectWithTag("Player").GetComponent<QuestSystem>()
    .updateMissionState(MissionObjectiveTypes.OBJ_EVENT, "passwordCreate");
            passwordMiniGameCollider.GetComponent<PasswordMinigameWindow>().isGameComplete(true);
#endif
            Destroy(gameObject);
        }
    }

    public void setPasswordMiniGameCollider(GameObject theCollider)
    {
        passwordMiniGameCollider = theCollider;
    }
}
