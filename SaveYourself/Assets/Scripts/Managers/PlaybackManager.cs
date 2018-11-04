using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class PlaybackManager : Singleton<PlaybackManager>
{

    bool isFinished;
    bool canControll;

    Transform platformParent;
    Animator[] animators;
    PoseType[] totalPoseTypes;
    List<PoseType> archivedPoseTypes;
    bool[] archieved;
    //to do 建立一个对象池 根据totalPoseTypes取出相应的对象 置入platform子物体 setposition 根据archieved点亮动作
    void Start()
    {
        LevelData levelData = null;
        totalPoseTypes = JsonHandler.LoadLevelData(ref levelData, 1).totolType;
        archivedPoseTypes = new List<PoseType>();

        PushPose(PoseType.Extinguisher);
        PushPose(PoseType.Extinguisher);
        PushPose(PoseType.Extinguisher);
        PushPose(PoseType.Smoke);

        StartPlayback();
    }
    public void StartPlayback()
    {
        UIs = GameObject.Find("Canvas/UIs").GetComponentsInChildren<Image>();
        lastUIID = UIs.Length - 1;
        StartCoroutine(PlaybackCoroutine());
        CheckPoseTypes();

    }
    private void Update()
    {
        if (platformParent && canControll)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                RotatePlatform(-1);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                RotatePlatform(1);
            }
        }
    }

    float[] rotationAngleBank;
    IEnumerator PlaybackCoroutine()
    {
        platformParent = GameObject.Find("PlatformParent").transform;
        rotationAngleBank = new float[platformParent.childCount];
        float sectionAngle = 360f / platformParent.childCount;
        animators = platformParent.GetComponentsInChildren<Animator>();
        for (int i = 0; i < rotationAngleBank.Length; i++)
        {
            rotationAngleBank[i] = i * sectionAngle;
        }

        for (int i = 0; i < rotationAngleBank.Length; i++)
        {
            isFinished = false;
            platformParent.DORotate(new Vector3(0, rotationAngleBank[i]), 2, RotateMode.FastBeyond360).OnComplete(() => isFinished = true);
            PopTip(1);
            yield return new WaitForSeconds(1f);
            if (i < animators.Length)
                animators[i].SetBool("Go", true);
            yield return new WaitUntil(delegate { return isFinished; });
            yield return new WaitForSeconds(1);
            if (i < animators.Length)
                animators[i].SetBool("Go", false);
        }
        print("YEAH!");
        currentAngle = platformParent.eulerAngles.y;
        canControll = true;
    }

    [SerializeField]
    float radius;
    [ContextMenu("SetPostitions")]
    public void SetPostitions()
    {
        if(platformParent = GameObject.Find("PlatformParent").transform) return;
        int i = 0;
        float sectionAngle = Mathf.PI * 2 / platformParent.childCount;
        foreach (Transform camPos in platformParent)
        {
            camPos.position = new Vector3(radius * Mathf.Cos(sectionAngle * i), 0, radius * Mathf.Sin(sectionAngle * i));
            camPos.eulerAngles = new Vector3(0, -i * 360 / platformParent.childCount + 90);
            i++;
        }
        Debug.Log("Set positons of" + platformParent.childCount);
    }

    float currentAngle;
    private void RotatePlatform(int i = 1)
    {
        platformParent.DOKill();
        if (i == 1)
        {
            currentAngle += 360 / platformParent.childCount;
            PopTip(i);
        }
        else if (i == -1)
        {
            currentAngle -= 360 / platformParent.childCount;
            PopTip(i);
        }        
        platformParent.DORotate(new Vector3(0, currentAngle), 2).OnComplete(() => isFinished = true);
    }

    int lastUIID;
    Image[] UIs;
    private void PopTip(int direction)
    {
        if (UIs[lastUIID]) UIs[lastUIID].rectTransform.DOLocalMoveX(-1426, 0.4f);
        if (direction == -1)
        {
            lastUIID--;
            if (lastUIID < 0) lastUIID = UIs.Length - 1;
        }
        else if (direction == 1)
        {
            lastUIID++;
            if (lastUIID > UIs.Length - 1) lastUIID = 0;
        }
        if (lastUIID < animators.Length)
            animators[lastUIID].SetBool("Go", true);
        UIs[lastUIID].transform.localPosition = new Vector3(-277,1000,0);
        UIs[lastUIID].transform.DOLocalMoveY(-78,0.4f);
    }

    public void PushPose(PoseType poseType)
    {
        if (!archivedPoseTypes.Contains(poseType))
        {
            archivedPoseTypes.Add(poseType);
        }
    }
    void CheckPoseTypes()
    {
        archieved = new bool[totalPoseTypes.Length];
        foreach (var item in archivedPoseTypes)
        {
            int index = 0;
            foreach (var item2 in totalPoseTypes)
            {
                if (item == item2)
                {
                    archieved[index] = true;
                }
                index++;
            }
        }
    }
}
public enum PoseType
{
    Smoke,
    Watch,
    Extinguisher,
    Routes
}

class PlayerControllerT : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

}