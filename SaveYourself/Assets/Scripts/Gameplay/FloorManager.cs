using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour {

	public int floorRow = 5;
	public int floorColume = 5;
	public FloorDetail[,] fd;

	private void Start()
	{
		//MapData mapdata = new MapData();
		int[] data = new int[floorColume * floorRow];
		int k=0;
		for (int i = 0; i < data.Length; i++)
		{
			data[i] = Random.Range(0, 3);
		}

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
				if(fd[i,j].WetLevel <= -10)
				{
					fd[i, j].WetLevel = fd[i, j].WetLevel + 60 * Time.deltaTime;
				}
				
				if (fd[i, j].SmokeLevel >= 50)
				{
					if(fd[i,j].WetLevel < 100)
					{
						fd[i, j].WetLevel = fd[i, j].WetLevel + 1 * Time.deltaTime;
					}					
					if(fd[i, j].WetLevel >= 10 && fd[i, j].floorType == Floortype.None)
					{
						fd[i, j].fireEff = GameEffectManager.Instance.AddWorldEffect("Fire", new Vector3(i, 0, j), 1, -1);
						fd[i, j].floorType = Floortype.Fire;
					}
					if(fd[i, j].floorType == Floortype.Fire)
					{
						foreach (var item in CheckFloorAround(i, j))
						{
							item.StartSmoking();
							//if(item.floorType != Floortype.Wall && item.floorType != Floortype.Fire)
							//{
							//	item.SetSmoke(item.smokeLevel + 60 * Time.deltaTime);
							//	if(item.smokeLevel > 10 && !item.hasSmoke)
							//	{
							//		GameEffectManager.Instance.AddWorldEffect("VFX_Smoke", new Vector3(item.posX, 0, item.posY), 1, -1);
							//		item.hasSmoke = true;
							//	}
							//}					
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
			if (value < -10)
			{
				floorType = Floortype.Water;
			}
			else if (value < 10)
			{
				floorType = Floortype.None;
			}
		}
	}
	public GameEffect smokeEff;
	public GameEffect fireEff;
	public GameEffect waterEff;
	public Floortype floorType = Floortype.None;

	public FloorDetail(int _floorType, int _posX, int _posY)
	{
		floorType = (Floortype)_floorType;
		posX = _posX;
		posY = _posY;
		if(_floorType == 0)
		{
			SmokeLevel = 0;
			WetLevel = 0;
		}else if(_floorType == 1)
		{
			SmokeLevel = 50F;
			WetLevel = 10F;
		}else if(_floorType == 2)
		{
			SmokeLevel = 0;
			WetLevel = -100;
		}
	}

	public void StartSmoking()
	{
		if (floorType != Floortype.Wall && floorType != Floortype.Fire)
		{
			if(smokeLevel < 100)
			{
				SmokeLevel = smokeLevel + 10 * Time.deltaTime;
			}		
			if (smokeLevel > 10 && smokeLevel < 50 && smokeEff == null)
			{
				smokeEff = GameEffectManager.Instance.AddWorldEffect("VFX_Smoke", new Vector3(posX, 0, posY), 0.2f, -1);
			}else if (smokeLevel > 50 && smokeEff != null && fireEff == null)
			{
				//Destroy(smokeEff.gameObject);
				fireEff = GameEffectManager.Instance.AddWorldEffect("Fire", new Vector3(posX, 0, posY), 1, -1);
			}
		}
	}
	public void Extinguish()
	{
		if(smokeLevel > 0)
		{
			SmokeLevel = smokeLevel - 60 * Time.deltaTime;
		}
		if (smokeLevel < 50)
		{
			if (fireEff != null)
			{
				//Destroy(fireEff.gameObject);
				smokeEff = GameEffectManager.Instance.AddWorldEffect("VFX_Smoke", new Vector3(posX, 0, posY), 1, -1);
			}
		}
		if(smokeLevel < 10)
		{
			if (smokeEff != null)
			{
				//Destroy(smokeEff.gameObject);
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
