using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Vector2Int
{
	public int x;
	public int y;

	public Vector2Int(int _x , int _y)
	{
		x = _x;
		y = _y;
	}

}

static class Dungeon 
{

	static public int[,] SpawnDungeon(Vector2Int size, int nbrSquare4Generation, float dpc , Vector2Int hallsSize)
	{
		int[,] DungeonMap; 
        	
		DungeonMap = CreateDungeonSquares(size.x , size.y, nbrSquare4Generation, dpc);
		int nbrRoom = FindRooms(DungeonMap, size.x,size.y);
        if (nbrRoom > 1)
        {
            DungeonMap = CreateDungeonHalls(DungeonMap, size.x, size.y, nbrRoom, hallsSize);
        }
        else
        {
            return SpawnDungeon(size, nbrSquare4Generation, dpc, hallsSize);
        }
		return DungeonMap;

	}
	static int[,] CreateDungeonSquares(int sizeX, int sizeZ, int squareNbr , float dpc)
	{
		int[,] newMap = new int[sizeX,sizeZ]; 

		for (int i = 0 ; i < squareNbr ; i++)
		{
			int roomSizeX = Random.Range(10, Mathf.RoundToInt(sizeX/dpc));
			int roomSizeZ = Random.Range(10, Mathf.RoundToInt(sizeZ/dpc));
			int roomPosX = Random.Range (2,sizeX-roomSizeX - 1 );
			int roomPosZ = Random.Range (2,sizeZ-roomSizeZ - 1 );

			for(int j = roomPosX ; j < roomPosX + roomSizeX; j++)
			{
				for(int k = roomPosZ ; k < roomPosZ + roomSizeZ; k++)
				{
					newMap[j,k] = 1;
				}
			}		
		}
		return newMap;
	}
	static int FindRooms(int[,] dungeonMap, int sizeX, int sizeZ)
	{

		List<Vector2> ListCoordToTest = new List<Vector2>(); 
		int[,] modifiedMap  = new int[sizeX,sizeZ]; 
		int    nbrRoomFound = 0;

		System.Array.Copy(dungeonMap,modifiedMap, sizeX*sizeZ);

		for(int i = 0; i < sizeX; i++)
		{
			for(int j = 0; j < sizeZ; j++)
			{
				if(modifiedMap[i,j] == 1)
				{
					ListCoordToTest.Add(new Vector2(i, j)); 
					while(ListCoordToTest.Count > 0) 
					{   
						int x = (int)ListCoordToTest[0].x; 
						int z = (int)ListCoordToTest[0].y; 
						ListCoordToTest.RemoveAt (0); 
						dungeonMap[x,z] = nbrRoomFound + 1; 
						for(int xAround = x - 1; xAround <= x + 1; xAround++)
						{
							for(int zAround = z - 1 ; zAround <= z + 1; zAround++)
							{
								if(modifiedMap[xAround,zAround] == 1)
								{
									ListCoordToTest.Add(new Vector2(xAround, zAround)); 
									modifiedMap[xAround,zAround] = 0; 
								}
							}
						}
					}
					nbrRoomFound++;
				}
			}
		}
		return nbrRoomFound;
	}
	static private int[,] CreateDungeonHalls(int[,] dungeonMap, int sizeX,int sizeZ, 
        int nbrRoom, Vector2Int hallSize)
	{
		int x1; 
		int z1; 
		int x2; 
		int z2; 

		// Start a corridor from each room
		for(int curRoomNbr = 1; curRoomNbr <= nbrRoom; curRoomNbr++)
		{
			int nbrRoomsTry = 0; 
			int nbrRoomsTryMax = 3000;

			x1 = Random.Range (1, sizeX-1);
			z1 = Random.Range (1, sizeZ-1);

			while(dungeonMap[x1,z1] != curRoomNbr && nbrRoomsTry < nbrRoomsTryMax)
			{
				x1 = Random.Range (1, sizeX-1);
				z1 = Random.Range (1, sizeZ-1);
				nbrRoomsTry++;
			}
			nbrRoomsTry = 0;

			x2 = Random.Range (1, sizeX-1);
			z2 = Random.Range (1, sizeZ-1);

			while((dungeonMap[x2,z2] == 0 || dungeonMap[x2,z2] == curRoomNbr) && nbrRoomsTry < nbrRoomsTryMax)
			{
				x2 = Random.Range (1, sizeX-1);
				z2 = Random.Range (1, sizeZ-1);
				nbrRoomsTry++;
			}

			int diffX = x2 - x1;
			int diffZ = z2 - z1;

			int xDirection = 1; 
			int zDirection = 1; 
            
			if(diffX != 0)
			{
				xDirection = diffX/Mathf.Abs(diffX); 
			}
			else
			{
				xDirection = 0; 
			}

			if(diffZ != 0)
			{
				zDirection = diffZ/Mathf.Abs(diffZ); 
			}
			else
			{
				zDirection = 0; 
			}

			int hallWidth  = Random.Range (hallSize.x,hallSize.y);
			int hallHeight = Random.Range (hallSize.x,hallSize.y);

			for(int i = x1; i != x1 + xDirection*hallWidth; i += xDirection)
			{
				for(int j = z1; j != z2; j += zDirection)
				{
					if(i >= 0 && i < sizeX && j >= 0 && j < +sizeZ)
					{
						if(dungeonMap[i,j] == 0) 
						{
							dungeonMap[i,j] = -1; 
						}
					}
				}
			}

			for(int i = x1; i != x2; i += xDirection)
			{
				for(int j = z2; j != z2 + zDirection*hallHeight; j += zDirection)
				{
					if(i >= 0 && i < sizeX && j >= 0 && j < +sizeZ) 
					{
						if(dungeonMap[i,j] == 0) 
						{
							dungeonMap[i,j] = -1; 
						}
					}
				}
			}
		}
		return dungeonMap; 
	}



}
