using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerInputTests
{
    private GameObject playerObject;
    private PlayerInput playerInput;

    [SetUp]
    public void SetUp()
    {
        // Kreiraj GameObject i dodaj PlayerInput komponentu
        playerObject = new GameObject();
        playerInput = playerObject.AddComponent<PlayerInput>();
    }

    [TearDown]
    public void TearDown()
    {
        // Uništi GameObject nakon svakog testa
        GameObject.Destroy(playerObject);
    }

    [UnityTest]
    public IEnumerator TestMovementInput()
    {
        // Simuliraj pritisak tipki W i D (kretanje prema naprijed-desno)
        playerInput.GetType().GetMethod("GetMovementInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(playerInput, null);
        yield return null;

        // Za ovaj test, treba postaviti oèekivani ulaz u skladu sa simulacijom
        Assert.AreEqual(new Vector3(0, 0, 0), playerInput.MovementInput); // Promeni prema stvarnom oèekivanju
    }

    [UnityTest]
    public IEnumerator TestMousePosition()
    {
        // Simuliraj pomicanje miša
        playerInput.GetType().GetMethod("GetMousePosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(playerInput, null);
        yield return null;

        Assert.AreEqual(new Vector2(0, 0), playerInput.MousePosition); // Promeni prema stvarnom oèekivanju
    }


}
