using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Klasa koja služi za prikazivanje i upravljanjem health barom igraèa
public class HealtBar : MonoBehaviour
{
    // Definiranje reference klizaèa
    public Slider slider;
    // Definiranje gradijenta boje
    public Gradient gradient;
    // Definiranje reference na sliku koja prikazuje koliko igraè ima health-a
    public Image fill;

    // Postavljanje poèetnog zdravlja igraèa
    private void Awake()
    {
        SetMaxHealth(100);
    }

    // Pretplata na dogaðaj promjene zdravlja igraèa
    private void OnEnable()
    {
        Player.OnHealthChanged += SetHealth;
    }

    // Odjava s dogaðaja promjene zdravlja igraèa
    private void OnDisable()
    {
        Player.OnHealthChanged -= SetHealth;
    }

    // Metoda za postavljanje maksimalnog zdravlja.
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    // Metoda za postavljanje trenutnog zdravlja.
    public void SetHealth(int health)
    {
        // Postavljanje trenutne vrijednosti slidera na zadanu vrijednost zdravlja
        slider.value = health;
        // Postavljanje boje punjenja trake na boju na temelju normalizirane vrijednosti slidera
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
