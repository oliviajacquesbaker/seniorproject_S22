using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _LightIntensity : MonoBehaviour
{
    [SerializeField]
    public RenderTexture lightCheckTexture;

    [SerializeField]
    public float defaultLightLevel = 0;
    public float LightLevel;

    [SerializeField]
    bool log = false;

    void Update()
    {
        CalcLightLevel();
        PlayerPassThrough();
    }

    private void PlayerPassThrough()
    {
        float perceivedLight = (LightLevel - defaultLightLevel) / 10000;

        _PlayerStatsController player = GameObject.FindGameObjectWithTag("Player").GetComponent<_PlayerStatsController>();
        player.UpdateHealth(perceivedLight); //Passes perceived light values to AI health controller.

        if (log)
        {
            Debug.Log((LightLevel - defaultLightLevel) / 10000);
        }
    }

    private void CalcLightLevel()
    {
        Color32[] rgb = textureRead();

        LightLevel = 0;

        for (int i = 0; i < rgb.Length; i++)
        {
            //Luminance!
            LightLevel += (0.2126f * rgb[i].r) + (0.7152f * rgb[i].g) + (0.0722f * rgb[i].b);
        }
    }

    //maps the rgb of our player to a colors array.
    private Color32[] textureRead()
    {
        RenderTexture tmpTexture = RenderTexture.GetTemporary(lightCheckTexture.width, lightCheckTexture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        Graphics.Blit(lightCheckTexture, tmpTexture);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = tmpTexture;


        Texture2D temp2DTexture = new Texture2D(lightCheckTexture.width, lightCheckTexture.height);
        temp2DTexture.ReadPixels(new Rect(0, 0, tmpTexture.width, tmpTexture.height), 0, 0);
        temp2DTexture.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(tmpTexture);

        Color32[] colors = temp2DTexture.GetPixels32();

        //BEWARE THE MEMORY LEAK! MAKE SURE THIS IS DESTROYED
        Destroy(temp2DTexture);

        return colors;
    }
}
