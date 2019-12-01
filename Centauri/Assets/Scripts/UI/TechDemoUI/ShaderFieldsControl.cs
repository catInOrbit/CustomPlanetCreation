using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ShaderFieldsControl : MonoBehaviour
{
    public Slider diffuseSlider;
    public Button camereaExit;

    // public Button chooseNormal;
    // public Button chosenTexture;
    // public Button chosenHeightmap;

    public Button acceptNormalHeight;

    public Slider heightMapHeight;
    public Slider specularPercentage;
    public Slider specularPower;
    public Slider ambientPercentage;
    public Slider bumpScale;

    public FlexibleColorPicker atmosphereColor;
    public FlexibleColorPicker planetColor;



    //--------------------------------------------------
    private GraphicControl graphicControl;
    private UIToGraphicManager uIToGraphicManager = new UIToGraphicManager();
    public GetPhotoFromCamera getPhotoFromCamera;

    public Texture2D ImageTaken { get; set; }


    private void Start()
    {
        if(graphicControl == null)
            graphicControl = new GraphicControl(GameObject.FindGameObjectWithTag("SurfacePoint").GetComponent<Renderer>().material);

        diffuseSlider.onValueChanged.AddListener(delegate { DiffuseControl(); });
        camereaExit.onClick.AddListener(TakePhotoOnButtonClick);

        // chooseNormal.onClick.AddListener(ApplyNormalMap);
        // chosenTexture.onClick.AddListener(ApplyTexture);
        // chosenHeightmap.onClick.AddListener(ApplyHeightMap);
        acceptNormalHeight.onClick.AddListener(ApplyNormalAndHeight);

        heightMapHeight.onValueChanged.AddListener(delegate { ApplyHeightMapHeigt(); });
        specularPercentage.onValueChanged.AddListener(delegate { ApplySpecular(); });
        specularPower.onValueChanged.AddListener(delegate { ApplySpecularPower(); });

        specularPower.onValueChanged.AddListener(delegate { ApplySpecularPower(); });

        bumpScale.onValueChanged.AddListener(delegate { ApplyBumpScale(); });
    }

    private void Update()
    {
        ApplyPlanetColor();
        ApplyAtmosphereColor();
    }

    private void DiffuseControl() => graphicControl.SetDiffusePercentage(diffuseSlider.value);

    public void InitiateColorPickerObject()
    {   
        atmosphereColor = GameObject.FindGameObjectWithTag("AtmosphereColor").gameObject.GetComponent<FlexibleColorPicker>();
        planetColor = GameObject.FindGameObjectWithTag("PlanetColor").gameObject.GetComponent<FlexibleColorPicker>();
    }


    public void TakePhotoOnButtonClick()
    {
        ImageTaken = getPhotoFromCamera.SaveTakenPhoto();
        if (ImageTaken == null)
            Debug.LogError("Can not save photo");

        getPhotoFromCamera.ImageReady = false;
    }

    public void LoadNormalFromDisk()
    {
        if (ImageTaken != null)
            uIToGraphicManager.GetNormalMapFromTexture(ImageTaken, 2);
    }

    public void LoadHeigtmapFromDisk()
    {
        if (ImageTaken != null)
            uIToGraphicManager.ConvertToGrayscale(ImageTaken);
    }


    public void ApplyNormalAndHeight()
    {
        ApplyNormalMap();
        ApplyHeightMap();
        ApplyTexture();
    }

    public void ApplyNormalMap()
    {
        if (ImageTaken != null)
        {
            // uIToGraphicManager.GetNormalMapFromTexture(ImageTaken, 2);

            Texture2D normalTex = null;
            // byte[] fileData;
            

            // if (File.Exists(UIToGraphicManager.NormalPath))
            // {
            //     fileData = File.ReadAllBytes(Application.persistentDataPath + "/NormalMap.png");
            //     normalTex = new Texture2D(2, 2);
            //     normalTex.LoadImage(fileData);
            // }

            normalTex = GetPhotoFromCamera.LoadImageFromDiskHelper(UIToGraphicManager.NormalPath);

            if(graphicControl == null)
                graphicControl = new GraphicControl(GameObject.FindGameObjectWithTag("SurfacePoint").GetComponent<Renderer>().material);
            graphicControl.ApplyNormalMap(normalTex);
        }
    }

    public void ApplyHeightMap()
    {
        if (ImageTaken != null)
        {
            // uIToGraphicManager.ConvertToGrayscale(ImageTaken);

            Texture2D heightmap = null;
            // byte[] fileData;

            // if (File.Exists(UIToGraphicManager.GreyscalePath))
            // {
            //     fileData = File.ReadAllBytes(Application.persistentDataPath + "/NormalMap.png");
            //     normalTex = new Texture2D(2, 2);
            //     normalTex.LoadImage(fileData);
            // }

            heightmap = GetPhotoFromCamera.LoadImageFromDiskHelper(UIToGraphicManager.GreyscalePath);

            if(graphicControl == null)
                graphicControl = new GraphicControl(GameObject.FindGameObjectWithTag("SurfacePoint").GetComponent<Renderer>().material);

            graphicControl.ApplyHeightMap(heightmap);
        }
    }

    public void ApplyTexture()
    {
        if (ImageTaken != null)
        {
            if(graphicControl == null)
                graphicControl = new GraphicControl(GameObject.FindGameObjectWithTag("SurfacePoint").GetComponent<Renderer>().material);

            graphicControl.ApplyTexture(ImageTaken);
        }

        else
         Debug.LogError("Unable to load texture");
    }

    public void ApplyHeightMapHeigt() => graphicControl.ApplyHeightMapHeight(heightMapHeight.value);
    public void ApplySpecular() => graphicControl.ApplySpecular(specularPercentage.value);
    public void ApplySpecularPower() => graphicControl.ApplySepcularPower(specularPower.value);
    public void ApplyAmbient() => graphicControl.ApplySepcularPower(ambientPercentage.value);
    public void ApplyAtmosphereColor() => graphicControl.ApplyAtmosphereColor(atmosphereColor.color);

    public void ApplyPlanetColor() => graphicControl.ApplyColor(planetColor.color);

    public void ApplyBumpScale() => graphicControl.ApplyBumpScale(bumpScale.value);

}