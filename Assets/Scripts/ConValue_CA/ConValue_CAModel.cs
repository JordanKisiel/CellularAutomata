using UnityEngine;
using System.Collections;
using System;

public class ConValue_CAModel : MonoBehaviour {

	public int cellsDimensionX;
	public int cellsDimensionY;
	public int seed;
	public enum FunctionType {SumAndTruncate, Dyadic, Tent, Custom};
	public FunctionType functionType;
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
				float[] neighbors = GetCellNeighbors(i, j, cellsCopy);

				float selectedCell = cellsCopy[i, j];

				cells[i, j] = ApplyRules(selectedCell, neighbors);
			}
		}
	}

	private float ApplyRules(float selectedCell, float[] neighbors){
		float average = selectedCell;

		for(int i = 0; i < neighbors.Length; i++){
			average += neighbors[i];
		}

		average = average / 9.0f;

		float value = 0;

		if(functionType == FunctionType.SumAndTruncate){
			value = SumAndTruncate(average);
		}else if(functionType == FunctionType.Dyadic){
			value = Dyadic(average);
		}else if(functionType == FunctionType.Tent){
			value = Tent(average);
		}else if(functionType == FunctionType.Custom){
			value = Custom(average);
		}

		return (float)System.Math.Round(value, precision);
	}

	private float SumAndTruncate(float average){
		double sum = average + addingConstant;

		float fractionalPortion = (float)(sum - System.Math.Truncate(sum));

		return fractionalPortion;
	}

	private float Dyadic(float average){
		if(average <= 0.5){
			return 2 * average;
		}

		return 2 * average - 1.0f;
	}

	private float Tent(float average){
		if(average <= 0.5){
			return 2 * average;
		}

		return 2 * (1 - average);
	}

	private float Custom(float average){
		if(average <= 0.33){
			return SumAndTruncate(average);
		}else if(average > 0.33 && average <= 0.67){
			return 2 * average - 1.0f;
		}

		return 2 * (1 - average);
	}

	//uses Moore neighborhood
	private float[] GetCellNeighbors(int xCoord, int yCoord, float[,] cells){
		float[] cellNeighbors = new float[8];

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

	public float GetCell(int xCoord, int yCoord){
		return cells[xCoord, yCoord];
	}
}
