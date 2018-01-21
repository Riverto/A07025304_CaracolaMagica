using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour {
    //class attributes
    //menuHint will tell the user that they can type menu whenever they want
    //to return to the main menu.
    const string menuHint = "Puedes escribir 'menu' en cualquier \n momento para regresar";

    //answerHint will tell the user the posible answers
    const string answerHint = "Recuerda que solo puedo responder 'Si, 'No' y 'Talvez'";

    //askQuestion will tell the user to input his question
    const string askQuestion = "Cual es tu pregunta?";

    //startHint will tell the user how to start the game
    const string startHint = "Para hacer preguntas escribe 'empezar'.";

    //againHint will tell the user how to ask another question.
    const string againHint = "Para hacer otra pregunta escribe 'otra'";


    //This Array contains the answers that we print
    string[] answers = { "Si", "No", "Talvez" };

    //This variable stores the number of questions we have been asked
    int timesAsked;

    //This variable will be used for our EE
    bool hasDisappeard;

    //This variable helps us know if this is the first time on the menu
    bool seenMenu;

    //Here I declare an enumerated type to represent the different game
    //states, and I declare a variable to hold the current game state
    enum GameState { MainMenu, Question, Answered, EasterEgg };
    GameState currentState;

    // Use this for initialization
    void Start () {
        timesAsked = 0;
        hasDisappeard = false;
        seenMenu = false;
        Debug.Log(Environment.UserName);
        ShowMainMenu();
	}

    private void ShowMainMenu()
    {
        currentState = GameState.MainMenu;
        Terminal.ClearScreen();
        if (!seenMenu)
        {
            Terminal.WriteLine("Haz encontrado la gran caracola magica.");
            seenMenu = true;
        }
        Terminal.WriteLine(startHint);
        Terminal.WriteLine(menuHint);
    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnUserInput(string input)
    {
        //Because we want the EasterEgg to be accesible from anywhere
        //We check its activation before anything
        if(timesAsked > 10 && !hasDisappeard)
        {
            hasDisappeard = true;
            currentState = GameState.EasterEgg;
            Terminal.WriteLine("Haces demasiadas preguntas "+ Environment.UserName+".");
            Terminal.WriteLine("La caracola magica desaparece.");
        }
        // If user inputs the "menu" keyword, then we call the
        // ShowMainMenu() method once more
        else if (currentState == GameState.Answered)
        {//if the current state is somewhere we can only use the menu command
            if (input == "menu")//You can only go back to menu if you 
            {
                ShowMainMenu();
            }
            //in the case that the player wants to ask again
            if(input == "otra")
            {
                AskForQuestion();
            }
        }
        else if (input == "menu")
        {
            ShowMainMenu();
        }
        // However, if the user types quit, close, exit, then we try to close
        // our game. If the game is played on a Web browser, then we ask the
        // user to close the browser's tab
        else if (input == "quit" || input == "close" || input == "exit")
        {
            Terminal.WriteLine("Please, close the browser's tab");
            Application.Quit();
        }
        // If the user inputs anything that is not menu, quit, close or exit,
        // then we are going to handle that input depending on the game state.
        // If the game state is still MainMenu, then we call the RunMainMenu()
        // method.
        else if (currentState == GameState.MainMenu)
        {
            RunMainMenu(input);
        }
        else if (currentState == GameState.Question)
        {
            if (!hasDisappeard)
            {
                timesAsked++;
                giveRandomAnswer();
                Terminal.WriteLine(againHint);
            } else
            {
                Terminal.WriteLine("No hay nada para escucharte.");
            }
        }
    }

    private void giveRandomAnswer()
    {//Returns a random answer from our array
        currentState = GameState.Answered;
        Terminal.WriteLine("La caracola contesta: " + answers[UnityEngine.Random.Range(0, answers.Length)]);
    }

    private void AskForQuestion()
    {
        currentState = GameState.Question;
        Terminal.ClearScreen();
        Terminal.WriteLine(askQuestion);
        Terminal.WriteLine(answerHint);
        Terminal.WriteLine(menuHint);
    }

    void RunMainMenu(string input)
    {
        input = input.ToLower();
        // We fisrt check that the input is a valid input
        bool isValidInput = (input == "empezar");

        // If the user inputs a valid level, we convert that input to an int
        // value and then we call the AskForPassword() method.
        if (isValidInput)
        {
            AskForQuestion();
        }
        
        //EasterEgg debug command
        else if(input == "trigger")
        {
            hasDisappeard = true;
            Terminal.ClearScreen();
            Terminal.WriteLine("EasterEgg triggered, type 'menu'");
        }

        // If not, then we just ask them to enter a valid level.
        else
        {
            Terminal.WriteLine(startHint);
        }

    }
}