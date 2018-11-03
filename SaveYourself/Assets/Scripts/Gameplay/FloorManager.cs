using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : Singleton<FloorManager>
{

	public int floorRow = 5;
	public int floorColume = 5;
	public FloorDetail[,] fd;

	private bool isStarted;

	public void GameStart()
	{
		ReadIdMap();
		isStarted = true;
	}
	Color32 normalColor = new Color32(0, 255, 0, 0);
	Color32 fireColor = new Color32(255, 0, 0, 0);
	Color32 wallColor = new Color32(0, 0, 255, 0);
	Color32 waterColor = new Color32(0, 0, 0, 255);
	private void ReadIdMap()
	{
		Texture2D map = Resources.Load<Texture2D>("IDMaps/IDMap_Level1");
		Color32[] colors = map.GetPixels32();
		int k = 0;
		fd = new FloorDetail[floorRow, floorColume];
		for (int j = 0; j < floorRow; j++)
		{
			for (int i = 0; i < floorColume; i++)
			{
				Floortype tempType;
				if (colors[k].g == normalColor.g)
				{
					tempType = Floortype.None;
				}
				else if (colors[k].r == fireColor.r)
				{
					tempType = Floortype.Fire;
				}
				else if (colors[k].b == wallColor.b)
				{
					tempType = Floortype.Wall;
				}else if(colors[k].a == waterColor.a)
				{
					tempType = Floortype.Water;
					//Debug.LogError("shui" + colors[k] + " " + wallColor + " " + k);
				}
				else
				{
					tempType = Floortype.Wall;
					Debug.LogError("图画错了" + colors[k] + " " + wallColor + " " + k);
				}

				fd[i, j] = new FloorDetail((int)tempType, i, j);
				if (fd[i, j].floorType == Floortype.Fire)
				{
					fd[i, j].fireEff = GameEffectManager.Instance.AddWorldEffect("Fire", new Vector3(i + 0.5f, 0, j+0.5f), 1, -1);
				}
				else if (fd[i, j].floorType == Floortype.Water)
				{
					fd[i, j].waterEff = GameEffectManager.Instance.AddWorldEffect("Water", new Vector3(i + 0.5f, 0, j + 0.5f), 0.5f, -1);
				}
				k++;
			}
		}
	}

	private void Update()
	{
		if (!isStarted)
		{
			return;
		}
		for (int i = 0; i < floorRow; i++)
		{
			for (int j = 0; j < floorColume; j++)
			{
				fd[i, j].AutoWarming();
				fd[i, j].StartFiring();
			}
		}
	}


	public FloorDetail[] CheckFloorAround(int i, int j)
	{
		List<FloorDetail> floors = new List<FloorDetail>();
		floors.Add(fd[i - 1, j]);
		floors.Add(fd[i + 1, j]);
		floors.Add(fd[i, j - 1]);
		floors.Add(fd[i, j + 1]);
		return floors.ToArray();
	}

	//public FloorDetail[] FloorsToExtinguish(int i, int j)
	//{
	//	List<FloorDetail> floors = new List<FloorDetail>();
	//	floors.Add(fd[i]);
	//}
}


public class FloorDetail
{
	public int posX;
	public int posY;
	private float smokeLevel;
	public float SmokeLevel
	{
		get
		{
			return smokeLevel;
		}
		set
		{
			smokeLevel = value;
		}
	}
	private float wetLevel;
	public float WetLevel
	{
		get
		{
			return wetLevel;
		}
		set
		{
			wetLevel = value;
			if (value < 100)
			{
				floorType = Floortype.Water;
			}
			else if (value < 200)
			{
				floorType = Floortype.None;
			}
			else
			{
				floorType = Floortype.Fire;
			}
		}
	}
	public GameEffect smokeEff;
	public GameEffect fireEff;
	public GameEffect waterEff;
	public bool isFiring = false;
	public bool isSmoking = false;
	public bool isWatering = false;
	public Floortype floorType = Floortype.None;

	private float speed = 10;

	public FloorDetail(int _floorType, int _posX, int _posY)
	{
		floorType = (Floortype)_floorType;
		posX = _posX;
		posY = _posY;
		if (_floorType == 0)
		{
			SmokeLevel = 150;
			WetLevel = 150;
		}
		else if (_floorType == 1)
		{
			SmokeLevel = 250F;
			WetLevel = 250F;
			isFiring = true;
		}
		else if (_floorType == 2)
		{
			SmokeLevel = 0;
			WetLevel = 0;
			isWatering = true;
		}
	}

	public void StartSmoking()
	{
		if (floorType != Floortype.Wall && floorType != Floortype.Fire)
		{
			if (SmokeLevel < 300)
			{
				SmokeLevel = SmokeLevel + speed * Time.deltaTime;
			}
			if (SmokeLevel > 100 && SmokeLevel < 200 && !isSmoking)
			{
				smokeEff = GameEffectManager.Instance.AddWorldEffect("VFX_Smoke", new Vector3(posX, 0, posY), 0.4f, -1);
				isSmoking = true;
			}
		}
	}
	public void Extinguish()
	{
		if (WetLevel > 0)
		{
			WetLevel = WetLevel - speed * Time.deltaTime * 10;
			if (WetLevel < 200)
			{
				if (isFiring)
				{
					fireEff.Die();
					isFiring = false;
					SmokeLevel = 150;
					smokeEff = GameEffectManager.Instance.AddWorldEffect("VFX_Smoke", new Vector3(posX, 0, posY), 0.4f, -1);
					isSmoking = true;
				}
			}
		}
		if (WetLevel < 100)
		{
			if (isSmoking)
			{
				smokeEff.Die();
				//smokeEff = null;
				isSmoking = false;
				SmokeLevel = 0;
			}
		}
	}

	public void AutoWarming()
	{
		if (WetLevel <= 100 && floorType != Floortype.Wall)
		{
			WetLevel = WetLevel + speed * Time.deltaTime;
		}
		else if (isWatering == true)
		{
			isWatering = false;
			waterEff.Die();
		}
	}
	public void StartFiring()
	{
		if (SmokeLevel >= 200)
		{
			if (WetLevel < 300)
			{
				WetLevel = WetLevel + speed * Time.deltaTime;
			}
			if (WetLevel >= 200 && isFiring == false)
			{
				fireEff = GameEffectManager.Instance.AddWorldEffect("Fire", new Vector3(posX, 0, posY), 1, -1);
				isFiring = true;
				if (isSmoking == true)
				{
					smokeEff.Die();
					isSmoking = false;
				}
			}
			if (floorType == Floortype.Fire)
			{
				foreach (var item in FloorManager.Instance.CheckFloorAround(posX, posY))
				{
					item.StartSmoking();
				}
			}
		}
	}
}

public enum Floortype
{
	None = 0,
	Fire = 1,
	Water = 2,
	Wall = 3
}
