using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameLauncher : MonoBehaviour
{
    // Buttons that user clicks to play a certain game
    public Button sketchGuessButton;
    public Button minecraftSimButton;
    public Button vampireSurvivalButton;


    /* 
        Base path to the directory containing your executables
        Then deeclaring each game's path separately
    */
    public string basePath = "C:/path/to/your/games/";
    public string sketchGuessExe = "SketchGuess.exe";
    public string minecraftSimExe = "MinecraftSim.exe";
    public string vampireSurvivalExe = "VampireSurvival.exe";

    void Start() {
        sketchGuessButton.onClick.AddListener(() => OpenGame(sketchGuessExe));
        minecraftSimButton.onClick.AddListener(() => OpenGame(minecraftSimExe));
        vampireSurvivalButton.onClick.AddListener(() => OpenGame(vampireSurvivalExe));
    }

   void OpenGame(string gameExe) {
        string fullPath = Path.Combine(basePath, gameExe);
        Process.Start(fullPath);
    }
}
