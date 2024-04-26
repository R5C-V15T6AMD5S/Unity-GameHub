using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownController : MonoBehaviour
{
   public int countdownTime; // vrijeme
   public Text countdownDisplay; // referenca na CountdownText objekt u Unity-u

   void Start() {
       StartCoroutine(CountdownToStart()); // Pokretanje odbrojavanja čim se igra pokrene
   }

   // koristimo korutinu za odbrojavanje. Koristi se korutina zbog njezinog svojstva da se može obustavljati
    IEnumerator CountdownToStart() {
       // Odbrojavanje dok vrijeme nije 0
       while(countdownTime > 0) {
           countdownDisplay.text = countdownTime.ToString(); // Pretvaranje trenutnog broja u string

           yield return new WaitForSeconds(1f); // Prekid korutine za 1 sekundu

           countdownTime--; // Dekrementiranje vremena nakon svake sekunde
       }
       
       countdownDisplay.text = "Crtaj!"; // Poruka igraču da igra može započeti
       yield return new WaitForSeconds(1f); // čekamo još 1 sekundu
       countdownDisplay.gameObject.SetActive(false); // micanje poruke sa ekrana;
   }
}