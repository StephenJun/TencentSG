using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class PlaybackManager : Singleton<PlaybackManager>
{

    bool isFinished;
    bool canControll;
    float[] rotationAngleBank;
    Transform platformParent;
    Image[] UIs;
    Animator[] animators;
    public float radius;

    void Start()
    {
        //UIs = transform.Find("UIs").GetComponentsInChildren<Image>();
        StartCoroutine(PlaybackCoroutine());

    }
    private void Update()
    {
        if (platformParent && canControll)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                RotatePlatform(1);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                RotatePlatform(-1);
            }
        }
    }
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
            platformParent.DORotate(new Vector3(0, rotationAngleBank[i]), 2).OnComplete(() => isFinished = true);
            yield return new WaitForSeconds(1f);
            if (i < animators.Length)
                animators[i].SetBool("Go", true);
            yield return new WaitUntil(delegate { return isFinished; });
            yield return new WaitForSeconds(1);
        }
        print("YEAH!");
        currentAngle = platformParent.eulerAngles.y;
        canControll = true;
    }

    [ContextMenu("SetPostitions")]
    public void SetPostitions()
    {
        platformParent = GameObject.Find("PlatformParent").transform;
        int i = 0;
        float sectionAngle = Mathf.PI * 2 / platformParent.childCount;
        foreach (Transform camPos in platformParent)
        {
            camPos.position = new Vector3(radius * Mathf.Cos(sectionAngle * i), 0, radius * Mathf.Sin(sectionAngle * i));
            camPos.eulerAngles = new Vector3(0, -i * 360 / platformParent.childCount + 90);
            PopTip(i);
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
        }
        else
        {
            currentAngle -= 360 / platformParent.childCount;
        }        
        platformParent.DORotate(new Vector3(0, currentAngle), 2).OnComplete(() => isFinished = true);
    }
    Image lastUI;
    private void PopTip(int i)
    {
        //if (lastUI) lastUI.
        UIs[i].transform.DOScale(0,0.8f);
    }
}
