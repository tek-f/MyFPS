using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    public int teamAmount = 2;

    public List<Team> teams;

    protected void Start()
    {
        Debug.Log("Setting up game...");
        SetUpGame();
    }
    public void SetUpGame()
    {
        for (int teamID = 1; teamID < teamAmount; teamID++)
        {
            teams.Add(new Team(teamID));
        }
    }

    public void AddScore(int teamID, int value)
    {
        foreach (Team team in teams)
        {
            if(team.teamID == teamID)
            {
                teams[teamID].score += value;
                return;
            }
        }
    }
}

[System.Serializable]
public class Team
{
    public int score;
    public int teamID;
    
    public Team(int teamID)
    {
        this.teamID = teamID;
    }
}
