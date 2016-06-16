using UnityEngine;
using System;
using System.Collections;

public class Cyclical_CAModel : MonoBehaviour {

	public int cellsDimensionX;
	public int cellsDimensionY;
	public int seed;
	public int numberOfValues;
	public float timeInterval;

	private int[,] cells;
	private System.Random numberGenerator;

	// Use this for initialization
	void Start () {
		cells = new int[cellsDimensionX, cellsDimensionY];

		numberGenerator = new System.Random(seed);
		GenerateRandomState(seed);

		InvokeRepeating("GenerateNextTimeStep", 0, timeInterval);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void GenerateRandomState(int seed){
		for(int i = 0; i < cellsDimensionX; i++){
			for(int j = 0; j < cellsDimensionY; j++){
				cells[i, j] = numberGenerator.Next(0, numberOfValues);
			}
		}
	}

	private void GenerateNextTimeStep(){
		int[,] cellsCopy = (int[,]) cells.Clone();

		for(int i = 0; i < cellsDimensionX; i++){
			for(int j = 0; j < cellsDimensionY; j++){
				int[] neighbors = GetCellNeighbors(i, j, cellsCopy);

				int selectedCell = cellsCopy[i, j];

				cells[i, j] = ApplyRules(selectedCell, neighbors);
			}
		}
	}

	private int ApplyRules(int selectedCell, int[] neighbors){
		int successorValue;
		if(selectedCell == numberOfValues - 1){
			successorValue = 0;
		}else{
			successorValue = selectedCell + 1;
		}

		for(int i = 0; i < neighbors.Length; i++){
			if(neighbors[i] == successorValue){
				return successorValue;
			}
		}

		return selectedCell;
	}

	//uses Moore neighborhood
	private int[] GetCellNeighbors(int xCoord, int yCoord, int[,] cells){
		int[] cellNeighbors = new int[8];

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

	public int GetCell(int xCoord, int yCoord){
		return cells[xCoord, yCoord];
	}
}
