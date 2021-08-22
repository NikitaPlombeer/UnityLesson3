using UnityEngine;

public class ChessBoardGenerator : MonoBehaviour
{
    public GameObject CellPrefab;

    void Start()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                CreateCell(x, y);
            }
        }
    }

    private void CreateCell(int x, int y)
    {
        var chessBoardTransform = transform;
        Vector3 cellPosition = chessBoardTransform.position + Vector3.right * x + Vector3.forward * y;
        GameObject cell = Instantiate(CellPrefab, cellPosition, Quaternion.identity, chessBoardTransform);

        Color cellColor = Color.black;
        if ((x + y) % 2 == 0)
        {
            cellColor = Color.white;
        }

        cell.GetComponent<Renderer>().material.color = cellColor;
    }

}