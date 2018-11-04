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
    Transform UIParent;
    Animator[] animators;
    PoseType[] totalPoseTypes;
    List<PoseType> archivedPoseTypes;
    Image tipBar;
    bool[] archieved;
    //to do 建立一个对象池 根据totalPoseTypes取出相应的对象 置入platform子物体 setposition 根据archieved点亮动作
    void Start()
    {
        //init
        LevelData levelData = null;
        totalPoseTypes = JsonHandler.LoadLevelData(ref levelData, 1).totolType;
        archivedPoseTypes = new List<PoseType>();

        //PushPose(PoseType.Extinguisher);
        //PushPose(PoseType.Watch);
        //PushPose(PoseType.Routes);
        //PushPose(PoseType.Smoke);
        //StartPlayback();
    }
    public void StartPlayback()
    {
        StartCoroutine(PlaybackCoroutine());
    }
    public void PushPose(PoseType poseType)
    {
        if (!archivedPoseTypes.Contains(poseType))
        {
            archivedPoseTypes.Add(poseType);
        }
    }
    public void Level2()
    {
        canControll = false;
        archivedPoseTypes.Clear();
		animators = new Animator[3];
        LevelData levelData = null;
        totalPoseTypes = JsonHandler.LoadLevelData(ref levelData, 1).totolType;
    }

    private void Update()
    {
        if (platformParent && canControll && isFinished)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                RotatePlatform(-1);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                RotatePlatform(1);
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                GameManager.Instance.NextLevel();
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                GameManager.Instance.RestartLevel();
            }
        }
    }

    IEnumerator PlaybackCoroutine()
    {

        platformParent = GameObject.Find("PlatformParent").transform;
        UIParent = GameObject.Find("Canvas/UIs").transform;
        tipBar = GameObject.Find("Canvas/TipBar").GetComponent<Image>();
        CheckPoseTypes();
        LoadPoses();
        LoadUIs();
        SetAnimators();
        SetPostitions();
        float[] rotationAngleBank = new float[platformParent.childCount];
        float sectionAngle = 360f / platformParent.childCount;
        for (int i = 0; i < rotationAngleBank.Length; i++)
        {
            rotationAngleBank[i] = i * sectionAngle;
        }

        for (int i = 0; i < rotationAngleBank.Length; i++)
        {
            isFinished = false;
            platformParent.DORotate(new Vector3(0, rotationAngleBank[i]), 2).OnComplete(() => isFinished = true);
            PopTip(1);
            yield return new WaitForSeconds(2f);
            if (i < animators.Length && animators[i])
                animators[i].SetBool("Go", true);
            yield return new WaitUntil(delegate { return isFinished; });
            yield return new WaitForSeconds(1);
            if (i < animators.Length && animators[i])
                animators[i].SetBool("Go", false);
        }
        print("YEAH!");
        currentAngle = platformParent.eulerAngles.y;
        RotatePlatform();
        tipBar.DOFade(1, 0);
        canControll = true;
    }

    float currentAngle;
    private void RotatePlatform(int direction = 1)
    {
        if (!isFinished) return;
        isFinished = false;
        if (direction == 1)
        {
            currentAngle += 360 / platformParent.childCount;
        }
        else if (direction == -1)
        {
            currentAngle -= 360 / platformParent.childCount;
        }
        PopTip(direction);
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
        if (lastUIID < animators.Length && animators[lastUIID])
            DOVirtual.DelayedCall(2, () => animators[lastUIID].SetBool("Go", true));
        //animators[lastUIID].SetBool(lastUIID.ToString(), true);
        UIs[lastUIID].transform.localPosition = new Vector3(-349, 1000, 0);
        UIs[lastUIID].transform.DOLocalMoveY(80, 0.4f);
    }

    void SetAnimators()
    {
        animators = new Animator[totalPoseTypes.Length]; //
        int i = 0;
        foreach (Transform platform in platformParent)
        {
            animators[i] = platform.GetComponentInChildren<Animator>();
            i++;
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
    void LoadPoses()
    {
        for (int i = 0; i < totalPoseTypes.Length; i++)
        {
            GameObject go = Resources.Load("Pose/Bear_Playback_" + totalPoseTypes[i].ToString()) as GameObject;
            if (!go)
            {
                Debug.Log("Pose/Bear_Playback_Routes_" + totalPoseTypes[i].ToString() + " is empty");
                go = Resources.Load("Pose/Bear_Playback_Empty") as GameObject;
            }
            go = Instantiate(go, platformParent);
        }
    }
    void LoadUIs()
    {
        for (int i = 0; i < totalPoseTypes.Length; i++)
        {
            GameObject go = Resources.Load("Tips/Tip_" + totalPoseTypes[i].ToString()) as GameObject;
            if (!go || !archieved[i])
            {
                go = Resources.Load("Tips/Tip_empty") as GameObject;
            }
            go = Instantiate(go, UIParent);
            //Debug.Log();
            //go.GetComponent<>
        }
        UIs = UIParent.GetComponentsInChildren<Image>();
        lastUIID = UIs.Length - 1;
    }
    [SerializeField]
    float radius;
    [ContextMenu("SetPostitions")]
    public void SetPostitions()
    {
        int i = 0;
        float sectionAngle = Mathf.PI * 2 / platformParent.childCount;
        foreach (Transform camPos in platformParent)
        {
            camPos.position = new Vector3(radius * Mathf.Cos(sectionAngle * i), 0, radius * Mathf.Sin(sectionAngle * i));
            camPos.eulerAngles = new Vector3(0, -i * 360 / platformParent.childCount + 90);
            i++;
        }
        Debug.Log("Set positons of " + platformParent.childCount);
    }

}
public enum PoseType
{
    Smoke,
    Watch,
    Extinguisher,
    Routes
}