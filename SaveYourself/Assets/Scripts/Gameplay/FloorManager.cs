using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : Singleton<FloorManager> {

	public int floorRow = 5;
	public int floorColume = 5;
	public FloorDetail[,] fd;

	private float speed = 30;

	private void Start()
	{
		//MapData mapdata = new MapData();
		int[] data = new int[floorColume * floorRow];
		int k=0;
		for (int i = 0; i < data.Length; i++)
		{
			//data[i] = Random.Range(0, 3);
			data[i] = 1;
		}

		//Initialize Floor
		fd = new FloorDetail[floorRow, floorColume];
		for (int i = 0; i < floorRow; i++)
		{
			for (int j = 0; j < floorColume; j++)
			{
				if (i == 0 || j == 0 || i == floorRow - 1 || j == floorColume - 1)
				{
					fd[i, j] = new FloorDetail(3, i, j);
				}
				else
				{
					fd[i, j] = new FloorDetail(data[k++], i, j);
					if (fd[i, j].floorType == Floortype.Fire)
					{
						fd[i, j].fireEff = GameEffectManager.Instance.AddWorldEffect("Fire", new Vector3(i, 0, j), 1, -1);
					}else if (fd[i, j].floorType == Floortype.Water)
					{
						fd[i, j].waterEff = GameEffectManager.Instance.AddWorldEffect("Water", new Vector3(i, 0, j), 0.5f, -1);
					}
				}
			}
		}
	}

	private void Update()
	{
		for (int i = 0; i < floorRow; i++)
		{
			for (int j = 0; j < floorColume; j++)
			{
				if (fd[i, j].WetLevel <= 100 && fd[i, j].floorType != Floortype.Wall)
				{
					fd[i, j].WetLevel = fd[i, j].WetLevel + speed * Time.deltaTime;
				}
				else if (fd[i, j].isWatering == true)
				{
					fd[i, j].isWatering = false;
					fd[i, j].waterEff.Die();
				}

				if(fd[i, j].floorType != Floortype.Wall)
				{
					print(fd[i, j].SmokeLevel);
				}
				
				if (fd[i, j].SmokeLevel >= 200)
				{
					if(fd[i,j].WetLevel < 300)
					{
						fd[i, j].WetLevel = fd[i, j].WetLevel + speed * Time.deltaTime;
					}					
					if(fd[i, j].WetLevel >= 200 && fd[i, j].isFiring == false)
					{
						fd[i, j].fireEff = GameEffectManager.Instance.AddWorldEffect("Fire", new Vector3(i, 0, j), 1, -1);
						fd[i, j].isFiring = true;
						if (fd[i, j].isSmoking == true)
						{
							fd[i, j].smokeEff.Die();
							fd[i, j].isSmoking = false;
						}
					}
					if(fd[i, j].floorType == Floortype.Fire)
					{
						foreach (var item in CheckFloorAround(i, j))
						{
							item.StartSmoking();			
						}
					}
				}
			}
		}
	}


	private FloorDetail[] CheckFloorAround(int i, int j)
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
public class MapData
{
	public int[] data;
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

	private float speed = 30;

	public FloorDetail(int _floorType, int _posX, int _posY)
	{
		floorType = (Floortype)_floorType;
		posX = _posX;
		posY = _posY;
		if(_floorType == 0)
		{
			SmokeLevel = 150;
			WetLevel = 150;
		}else if(_floorType == 1)
		{
			SmokeLevel = 250F;
			WetLevel = 250F;
			isFiring = true;
		}else if(_floorType == 2)
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
			if(SmokeLevel < 300)
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
		if(WetLevel > 0)
		{
			WetLevel = WetLevel - speed * Time.deltaTime * 4;
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
}

public enum Floortype
{
	None = 0,
	Fire = 1,
	Water = 2,
	Wall = 3
}
