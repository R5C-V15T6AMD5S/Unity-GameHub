using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealtBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    private bool isHealthSet = false;

    private void Awake()
    {
        SetMaxHealth(100);
    }

    private void OnEnable()
    {
        Player.OnHealthChanged += SetHealth;
    }

    private void OnDisable()
    {
        Player.OnHealthChanged -= SetHealth;
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
