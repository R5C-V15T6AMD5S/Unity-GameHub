using UnityEngine;

public static class MyNoise
{
    // Ova klasa sadrži statične metode za generaciju i manipulaciju šumom
    public static float RemapValue(float value, float initialMin, float initialMax, float outputMin, float outputMax)
    {
        // Ova metoda omogućava preslikavanje vrijednosti iz određenog raspona u neki drugi raspon. 

        return outputMin + (value - initialMin) * (outputMax - outputMin) / (initialMax - initialMin);
    }

    public static float RemapValue01(float value, float outputMin, float outputMax)
    {
        // Ova metoda omogućava preslikavanje vrijednosti iz raspona 0 - 1 u neki drugi raspon.

        return outputMin + (value - 0) * (outputMax - outputMin) / (1 - 0);
    }

    public static int RemapValue01ToInt(float value, float outputMin, float outputMax)
    {
        // Konvertira preslikanu vrijednost na integer vrijednost

        return (int)RemapValue01(value, outputMin, outputMax);
    }

    public static float Redistribution(float noise, NoiseSettings settings)
    {
        // Mijenja vrijednost šuma za stvaranje pojedinih efekata (naglašava ili smanjuje određene značajke, npr. planine)

        return Mathf.Pow(noise * settings.redistributionModifier, settings.exponent);
    }

    public static float OctavePerlin(float x, float z, NoiseSettings settings)
    {
        // Ova metoda izračunava Octave Perlin šum. Parametri x i z su koordinate za točku, settings parametar će prilagoditi izračun šuma

        x *= settings.noiseZoom;
        z *= settings.noiseZoom;

        // Osiguravanje da neće biti integer vrijednost
        x += settings.noiseZoom;
        z += settings.noiseZoom;

        // Ukupna količina šuma
        float total = 0;

        float frequency = 1;
        float amplitude = 1;

        // Osigurava normaliziranje rezultata unutar raspona 0 - 1
        float amplitudeSum = 0;

        for (int i = 0; i < settings.octaves; i++)
        {
            // Iterira se za pojedinu oktavu te se na taj način raslojava svijet

            total += Mathf.PerlinNoise((settings.offset.x + settings.worldOffset.x + x) * frequency,
            (settings.offset.y + settings.worldOffset.y + z) * frequency) * amplitude;

            amplitudeSum += amplitude;

            amplitude *= settings.persistance;
            frequency *= 2;
        }

        return total / amplitudeSum;
    }
}
