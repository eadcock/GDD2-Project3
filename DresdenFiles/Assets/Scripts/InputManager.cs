using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using quiet;
using System.Linq;

public class InputManager : MonoBehaviour
{
    public enum States
    {
        gameState,
        pauseState,
        endState
    }
    States currentState;

    //public GameObject endPanel;
    public GameObject pausePanel;

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
        //Debug.Log(newState);
        if (newState > 2)
        {
            return;
        }

        currentState = (States)newState;
        if (newState < 1)
        {
            // play
            isPaused = false;
            Time.timeScale = 1;
            pausePanel.SetActive(false);
        }
        else if (newState < 2)
        {
            // pause
            isPaused = true;
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }
        else
        {
            // end

        }
        //pausePanel.SetActive(currentState == States.pauseState);
        //endPanel.SetActive(currentState == States.endState);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}