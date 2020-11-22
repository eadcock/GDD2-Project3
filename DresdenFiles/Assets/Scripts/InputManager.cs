using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public enum States
    {
        gameState,
        pauseState,
        endState
    }
    States currentState;

    public static bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        currentState = States.gameState;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case States.gameState:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ChangeState(1);
                }
                break;
            case States.pauseState:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ChangeState(0);
                }
                break;
        }
    }

    public void ChangeState(int newState)
    {
        currentState = (States)newState;
        if (newState < 1)
        {
            // play
            isPaused = false;
            Time.timeScale = 1;
        }
        else if (newState < 2)
        {
            // pause
            isPaused = true;
            Time.timeScale = 0;
        }
        else
        {
            // end
            QuitGame();
        }
    }

    private void OnGUI()
    {
        if (isPaused)
        {
            GUI.Box(new Rect(40, 40, Screen.width - 80, Screen.height - 80), "PAUSED");
            if (GUI.Button(new Rect((Screen.width / 2) - 30, (Screen.height / 2) - 10, 60, 30), "Resume"))
                ChangeState(0);
            if (GUI.Button(new Rect((Screen.width / 2) - 30, (Screen.height / 2) + 30, 60, 30), "Quit"))
                ChangeState(2);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}