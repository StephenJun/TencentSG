using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    public InteractiveObject interObj;
    public int Amount;
	//public AudioClip WIKIBtnClicked;

	[SerializeField]
	private Image durabilityImage;
	private Image DurabilityImage
	{
		get
		{
			if (durabilityImage == null)
			{
				durabilityImage = transform.Find("Img_Durability").GetComponent<Image>();
			}
			return durabilityImage;
		}
	}

    private Image image;
    private Image Image
    {
        get
        {
            if (image == null)
            {
                image = GetComponent<Image>();
            }
            return image;
        }
    }
    private Text text;
    private Text Text
    {
        get
        {
            if (text == null)
            {
                text = GetComponentInChildren<Text>();
            }
            return text;
        }
    }

    private float smoothing = 5;
	private float targetScale = 1;
	private Vector3 AnimScale = Vector3.one * 1.3f;

    private void Start()
    {
        Image.sprite = Resources.Load<Sprite>("ItemsIcon/" + interObj.itemName);
        Text.text = Amount.ToString();

    }

    private void Update()
    {
		DurabilityImage.fillAmount = interObj.durability / 100;
		if (transform.localScale.x != targetScale)
		{
			float scale = Mathf.Lerp(transform.localScale.x, targetScale, smoothing * Time.deltaTime);
			transform.localScale = Vector3.one * scale;
		}

		if(interObj.durability < 0)
		{
			Destroy(gameObject);
		}
	}

    public void SetIcon(InteractiveObject itemBase, int amount = 1)
    {
        //transform.localScale = AnimScale;
        this.interObj = itemBase;
        this.Amount = amount;
		//更新UI
        Image.sprite = Resources.Load<Sprite>("ItemsIcon/" + itemBase.itemName);
        if (Amount > 1)
        {
            Text.text = Amount.ToString();
        }
        else
        {
            Text.text = "";
        }
    }

	public void UpdateIcon()
	{
		Image.sprite = Resources.Load<Sprite>("ItemsIcon/" + this.interObj.itemName);
	}

    #region 控制物品数量的方法
    public void AddAmount(int amount = 1)
    {
        transform.localScale = AnimScale;
        this.Amount += amount;
        Text.text = Amount.ToString();
    }

    public void ReduceAmount(int amount = 1)
    {
        transform.localScale = AnimScale;
        this.Amount -= amount;
        if (Amount > 1)
            Text.text = Amount.ToString();
        else
            Text.text = "";
    }

    public void SetAmount(int amount)
    {
        transform.localScale = AnimScale;
        this.Amount = amount;
        if (Amount > 1)
        {
            Text.text = Amount.ToString();
        }
        else
        {
            Text.text = "";
        }
    }
    #endregion

    #region 给pickItem用方法
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void SetLocalPosition(Vector3 pos)
    {
        transform.localPosition = pos;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 1.3F;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
           
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            //InventoryManager.Instance.inventory.SetDescription(interObj);
            //AudioManager.Instance.PlayFXAudioClip(WIKIBtnClicked);
        }
    }
    #endregion
}
