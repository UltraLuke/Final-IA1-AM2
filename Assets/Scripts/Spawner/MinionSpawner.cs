using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    [Header("Objects")]
    public GameObject minionReference;
    public Transform spawnParent;
    public Vector3 addRotation;

    [Header("Spawn Area")]
    [SerializeField] Vector2 area;
    [SerializeField] [Range(1, 3)] int rowQuantity;
    [SerializeField] [Range(1, 8)] int columnQuantity;

    private List<GameObject> minions = new List<GameObject>();
    private bool _parameterChanged;

    Vector2 _lastArea;
    int _lastRowQuantity;
    int _lastColumnQuantity;

    public int RowQuantity
    {
        get => rowQuantity;
        set
        {
            rowQuantity = value;
            rowQuantity = rowQuantity >= 1 ? rowQuantity : 1;
            _lastRowQuantity = rowQuantity;
            _parameterChanged = true;
        }
    }
    public int ColumnQuantity
    {
        get => columnQuantity;
        set
        {
            columnQuantity = value;
            columnQuantity = columnQuantity >= 1 ? columnQuantity : 1;
            _lastColumnQuantity = columnQuantity;
            _parameterChanged = true;
        }
    }
    private void OnValidate()
    {
        if (area != _lastArea)
        {
            area.x = area.x > 0 ? area.x : .1f;
            area.y = area.y > 0 ? area.y : .1f;
            _lastArea = area;
            _parameterChanged = true;
        }
        if (rowQuantity != _lastRowQuantity)
        {
            rowQuantity = rowQuantity >= 1 ? rowQuantity : 1;
            _lastRowQuantity = rowQuantity;
            _parameterChanged = true;
        }
        if (columnQuantity != _lastColumnQuantity)
        {
            columnQuantity = columnQuantity >= 1 ? columnQuantity : 1;
            _lastColumnQuantity = columnQuantity;
            _parameterChanged = true;
        }
    }
    private void Start()
    {
        _lastArea = area;
        _lastRowQuantity = rowQuantity;
        _lastColumnQuantity = columnQuantity;

        _parameterChanged = true;
    }
    private void Update()
    {
        if (minionReference != null && _parameterChanged)
        {
            RefreshList();
            CalculatePositions();
            _parameterChanged = false;
        }
    }
    private void RefreshList()
    {
        int totalQty = rowQuantity * columnQuantity;
        int difference;

        if (minions.Count < totalQty)
        {
            difference = totalQty - minions.Count;
            for (int i = 0; i < difference; i++)
            {
                var minion = Instantiate(minionReference, spawnParent);
                minions.Add(minion);
                minion.SetActive(true);
                minion.transform.Rotate(addRotation);
            }
        }
        else if (minions.Count > totalQty)
        {
            difference = minions.Count - totalQty;
            int lastIndex;
            for (int i = 0; i < difference; i++)
            {
                lastIndex = minions.Count - 1;

                if (minions[lastIndex] != null)
                {
                    Destroy(minions[lastIndex]);
                }

                minions.RemoveAt(lastIndex);
            }
        }
    }
    private void CalculatePositions()
    {
        float xPos, zPos;
        int rows = rowQuantity;
        int columns = columnQuantity;
        Vector3 originPoint = transform.position - new Vector3(area.x / 2, 0, area.y / 2);
        Debug.Log(originPoint);
        var rowDivision = area.y / (rows + 1);
        var columnDivision = area.x / (columns + 1);

        Vector3 position;

        int indexMinions = 0;
        for (int r = 0; r < rows; r++)
        {
            zPos = (r + 1) * rowDivision + originPoint.z;
            for (int c = 0; c < columns; c++)
            {
                xPos = (c + 1) * columnDivision + originPoint.x;
                position = new Vector3(xPos, originPoint.y, zPos);

                if (minions != null)
                    minions[indexMinions].transform.position = position;

                indexMinions++;
            }
        }
    }
}
