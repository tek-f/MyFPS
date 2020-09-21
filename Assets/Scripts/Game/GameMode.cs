using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    public int teamAmount = 2;//the number of teams in a game
    public List<Team> teams;//a list of Team classes that hold info for each time in the game
    public int gameScoreLimit;//the score that a team must reach to win the game

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
    public void EndGame()
    {

    }
    protected void Start()
    {
        Debug.Log("Setting up game...");
        SetUpGame();
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
