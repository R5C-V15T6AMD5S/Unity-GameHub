using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/*
    This script is used for those who have Linux
    Means its Linux Distro script ONLY
*/
public class GameLauncherLinux : MonoBehaviour
{
    // Buttons that user clicks to play a certain game
    public Button sketchGuessButton;
    public Button minecraftSimButton;
    public Button vampireSurvivalButton;

     // Base path to the directory containing your executables
    public string basePath = "/home/user/path/to/your/games/";

    // Names of the executables
    public string sketchGuessExe = "SketchGuess";
    public string minecraftSimExe = "MinecraftSim";
    public string vampireSurvivalExe = "VampireSurvival";

    void Start() {
        sketchGuessButton.onClick.AddListener(() => OpenGame(sketchGuessExe));
        minecraftSimButton.onClick.AddListener(() => OpenGame(minecraftSimExe));
        vampireSurvivalButton.onClick.AddListener(() => OpenGame(vampireSurvivalExe));
    }

    void OpenGame(string gameExe) {
        string fullPath = Path.Combine(basePath, gameExe);

        // Ensure executable permissions
        if (!File.Exists(fullPath)) {
            UnityEngine.Debug.LogError($"Executable not found: {fullPath}"); return;
        }

        var processInfo = new ProcessStartInfo {
            FileName = fullPath,
            UseShellExecute = true,
            RedirectStandardOutput = false,
            RedirectStandardError = false,
            CreateNoWindow = true
        }; Process.Start(processInfo);
    }
}
