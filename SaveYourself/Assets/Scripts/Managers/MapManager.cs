using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapManager : Singleton<MapManager>
{
    [SerializeField]
    Transform targetTransform;
    RectTransform originTransform;
    RectTransform coverTransform;
    RectTransform rotationHandler;
    Texture2D fog;
    int mapScaleX;
    int mapScaleY;
    int miniMapScaleX;
    int miniMapScaleY;
    Color[] colorBuffer;
    Color[] eraserBuffer;
    [SerializeField]
    float scaleFactor = 5;
    float U;
    float V;
    [SerializeField] GameObject[] map = new GameObject[2];
    protected override void Awake()
    {
        base.Awake();
        rotationHandler = transform.Find("RotationHandler").gameObject.GetComponent<RectTransform>();
        originTransform = rotationHandler.Find("MapOrigin 2").gameObject.GetComponent<RectTransform>();
        coverTransform = rotationHandler.Find("MapCover").gameObject.GetComponent<RectTransform>();
        fog = new Texture2D((int)originTransform.sizeDelta.x, (int)originTransform.sizeDelta.y, TextureFormat.Alpha8, false);
        rotationHandler.Find("MapCover").gameObject.GetComponent<RawImage>().texture = fog;

        mapScaleX = (int)originTransform.sizeDelta.x;
        mapScaleY = (int)originTransform.sizeDelta.y;
        miniMapScaleX = (int)GetComponent<RectTransform>().sizeDelta.x;
        miniMapScaleY = (int)GetComponent<RectTransform>().sizeDelta.y;
        fog.Apply();
        

    }
    [ContextMenu("Reset")]
    void Start()
    {
        Texture2D eraser = Resources.Load<Texture2D>("eraser");
        eraserBuffer = eraser.GetPixels();
        U = 256 / (float)miniMapScaleX;
        V = 256 / (float)miniMapScaleY;
        colorBuffer = new Color[fog.width * fog.height];
        for (int i = 0; i < colorBuffer.Length; i++)
        {
            colorBuffer[i] = Color.black;
        }
        fog.SetPixels(colorBuffer);
        fog.Apply();
    }

    void Update()
    {
        if (!targetTransform && PlayerController.Instance) targetTransform = PlayerController.Instance.transform;
        else if (targetTransform) UpdateMap1(Mapping(targetTransform.position));

    }
    public Vector2Int Mapping(Vector3 position)
    {
        Vector2Int _temp = Vector2Int.zero;
        _temp.x = (int)(position.x * scaleFactor);
        _temp.y = (int)(position.z * scaleFactor);
        return _temp;
    }

    void UpdateMap(Vector2Int playerPosition)
    {
        int pixelX;
        int pixelY;
        int playerPositionX = (int)(playerPosition.x * scaleFactor);
        int playerPositionY = (int)(playerPosition.y * scaleFactor);
        originTransform.localPosition = coverTransform.localPosition = new Vector3(-playerPositionX, -playerPositionY, 0);
        //rotationHandler.eulerAngles = new Vector3(0, 0, targetTransform.eulerAngles.y);


    }
    void UpdateMap1(Vector2Int playerPosition)
    {
        int pixelX;
        int pixelY;
        int playerPositionX = (int)(playerPosition.x);
        int playerPositionY = (int)(playerPosition.y);
        originTransform.localPosition = coverTransform.localPosition = new Vector3(-playerPositionX, -playerPositionY, 0);
        //rotationHandler.eulerAngles = new Vector3(0, 0, targetTransform.eulerAngles.y);
        //get color from new positon
        //colorBuffer = fog.GetPixels((mapScaleX - miniMapScaleX) / 2 + playerPositionX, (mapScaleY - miniMapScaleY) / 2 + playerPositionY, miniMapScaleX, miniMapScaleY);
        //    int pixelNeedUpdateX = Mathf.Clamp(mapScaleX - (mapScaleX - miniMapScaleX) / 2 - playerPositionX, 0, miniMapScaleX);
        //    int pixelNeedUpdateY = Mathf.Clamp(mapScaleY - (mapScaleY - miniMapScaleY) / 2 - playerPositionY, 0, miniMapScaleY);
        //    colorBuffer = fog.GetPixels(
        //Mathf.Clamp((mapScaleX - miniMapScaleX) / 2 + playerPositionX, 0, mapScaleX)
        //, Mathf.Clamp((mapScaleY - miniMapScaleY) / 2 + playerPositionY, 0, mapScaleY)
        //, pixelNeedUpdateX
        //, pixelNeedUpdateY);

        for (int i = 0; i < miniMapScaleY; i++)
        {
            for (int j = 0; j < miniMapScaleX; j++)
            {
                pixelX = (mapScaleX - miniMapScaleX) / 2 + j + playerPositionX;
                pixelY = (mapScaleY - miniMapScaleY) / 2 + i + playerPositionY;
                int k = (int)(i * 256 * V) + (int)(j * U - 1);
                if (colorBuffer[i * miniMapScaleY + j].a > eraserBuffer[(int)(((int)(i * V - 0.1) * 256) + (j * U))].a)
                {
                    fog.SetPixel(pixelX, pixelY, eraserBuffer[(int)(((int)(i * V - 0.1) * 256) + (j * U))]);
                }
            }
        }
        fog.Apply();
    }

    //void UpdateMap1(Vector2Int playerPosition)
    //{
    //    int pixelX;
    //    int pixelY;
    //    int playerPositionX = (int)(playerPosition.x * scaleFactor);
    //    int playerPositionY = (int)(playerPosition.y * scaleFactor);
    //    originTransform.localPosition = coverTransform.localPosition = new Vector3(-playerPositionX, -playerPositionY, 0);
    //    rotationHandler.eulerAngles = new Vector3(0, 0, PlayerController.Instance.transform.eulerAngles.y);
    //    //get color from new positon
    //    colorBuffer = fog.GetPixels((mapScaleX - miniMapScaleX ) / 2 + playerPositionX, (mapScaleY - miniMapScaleY) / 2 + playerPositionY , miniMapScaleX, miniMapScaleY);
    //    for (int i = 0; i < miniMapScaleY; i++)
    //    {
    //        for (int j = 0; j < miniMapScaleX; j++)
    //        {
    //            pixelX = (mapScaleX - miniMapScaleX) / 2 + j + playerPositionX;
    //            pixelY = (mapScaleY - miniMapScaleY) / 2 + i + playerPositionY;
    //            int k = (int)(i * 256 * V) + (int)(j * U - 1);
    //            if (colorBuffer[i * miniMapScaleY + j].a > eraserBuffer[(int)(((int)(i * V - 0.1) * 256) + (j * U))].a)
    //            {
    //                fog.SetPixel(pixelX, pixelY, eraserBuffer[(int)(((int)(i * V - 0.1) * 256) + (j * U))]);
    //            }
    //        }
    //    }
    //    fog.Apply();
    //}

    void UpdateMap2(Vector2Int playerPosition)
    {
        Vector2Int position;
        int playerPositionX = playerPosition.x;
        int playerPositionY = playerPosition.y;
        originTransform.localPosition = coverTransform.localPosition = new Vector3(-playerPositionX * 10, -playerPositionY * 10, 0);
        rotationHandler.eulerAngles = new Vector3(0, 0, targetTransform.eulerAngles.y);

        colorBuffer = fog.GetPixels((mapScaleX - miniMapScaleX) / 2, (mapScaleY - miniMapScaleY) / 2, 256, 256);

        for (int i = 0; i < miniMapScaleY; i++)
        {
            for (int j = 0; j < miniMapScaleX; j++)
            {
                position = new Vector2Int((mapScaleX - miniMapScaleX) / 2 + j + playerPositionX * 10, (mapScaleY - miniMapScaleY) / 2 + i + playerPositionY * 10);

                if (colorBuffer[i * 256 + j].a > eraserBuffer[j + i * 256].a)
                {
                    colorBuffer[i * 256 + j] = eraserBuffer[j + i * 256];
                }
            }
        }
        fog.SetPixels((mapScaleX - miniMapScaleX) / 2, (mapScaleY - miniMapScaleY) / 2, 256, 256, colorBuffer);
        fog.Apply();
    }

    public void UseMap(int index)
    {
        if (index == 0)
        {
            rotationHandler.Find("MapOrigin 1").gameObject.SetActive(true);
            rotationHandler.Find("MapOrigin 2").gameObject.SetActive(false);
            originTransform = rotationHandler.Find("MapOrigin 1").gameObject.GetComponent<RectTransform>();
            map[0].SetActive(true);
            map[1].SetActive(false);
        }
        if (index == 1)
        {
            rotationHandler.Find("MapOrigin 2").gameObject.SetActive(true);
            rotationHandler.Find("MapOrigin 1").gameObject.SetActive(false);
            originTransform = rotationHandler.Find("MapOrigin 2").gameObject.GetComponent<RectTransform>();
            map[0].SetActive(false);
            map[1].SetActive(true);
        }

    }
}
