using UnityEngine;
using System.Collections;

public class GOL_CAModel : MonoBehaviour {

	public int cellsDimensionX;
	public int cellsDimensionY;
	public int seed;
	[Range(0.01f, 0.99f)] public float percentLiving;
	public float timeInterval;

	private bool[,] cells;

	// Use this for initialization
	void Start () {
		cells = new bool[cellsDimensionX, cellsDimensionY];

		GenerateRandomState(seed);

		InvokeRepeating("GenerateNextTimeStep", 0, timeInterval);
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	private void GenerateRandomState(int seed){
		Random.seed = seed;

		for(int i = 0; i < cellsDimensionX; i++){
			for(int j = 0; j < cellsDimensionY; j++){
				cells[i, j] = Random.value <= percentLiving;
			}
		}
	}

	private void GenerateNextTimeStep(){
		bool[,] cellsCopy = (bool[,]) cells.Clone();

		for(int i = 0; i < cellsDimensionX; i++){
			for(int j = 0; j < cellsDimensionY; j++){
				bool[] neighbors = GetCellNeighbors(i, j, cellsCopy);

				int livingCount = 0;
				for(int x = 0; x < neighbors.Length; x++){
					if(neighbors[x] == true){
						livingCount += 1;
					}
				}

				bool selectedCell = cellsCopy[i, j];

				//underpopulation rule
				if(livingCount < 2 && selectedCell){
					cells[i, j] = false;
				}
				//survival
				else if((livingCount == 2 || livingCount == 3) && selectedCell){
					cells[i, j] = true;
				}
				//overpopulation rule
				else if(livingCount > 3 && selectedCell){
					cells[i, j] = false;
				}
				//birth rule
				else if(livingCount == 3 && !selectedCell){
					cells[i, j] = true;
				}
			}
		}
	}

	//uses Moore neighborhood
	private bool[] GetCellNeighbors(int xCoord, int yCoord, bool[,] cells){
		bool[] cellNeighbors = new bool[8];

		//west cell
		if(xCoord != 0){
			cellNeighbors[0] = cells[xCoord - 1, yCoord];
		}else{
			cellNeighbors[0] = cells[cellsDimensionX - 1, yCoord];
		}

		//east cell
		if(xCoord != cellsDimensionX - 1){
			cellNeighbors[1] = cells[xCoord + 1, yCoord];
		}else{
			cellNeighbors[1] = cells[0, yCoord];
		}

		//north cell
		if(yCoord != 0){
			cellNeighbors[2] = cells[xCoord, yCoord - 1];
		}else{
			cellNeighbors[2] = cells[xCoord, cellsDimensionY - 1];
		}

		//south cell
		if(yCoord != cellsDimensionY - 1){
			cellNeighbors[3] = cells[xCoord, yCoord + 1];
		}else{
			cellNeighbors[3] = cells[xCoord, 0];
		}

		//northwest cell
		if(xCoord != 0 && yCoord != 0){
			cellNeighbors[4] = cells[xCoord - 1, yCoord - 1];
		}else if(xCoord != 0){
			cellNeighbors[4] = cells[xCoord - 1, cellsDimensionY - 1];
		}else if(yCoord != 0){
			cellNeighbors[4] = cells[cellsDimensionX - 1, yCoord - 1];
		}else{
			cellNeighbors[4] = cells[cellsDimensionX - 1, cellsDimensionY - 1];
		}

		//northeast cell
		if(xCoord != cellsDimensionX - 1 && yCoord != 0){
			cellNeighbors[5] = cells[xCoord + 1, yCoord - 1];
		}else if(xCoord != cellsDimensionX - 1){
			cellNeighbors[5] = cells[xCoord + 1, cellsDimensionY - 1];
		}else if(yCoord != 0){
			cellNeighbors[5] = cells[0, yCoord - 1];
		}else{
			cellNeighbors[5] = cells[0, cellsDimensionY - 1];
		}

		//southeast cell
		if(xCoord != cellsDimensionX - 1 && yCoord != cellsDimensionY - 1){
			cellNeighbors[6] = cells[xCoord + 1, yCoord + 1];
		}else if(xCoord != cellsDimensionX - 1){
			cellNeighbors[6] = cells[xCoord + 1, 0];
		}else if(yCoord != cellsDimensionY - 1){
			cellNeighbors[6] = cells[0, yCoord + 1];
		}else{
			cellNeighbors[6] = cells[0, 0];
		}

		//southwest cell
		if(xCoord != 0 && yCoord != cellsDimensionY - 1){
			cellNeighbors[7] = cells[xCoord - 1, yCoord + 1];
		}else if(xCoord != 0){
			cellNeighbors[7] = cells[xCoord - 1, 0];
		}else if(yCoord != cellsDimensionY - 1){
			cellNeighbors[7] = cells[cellsDimensionX - 1, yCoord + 1];
		}else{
			cellNeighbors[7] = cells[cellsDimensionX - 1, 0];
		}

		return cellNeighbors;
	}

	public bool GetCell(int xCoord, int yCoord){
		return cells[xCoord, yCoord];
	}
}
