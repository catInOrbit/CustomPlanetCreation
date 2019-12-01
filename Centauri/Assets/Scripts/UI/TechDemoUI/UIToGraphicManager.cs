using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToGraphicManager : MonoBehaviour
{
   

    public static string NormalPath {get; set;}
    public static string GreyscalePath {get; set;}

    private void Awake() 
    {
        
    }
    void Start()
    {
        NormalPath = Application.persistentDataPath + "/NormalMap.png";
        GreyscalePath = Application.persistentDataPath + "/grayscale.png";
    }

    /// <summary>
    /// Convert texture2D to normal map
    /// </summary>
    /// <param name="sourceImage">Texture2D captured from camera</param>
    /// <param name="strength">nomral bump strength</param>
   public void GetNormalMapFromTexture(Texture2D sourceImage, float strength)
    {

        if(NormalPath == null)
            NormalPath = Application.persistentDataPath + "/NormalMap.png";
        strength = Mathf.Clamp(strength, 0.0f, 1.0f);
        float yUp,yDown,xLeft,xRight,xDelta,yDelta;

        Texture2D normalTexture = new Texture2D(sourceImage.width, sourceImage.height);


        //Scan normal texture from left to right, starting from left bottom
        //Image may be revesed with Y axis pointing down
        for(int y = 0; y < normalTexture.width; y++)
        {
            for (int x = 0; x < normalTexture.width; x++)
            {
                yUp = sourceImage.GetPixel(x, y + 1).grayscale * strength;
                yDown = sourceImage.GetPixel(x, y - 1).grayscale * strength;
                xLeft = sourceImage.GetPixel(x - 1, y).grayscale * strength;
                xRight = sourceImage.GetPixel(x + 1, y).grayscale * strength;


                //R = (Nx + 1) / 2
                //G = (Ny + 1) / 2

                xDelta = ((xLeft - xRight) + 1) / 2;
                yDelta = ((yUp - yDown) + 1) / 2;

                normalTexture.SetPixel(x, y, new Color(xDelta, yDelta, 1, yDelta));
            }
        }

        normalTexture.Apply();
        Debug.Log("Function completed, writing to disk");

        System.IO.File.WriteAllBytes(NormalPath, normalTexture.EncodeToPNG());
        Debug.Log("Normal is saved at +" + NormalPath);

        //return normalTexture;

    }

    public void ConvertToGrayscale(Texture2D image)
    {
        Color32[] pixels = image.GetPixels32();
        for (int x = 0; x < image.width; x++)
        {
            for (int y = 0; y < image.height; y++)
            {
                Color32 pixel = pixels[x + y * image.width];
                int p = ((256 * 256 + pixel.r) * 256 + pixel.b) * 256 + pixel.g;
                int b = p % 256;
                p = Mathf.FloorToInt(p / 256);
                int g = p % 256;
                p = Mathf.FloorToInt(p / 256);
                int r = p % 256;
                float l = (0.2126f * r / 255f) + 0.7152f * (g / 255f) + 0.0722f * (b / 255f);
                Color c = new Color(l, l, l, 1);
                image.SetPixel(x, y, c);
            }
        }

        image.Apply(false);
        var bytes = image.EncodeToPNG();

        if(GreyscalePath == null)
            GreyscalePath = Application.persistentDataPath + "/grayscale.png";

        System.IO.File.WriteAllBytes(GreyscalePath, bytes);
    }

}
