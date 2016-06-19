using UnityEngine;
using System.Collections;
using System;

public class ConValue_CAModel : MonoBehaviour {

	public int cellsDimensionX;
	public int cellsDimensionY;
	public int seed;
	public bool useExtendedNeighborhood;
	[Range(0.0f, 1.0f)]public float addingConstant;
	[Range(0, 7)]public int precision;
	public float timeInterval;

	private float[,] cells;

	// Use this for initialization
	void Start () {
		cells = new float[cellsDimensionX, cellsDimensionY];

		UnityEngine.Random.seed = seed;
		GenerateRandomState();

		InvokeRepeating("GenerateNextTimeStep", 0, timeInterval);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void GenerateRandomState(){
		for(int i = 0; i < cellsDimensionX; i++){
			for(int j = 0; j < cellsDimensionY; j++){
				cells[i, j] = UnityEngine.Random.value;
			}
		}
	}

	private void GenerateNextTimeStep(){
		float[,] cellsCopy = (float[,]) cells.Clone();

		for(int i = 0; i < cellsDimensionX; i++){
			for(int j = 0; j < cellsDimensionY; j++){
				Vector2[] neighbors = GetCellNeighborCoord(i, j);

				float[] neighborValues = GetCellNeighborValues(neighbors, cellsCopy);

				float selectedCell = cellsCopy[i, j];

				cells[i, j] = ApplyRules(selectedCell, neighborValues);
			}
		}
	}


	//TODO: try out linear equations to make sure they can produce interesting results
	private float ApplyRules(float selectedCell, float[] neighbors){
		float average = selectedCell;

		for(int i = 0; i < neighbors.Length; i++){
			average += neighbors[i];
		}

		average = average / 9.0f;

		double sum = average + addingConstant;

		float fractionalPortion = (float)(sum - System.Math.Truncate(sum));

		return (float)System.Math.Round(fractionalPortion, precision);
	}

	//uses Moore neighborhood
	private Vector2[] GetCellNeighborCoord(int xCoord, int yCoord){
		Vector2[] cellNeighborCoord;

		if(useExtendedNeighborhood){
			cellNeighborCoord = new Vector2[24];
		}else{
			cellNeighborCoord = new Vector2[8];
		}


		//west cell
		cellNeighborCoord[0] = new Vector2(xCoord - 1, yCoord);

		//east cell
		cellNeighborCoord[1] = new Vector2(xCoord + 1, yCoord);

		//north cell
		cellNeighborCoord[2] = new Vector2(xCoord, yCoord - 1);

		//south cell
		cellNeighborCoord[3] = new Vector2(xCoord, yCoord + 1);

		//northwest cell
		cellNeighborCoord[4] = new Vector2(xCoord - 1, yCoord - 1);

		//northeast cell
		cellNeighborCoord[5] = new Vector2(xCoord + 1, yCoord - 1);

		//southeast cell
		cellNeighborCoord[6] = new Vector2(xCoord + 1, yCoord + 1);

		//southwest cell
		cellNeighborCoord[7] = new Vector2(xCoord - 1, yCoord + 1);

		if(useExtendedNeighborhood){
			
			cellNeighborCoord[8] = new Vector2(xCoord - 2, yCoord - 2);

			//east east cell
			cellNeighborCoord[9] = new Vector2(xCoord - 1, yCoord - 2);

			//north north cell
			cellNeighborCoord[10] = new Vector2(xCoord, yCoord - 2);

			//south south cell
			cellNeighborCoord[11] = new Vector2(xCoord + 1, yCoord - 2);

			//northwest northwest cell
			cellNeighborCoord[12] = new Vector2(xCoord + 2, yCoord - 2);

			//northeast northeast cell
			cellNeighborCoord[13] = new Vector2(xCoord - 2, yCoord - 1);

			//southeast southeast cell
			cellNeighborCoord[14] = new Vector2(xCoord + 2, yCoord - 1);

			//southwest southwest cell
			cellNeighborCoord[15] = new Vector2(xCoord - 2, yCoord);

			//west northwest cell
			cellNeighborCoord[16] = new Vector2(xCoord + 2, yCoord);

			//east northeast cell
			cellNeighborCoord[17] = new Vector2(xCoord - 2, yCoord + 1);

			//north northwest cell
			cellNeighborCoord[18] = new Vector2(xCoord + 2, yCoord + 1);

			//south southwest cell
			cellNeighborCoord[19] = new Vector2(xCoord - 2, yCoord + 2);

			//northwest northwest cell
			cellNeighborCoord[20] = new Vector2(xCoord - 1, yCoord + 2);

			//northeast northeast cell
			cellNeighborCoord[21] = new Vector2(xCoord, yCoord + 2);

			//southeast southeast cell
			cellNeighborCoord[22] = new Vector2(xCoord + 1, yCoord + 2);

			//southwest southwest cell
			cellNeighborCoord[23] = new Vector2(xCoord + 2, yCoord + 2);
		}

		//wrap cell coordinates to create toroidal space
		for(int i = 0; i < cellNeighborCoord.Length; i++){

			if(cellNeighborCoord[i].x == -1){
				cellNeighborCoord[i] = new Vector2(cellsDimensionX - 1, cellNeighborCoord[i].y);
			}
			if(cellNeighborCoord[i].y == -1){
				cellNeighborCoord[i] = new Vector2(cellNeighborCoord[i].x, cellsDimensionY - 1);
			}
			if(cellNeighborCoord[i].x == -2){
				cellNeighborCoord[i] = new Vector2(cellsDimensionX - 2, cellNeighborCoord[i].y);
			}
			if(cellNeighborCoord[i].y == -2){
				cellNeighborCoord[i] = new Vector2(cellNeighborCoord[i].x, cellsDimensionY - 2);
			}
			if(cellNeighborCoord[i].x == cellsDimensionX){
				cellNeighborCoord[i] = new Vector2(0, cellNeighborCoord[i].y);
			}
			if(cellNeighborCoord[i].y == cellsDimensionY){
				cellNeighborCoord[i] = new Vector2(cellNeighborCoord[i].x, 0);
			}
			if(cellNeighborCoord[i].x == cellsDimensionX + 1){
				cellNeighborCoord[i] = new Vector2(1, cellNeighborCoord[i].y);
			}
			if(cellNeighborCoord[i].y == cellsDimensionY + 1){
				cellNeighborCoord[i] = new Vector2(cellNeighborCoord[i].x, 1);
			}
		}

		return cellNeighborCoord;
	}

	private float[] GetCellNeighborValues(Vector2[] coords, float[,] cells){
		float[] values = new float[coords.Length];

		for(int i = 0; i < coords.Length; i++){
			values[i] = cells[(int)coords[i].x, (int)coords[i].y];
		}

		return values;
	}

	public float GetCell(int xCoord, int yCoord){
		return cells[xCoord, yCoord];
	}
}
