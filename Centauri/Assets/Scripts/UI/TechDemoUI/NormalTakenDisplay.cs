using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalTakenDisplay : MonoBehaviour
{
    public RawImage photoTaken;
    public RawImage normalTaken;
    public RawImage heightMapTaken;


    private GetPhotoFromCamera getPhotoFromCamera;

    public void DisplayPhotoAndMap()
    {
        Texture2D image = GetPhotoFromCamera.LoadImageFromDiskHelper(GetPhotoFromCamera.ImagePath);
        Texture2D imageNormal = GetPhotoFromCamera.LoadImageFromDiskHelper(UIToGraphicManager.NormalPath);
        Texture2D heightMap = GetPhotoFromCamera.LoadImageFromDiskHelper(UIToGraphicManager.GreyscalePath);


        photoTaken.texture = image;
        normalTaken.texture = imageNormal;
        heightMapTaken.texture = heightMap;
    }


}
