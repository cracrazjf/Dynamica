using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class TestCodeOnly : MonoBehaviour
{
    [SerializeField]
    Camera FPSCamera;
    [SerializeField]
    RawImage displayImage;
    [SerializeField]
    Transform name_Value_Pair;
    int[] resolution = new int[2];
    List<string>driveLabelList = new List<string> {
        "hunger", 
        "thirst", 
        "sleepiness",
        "fatigue",
        "health",
    };
    List<string> bodyLabelList = new List<string> {
        "standing", 
        "sitting",
        "crouching",
        "sleeping",
        "laying",
        "alive"
    };
    List<string> actionLabelList = new List<string> {
        "crouch",      // 0, negative is down 
        "sit",         // 1, negative is down
        "lay",         // 2 -1 or 1 (or 0 if not switched)
        "stand",       // 3 -1 or 1 (or 0 if not switched)
        "rotate",      // 4, now a proportion
        "take steps",  // 5, now a proportion
        "consume",     // 6, set to consumable if ongoing
        "sleep",       // 7, awake/maintain/fall asleep
        "rest",        // 8 -1 or 1 (or 0 if not switched)
        "look",        // 9
        "use hands",   // 10 -1 left / 1 right
        "look vertically",   // 11 -1 or 1 (or 0 if not switched)
        "look horizontally", // 12 -1 or 1 (or 0 if not switched)
        "RP x",        // 13  -1 to 1, proportion of max range from start pos
        "RP y",        // 14
        "RP z",        // 15
            
    };
    Matrix<float> m = Matrix<float>.Build.Dense(1, 5, -1);
    bool showHiddenOnly;
    Transform driveInputContent;
    Transform bodyInputContent;
    Transform actionInputContent;
    Transform redVisualInputContent;
    Transform greenVisualInputContent;
    Transform blueVisualInputContent;
    Transform hiddenContent;
    Transform driveOutputContent;
    Transform bodyOutputContent;
    Transform actionOutputContent;
    Transform redVisualOutputContent;
    Transform greenVisualOutputContent;
    Transform blueVisualOutputContent;
    List<Transform> contentList = new List<Transform>();
    RawImage redInputImage;
    RawImage greenInputImage;
    RawImage blueInputImage;
    RawImage hiddenImage;
    RawImage redOutputImage;
    RawImage greenOutputImage;
    RawImage blueOutputImage;
    List<RawImage> driveInputImages = new List<RawImage>();
    List<RawImage> bodyInputImages = new List<RawImage>();
    List<RawImage> actionInputImages = new List<RawImage>();
    List<RawImage> driveOutputImages = new List<RawImage>();
    List<RawImage> bodyOutputImages = new List<RawImage>();
    List<RawImage> actionOutputImages = new List<RawImage>();
    
    Matrix<float> visualInputMatrix;
    // Start is called before the first frame update
    void Start()
    {


        resolution[0] = FPSCamera.targetTexture.width;
        resolution[1] = FPSCamera.targetTexture.height;

        visualInputMatrix = Matrix<float>.Build.Dense(3, resolution[0] * resolution[1]);


        driveInputContent = GameObject.Find("DriveInput Content").transform;
        bodyInputContent = GameObject.Find("BodyInput Content").transform;
        actionInputContent = GameObject.Find("ActionInput Content").transform;
        hiddenContent = GameObject.Find("Hidden Content").transform.GetChild(0);
        redVisualInputContent = GameObject.Find("RedVisualInput Content").transform.GetChild(0);
        greenVisualInputContent = GameObject.Find("GreenVisualInput Content").transform.GetChild(0);
        blueVisualInputContent = GameObject.Find("BlueVisualInput Content").transform.GetChild(0);
        driveOutputContent = GameObject.Find("DriveOutput Content").transform;
        bodyOutputContent = GameObject.Find("BodyOutput Content").transform;
        actionOutputContent = GameObject.Find("ActionOutput Content").transform;
        redVisualOutputContent = GameObject.Find("RedVisualOutput Content").transform.GetChild(0);
        greenVisualOutputContent = GameObject.Find("GreenVisualOutput Content").transform.GetChild(0);
        blueVisualOutputContent = GameObject.Find("BlueVisualOutput Content").transform.GetChild(0);


        GenerateContentWithTitle(driveLabelList, driveInputContent, driveInputImages);
        GenerateContentWithTitle(bodyLabelList, bodyInputContent, bodyInputImages);
        GenerateContentWithTitle(actionLabelList, actionInputContent, actionInputImages);
        redInputImage = Instantiate(displayImage, redVisualInputContent);
        greenInputImage = Instantiate(displayImage, greenVisualInputContent);
        blueInputImage = Instantiate(displayImage, blueVisualInputContent);
        hiddenImage = Instantiate(displayImage, hiddenContent);
        GenerateContentWithTitle(driveLabelList, driveOutputContent, driveOutputImages);
        GenerateContentWithTitle(bodyLabelList, bodyOutputContent, bodyOutputImages);
        GenerateContentWithTitle(actionLabelList, actionOutputContent, actionOutputImages);
        redOutputImage = Instantiate(displayImage, redVisualOutputContent);
        greenOutputImage = Instantiate(displayImage, greenVisualOutputContent);
        blueOutputImage = Instantiate(displayImage, blueVisualOutputContent);

        contentList.Add(driveInputContent);
        contentList.Add(bodyInputContent);
        contentList.Add(actionInputContent);
        contentList.Add(hiddenContent);
        contentList.Add(redVisualInputContent);
        contentList.Add(greenVisualInputContent);
        contentList.Add(blueVisualInputContent);
        contentList.Add(driveOutputContent);
        contentList.Add(bodyOutputContent);
        contentList.Add(actionOutputContent);
        contentList.Add(redVisualOutputContent);
        contentList.Add(greenVisualOutputContent);
        contentList.Add(blueVisualOutputContent);

    }
    // Update is called once per frame
    void Update()
    {
        UpdateVisualInput(resolution[0], resolution[1]);

        float[] inputRedColorArray = ConvertToColor(visualInputMatrix.Row(0));
        float[] inputGreenColorArray = ConvertToColor(visualInputMatrix.Row(1));
        float[] inputBlueColorArray = ConvertToColor(visualInputMatrix.Row(2));
        DisplayColor(inputRedColorArray, redInputImage);
        DisplayColor(inputGreenColorArray, greenInputImage);
        DisplayColor(inputBlueColorArray, blueInputImage);

        float[] outputRedColorArray = ConvertToColor(visualInputMatrix.Row(0));
        float[] outputGreenColorArray = ConvertToColor(visualInputMatrix.Row(1));
        float[] outputBlueColorArray = ConvertToColor(visualInputMatrix.Row(2));
        DisplayColor(outputRedColorArray, redOutputImage);
        DisplayColor(outputGreenColorArray, greenOutputImage);
        DisplayColor(outputBlueColorArray, blueOutputImage);
    }

    void GenerateContentWithTitle(List<string> contentLableList, Transform content, List<RawImage> contentImages) 
    { 
        if(contentLableList.Count >= 15)
        {
            content.GetComponent<GridLayoutGroup>().cellSize = new Vector2(40, 30);
        }
        for (int i = 0; i < contentLableList.Count; i++)
        {
            Instantiate(name_Value_Pair, content);
            content.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text = contentLableList[i];
            contentImages.Add(content.GetChild(i).GetChild(1).GetComponent<RawImage>());
        }
    }
    float[] ConvertToColor(Vector<float> passedV)
    {
        List<float> colorList = new List<float>();
        foreach(System.Tuple<int, float> x in passedV.EnumerateIndexed())
        {
            float scaledValue = (x.Item2 + 1) / 2;
            if (scaledValue <= 0.5)
            {
                colorList.Add(1);
                colorList.Add(scaledValue * 2);
                colorList.Add(scaledValue * 2);
                colorList.Add(1);
            }
            else
            {
                colorList.Add((scaledValue * -2) + 2);
                colorList.Add(1);
                colorList.Add((scaledValue * -2) + 2);
                colorList.Add(1);
            }
            
        }
        return colorList.ToArray();
    }
    void UpdateVisualInput(int width, int height)
    {

        Texture2D visualInputTexture = new Texture2D(width, height, TextureFormat.RGB24, false);

        FPSCamera.Render();
        RenderTexture.active = FPSCamera.targetTexture;
        visualInputTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        Color[] colorArray = visualInputTexture.GetPixels();

        int resolutionSquared = width * height;
        for (int i = 0; i < resolutionSquared; i++)
        {
            visualInputMatrix[0, i] = (colorArray[i].r * 2) -1;
            visualInputMatrix[1, i] = (colorArray[i].g * 2) - 1;
            visualInputMatrix[2, i] = (colorArray[i].b * 2) - 1;
        }
    }
    void DisplayColor(float[] colorArray, RawImage image)
    {
        var tex = new Texture2D(resolution[0], resolution[1], TextureFormat.RGBAFloat, true);
        tex.SetPixelData(colorArray, 0, 0); // mip 0

        tex.filterMode = FilterMode.Point;
        tex.Apply(updateMipmaps: false);

        image.texture = tex;
    }
    public void SwitchDisplayHiddenLayer()
    {
        if (!showHiddenOnly)
        {
            foreach (Transform content in contentList)
            {
                if (content.name != "Hidden Values")
                {
                    content.gameObject.SetActive(false);
                }
                else
                {
                    
                }
                showHiddenOnly = true;
            }
        }
        else
        {
            foreach (Transform content in contentList)
            {
                if (content.name != "Hidden Values")
                {
                    content.gameObject.SetActive(true);
                }
                else
                {
                    content.gameObject.SetActive(false);
                }
                showHiddenOnly = false;
            }
        }
    }

}
