using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuessedWordCorrectness : MonoBehaviour
{
    public TMP_Text displayText;

    // Private strings to check the correctness of it
    // LAter will be controlled by a script
    private string wordToGuess = "doom";

    /*
        Sets the string to undeerlines (len of word)
        Converts it to string to display it on TMP Label
    */
    void Start() {
        StringBuilder txtBuilder = new StringBuilder();
        for (int i = 0; i < wordToGuess.Length; i++) {
            txtBuilder.Append("_ ");
        } displayText.text = txtBuilder.ToString();
    }

    // Called when the user sens message (guess)
    public void CheckWordCorrectnessOnSubmit(InputField inputMessage) {
        string guess = inputMessage.text.ToLower();

        // Check if the guess length is less than or equal to the wordToGuess length
        if(guess.Length <= wordToGuess.Length) {
            StringBuilder updatedText = new StringBuilder(displayText.text);

            for (int i = 0; i < guess.Length; i++) {
                if(wordToGuess[i] == guess[i]) {
                    updatedText[i * 2] = wordToGuess[i]; // Replaces _ to correct letter if guessed
                }
            }

            displayText.text = updatedText.ToString().ToUpper();
        }
    }
}