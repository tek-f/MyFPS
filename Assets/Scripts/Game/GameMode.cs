using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyFPS.Player;

namespace MyFPS.GameAdmin
{
    public class GameMode : MonoBehaviour
    {
        /// <summary>
        /// Number of teams in the game.
        /// </summary>
        public int teamAmount = 2;
        /// <summary>
        /// Used in AddScore() checks to determine game type, i.e. CTF (Capture the flag)
        /// </summary>
        public string gameType;
        /// <summary>
        /// List of Team classes that hold info for each time in the game
        /// </summary>
        public List<Team> teams;
        /// <summary>
        /// List of PlayerHandlers within the game scene.
        /// </summary>
        public List<PlayerHandler> playersList = new List<PlayerHandler>();
        /// <summary>
        /// the score that a team must reach to trigger EndGame() to run.
        /// </summary>
        public int gameScoreLimit = 5;
        /// <summary>
        /// Set Up function for the GameMode.
        /// </summary>
        public void SetUpGame()
        {
            for (int teamID = 0; teamID < teamAmount; teamID++)
            {
                teams.Add(new Team(teamID));
            }
        }
        /// <summary>
        /// Increases a Teams score, depending on _teamID, by _value.
        /// </summary>
        /// <param name="_teamID">The teamID of the team that has scored.</param>
        /// <param name="_value">The value that the teams score is being increased by.</param>
        public virtual void AddScore(int _teamID, int _value)
        {
            teams[_teamID].score += _value;
            foreach (PlayerHandler player in playersList)
            {
                player.UpdateTeamScores(teams[0].score, teams[1].score);
            }
            if (teams[_teamID].score >= gameScoreLimit)
            {
                EndGame();
            }
        }
        /// <summary>
        /// Is called by AddScore() when gameScoreLimit is reached by one of the teams scores.
        /// </summary>
        public virtual void EndGame()
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            foreach (PlayerHandler player in playersList)
            {
                player.GetComponent<FirstPersonController>().enabled = false;
                player.GetComponent<PlayerHandler>().endGamePanel.SetActive(true);

            }
        }
        protected virtual void Start()
        {
            Debug.Log("Setting up game...");
            SetUpGame();
        }
    }

    [System.Serializable]
    public class Team
    {
        /// <summary>
        /// Represents the teams score.
        /// </summary>
        public int score;
        /// <summary>
        /// ID of the team, starting from 0.
        /// </summary>
        public int teamID;
        public Team(int teamID)
        {
            this.teamID = teamID;
        }
    }
}