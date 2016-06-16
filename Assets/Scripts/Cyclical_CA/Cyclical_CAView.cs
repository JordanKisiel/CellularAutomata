using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Cyclical_CAModel))]
public class Cyclical_CAView : MonoBehaviour {

	public float cellBuffer;
	public float cellSize;
	public GameObject cellRepresentation;
	public Color[] colors;

	private Cyclical_CAModel caModel;

	// Use this for initialization
	void Start () {
		caModel = gameObject.GetComponent<Cyclical_CAModel>();

		for(int i = 0; i < caModel.cellsDimensionX; i++){
			GameObject cellRow = new GameObject();

			cellRow.transform.parent = gameObject.transform;
			cellRow.name = "Cell Row " + i;

			for(int j = 0; j < caModel.cellsDimensionY; j++){
				GameObject cell = Instantiate(cellRepresentation, Vector3.zero, Quaternion.identity) as GameObject;

				cell.transform.parent = cellRow.transform;
				cell.name = "Cell " + j;

				float xPos = j + (float)(cellSize/2) + (j * cellBuffer);
				cell.transform.position = new Vector2(xPos, 0);
			}

			float rowPosY = i + (float)(cellSize + (i * cellBuffer));
			cellRow.transform.position = new Vector2(0, rowPosY);
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < caModel.cellsDimensionX; i++) {
			for (int j = 0; j < caModel.cellsDimensionY; j++) {
				gameObject.transform.GetChild(i).GetChild(j).gameObject.GetComponent<SpriteRenderer>().color = colors[caModel.GetCell(i, j)];
			}
		}
	}
}
