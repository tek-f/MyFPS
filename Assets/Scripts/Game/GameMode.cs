using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyFPS.Player;
using Mirror;

namespace MyFPS.GameAdmin
{
    public class GameMode : NetworkBehaviour
    {
        public static GameMode instance = null;

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
        public List<Transform> spawnPoints = new List<Transform>();

        public PlayerHandler localPlayer;
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
        public virtual void AddScore(int _teamID)
        {
            Debug.Log(teams[_teamID]);
            teams[_teamID].score++;
            Debug.Log(localPlayer);
            localPlayer.UpdateTeamScores(teams[0].score, teams[1].score);
            if (teams[_teamID].score >= gameScoreLimit)
            {
                EndGame();
            }
        }
        [ClientRpc]
        public void RpcUnpdateScoreNetwork(int _teamIndex)
        {
            AddScore(_teamIndex);
        }
        public void UpdateScores(int _teamIndex)
        {
            teams[_teamIndex].score++;
            if (teams[_teamIndex].score >= gameScoreLimit)
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

            localPlayer.GetComponent<FirstPersonController>().enabled = false;
            localPlayer.GetComponent<PlayerHandler>().endGamePanel.SetActive(true);
            localPlayer.enabled = false;
        }
        public virtual void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else if(instance != this)
            {
                Destroy(gameObject);
                return;
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