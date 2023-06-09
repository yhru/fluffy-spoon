using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SocialManager : MonoBehaviour
{
    private string screenshotFilename = "screenshot.png";

    private void Start() { }

    public void SocialShare()
    {
        StartCoroutine(TakeScreenshot());
    }

    public IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame();

        // Get main camera + Create and set RenderTexture
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        Camera mainCamera = Camera.main;
        RenderTexture currentRT = mainCamera.targetTexture;
        mainCamera.targetTexture = renderTexture;

        // Capture the screenshot
        Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        mainCamera.Render();
        RenderTexture.active = renderTexture;
        screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshotTexture.Apply();

        // Set back actual RenderTexture of main camera + Deactivate RenderTexture
        mainCamera.targetTexture = currentRT;
        RenderTexture.active = null;

        // Convert the screenshot to bytes
        byte[] screenshotBytes = screenshotTexture.EncodeToPNG();

        // Save the screenshot to persistent data path
        string screenshotPath = System.IO.Path.Combine(Application.persistentDataPath, screenshotFilename);
        System.IO.File.WriteAllBytes(screenshotPath, screenshotBytes);

        // Clean memory
        Destroy(renderTexture);
        Destroy(screenshotTexture);

        // Share the screenshot using native sharesheet
        new NativeShare().AddFile(screenshotPath)
            .SetSubject("Check out Cerealis!")
            .SetText("#cerealis #coloring #AR")
            .Share();
    }
}
