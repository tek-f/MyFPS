using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyFPS.GameAdmin
{
    public class GameMode : MonoBehaviour
    {
        public int teamAmount = 2;//the number of teams in a game
        public List<Team> teams;//a list of Team classes that hold info for each time in the game
        public int gameScoreLimit;//the score that a team must reach to win the game
        [SerializeField] protected GameObject endGamePanel;

        [SerializeField] Text team1ScoreText, team2ScoreText;
        public void SetUpGame()
        {
            for (int teamID = 1; teamID < teamAmount; teamID++)
            {
                teams.Add(new Team(teamID));
            }
        }
        public virtual void AddScore(int teamID, int value)
        {
            teams[teamID - 1].score += value;
            if (teamID == 1)
            {
                team1ScoreText.text = teams[0].score.ToString();
            }
            else
            {
                team2ScoreText.text = teams[1].score.ToString();
            }
            if (teams[teamID - 1].score >= gameScoreLimit)
            {
                EndGame();
            }
        }
        public virtual void EndGame()
        {
            Time.timeScale = 0;
            endGamePanel.SetActive(true);
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
}