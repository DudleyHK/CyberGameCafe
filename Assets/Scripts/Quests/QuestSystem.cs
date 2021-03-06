﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{
    [SerializeField]
    private List<Mission> currentMissions;
    private int activeMission = 0;

	public bool missionStarted(Mission m)
	{
		return currentMissions.Contains (m);
	}

    public Mission getActiveMission()
    {
        return currentMissions.Count > 0 ? currentMissions[activeMission] : null;
    }

    // utility functions.
    void setActiveMission(int newActiveMission)
    {
        // sets the new mission from the current missions list.
        if(newActiveMission < currentMissions.Count && newActiveMission > 0)
        {
            activeMission = newActiveMission;
        }
    }

    // hands in the chosen mission by the player. 
    void handInMission(int missionToHandIn)
    {
        // TODO - add more code to check and make sure that the mission is complete. 
        currentMissions.RemoveAt(missionToHandIn);

    }

	public void updateMissionState(MissionObjectiveTypes missionType, string missionTag, bool skip = false)
    {
        if(currentMissions.Count != 0)
        {
            for(int i = 0; i < currentMissions.Count; i++)
            {
                currentMissions[i].updateActiveMissionObjectives(missionType, missionTag, skip);

                if (currentMissions[i].isCompleated())
                {
                    handInMission(i);
                }
            }
        }
    }


    public void assignMission(Mission newMission, GameObject questGiver)
    {
		GameObject point = questGiver.transform.GetChild (0).gameObject;
		if (point.name == "QuestIndicator") {
			GameObject.Destroy (point);
		}
        if (currentMissions == null)
        {
            activeMission = 0;
            currentMissions = new List<Mission>();
        }

        newMission.startMission();
        currentMissions.Add(newMission);
    }
}
