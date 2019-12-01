using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshRenderer))]
public class GetPhotoFromCamera : MonoBehaviour
{
    private WebCamTexture webCamTexture;
    private Renderer meshRenderer;
    public Animator canvasAnimator;

    public bool ImageReady { get; set; }

    public bool enableLiveCam;
    private ShaderFieldsControl shaderFieldsControl;
    private UIToGraphicManager uIToGraphicManager;

    public static string ImagePath {get; set;}

    private void Awake() 
    {
        ImagePath = Application.persistentDataPath + "/photoTaken.png";
    }

    private void Start()
    {
        ImageReady = false;

        shaderFieldsControl = GameObject.FindObjectOfType<ShaderFieldsControl>();
        uIToGraphicManager = GameObject.FindObjectOfType<UIToGraphicManager>();
        meshRenderer = GetComponent<Renderer>();
        webCamTexture = new WebCamTexture();
        //Set mesh texture to that of webcam
        meshRenderer.material.mainTexture = webCamTexture;
        WebCamDevice[] devices = WebCamTexture.devices;
        //foreach (var device in devices)
        //    Debug.Log("Camera device name: " + device.name);
        //webCamTexture.deviceName = devices[1].name;
        webCamTexture.Play();
    }

    void Update()
    {
        GetComponent<RawImage>().texture = webCamTexture;
    }


    public Texture2D SaveTakenPhoto()
    {
        bool success = false;
        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();
        byte[] bytes = photo.EncodeToPNG();


        File.WriteAllBytes(ImagePath, bytes);
        Debug.Log("Phto is saved at " +ImagePath);


        // canvasAnimator.ResetTrigger("CameraTrigger");
        // canvasAnimator.SetTrigger("ExitTrigger");

        ImageReady = true;
        return photo;
    }

    public static Texture2D LoadImageFromDiskHelper(string path)
    {
            Texture2D normalTex = null;
            byte[] fileData;

            if (File.Exists(path))
            {
                fileData = File.ReadAllBytes(path);
                normalTex = new Texture2D(2, 2);
                normalTex.LoadImage(fileData);
            }

            return normalTex;
    }

    
}
