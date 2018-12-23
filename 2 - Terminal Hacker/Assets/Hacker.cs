using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacker : MonoBehaviour
{
    int CurrentLevel;
    enum Screen { Menu, Password, Win, Error };
    Screen CurrentScreen;

    void Start()
    {
        CurrentScreen = Screen.Menu;
        ShowMenu();
    }

    void OnUserInput(string input)
    {
        if (input == "menu")
        {
            CurrentScreen = Screen.Menu;
            Terminal.ClearScreen();
            ShowMenu();
        }
        else
        {
            switch (CurrentScreen)
            {
                case Screen.Menu:
                    ProcessMenuInput(input);
                    break;
                case Screen.Password:
                    ProcessPasswordInput(input);
                    break;
                case Screen.Win:
                    ProcessWinInput(input);
                    break;
                default:
                    CurrentScreen = Screen.Menu;
                    Terminal.ClearScreen();
                    ShowMenu();
                    break;
            }
        }
    }

    void ShowMenu()
    {
        Terminal.WriteLine("What would you like to hack into?");
        Terminal.WriteLine("Press 1 for the local library");
        Terminal.WriteLine("Press 2 for the police station");
        Terminal.WriteLine("Enter your selection:");
    }

    void ProcessMenuInput(string input)
    {
        if (input == "1")
        {
            CurrentLevel = 1;
            StartGame();
        }
        else if (input == "2")
        {
            CurrentLevel = 2;
            StartGame();
        }
        else if (input == "007")
        {
            Terminal.WriteLine("Please select a level Mr Bond!");
        }
        else
        {
            Terminal.WriteLine("Please choose a valid level");
        }
    }

    void StartGame()
    {
        CurrentScreen = Screen.Password;
        Terminal.WriteLine("You have chosen level " + CurrentLevel);
        Terminal.WriteLine("Please enter your password: ");
    }
}