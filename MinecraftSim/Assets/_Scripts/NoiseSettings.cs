using UnityEngine;

[CreateAssetMenu(fileName = "noiseSettings", menuName = "Data/NoiseSettings")]
public class NoiseSettings : ScriptableObject
{
    // NoiseSettings enkapsulira postavke za generaciju proceduralnog šuma

    // Definira skalu šuma, omogućava više ili manje visoravni
    public float noiseZoom;

    public int octaves;
    public Vector2Int offset;

    // worldOffset je jednak mapSeedOffset-u
    public Vector2Int worldOffset;
    public float persistance;
    public float redistributionModifier;
    public float exponent;
}
