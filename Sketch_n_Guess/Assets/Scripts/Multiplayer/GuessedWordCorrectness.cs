using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuessedWordCorrectness : MonoBehaviour
{
    // GameObjects used to render and proces the word
    public TMP_Text displayText;

    // Private strings to check the correctness of it
    private string wordToGuess = "doom";

    void Start() {
        StringBuilder txtBuilder = new StringBuilder();
        for (int i = 0; i < wordToGuess.Length; i++) {
            txtBuilder.Append("_ ");
        } 

        displayText.text = txtBuilder.ToString();
    }

    // Called when the user submits their guess
    public void CheckWordCorrectnessOnSubmit(InputField inputMessage) {
        string guess = inputMessage.text.ToLower();

        // Check if the guess length is less than or equal to the wordToGuess length
        if(guess.Length <= wordToGuess.Length) {
            StringBuilder updatedText = new StringBuilder(displayText.text);

            for (int i = 0; i < guess.Length; i++) {
                if(wordToGuess[i] == guess[i]) {
                    // Replace the underscore in the displayed text with the guessed character
                    updatedText[i * 2] = wordToGuess[i];
                }
            }

            displayText.text = updatedText.ToString();
        }
    }
}