using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private MazeCell _mazeCellPrefab;

    [SerializeField]
    private GameObject _endMarkerPrefab;

    [SerializeField]
    private GameObject _specialItemPrefab;
    [SerializeField]
    [Range(0f, 1f)]
    private float _specialItemSpawnChance = 0.2f;

    [SerializeField]
    private GameObject _extraItemPrefab1;
    [SerializeField]
    [Range(0f, 1f)]
    private float _extraItemSpawnChance1 = 0.15f;

    [SerializeField]
    private GameObject _extraItemPrefab2;
    [SerializeField]
    [Range(0f, 1f)]
    private float _extraItemSpawnChance2 = 0.1f;

    [SerializeField]
    private int _mazeWidth = 10;
    [SerializeField]
    private int _mazeDepth = 10;

    private MazeCell[,] _mazeGrid;
    private MazeCell _endCell;

    void Start()
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                // Instanzia la cella alla posizione (x, 0, z) senza modificare la scala
                MazeCell cell = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
                _mazeGrid[x, z] = cell;

                // Aggiungi la logica per non spawnare nella cella finale
                if (x != _mazeWidth - 1 || z != _mazeDepth - 1)
                {
                    // Spawn degli oggetti con posizione Y fissa
                    TryPlaceItem(_specialItemPrefab, _specialItemSpawnChance, cell, 0.2f);  // Posizione Y fissa
                    TryPlaceItem(_extraItemPrefab1, _extraItemSpawnChance1, cell, 0.2f);
                    TryPlaceItem(_extraItemPrefab2, _extraItemSpawnChance2, cell, 0.2f);
                }
            }
        }

        _endCell = _mazeGrid[_mazeWidth - 1, _mazeDepth - 1];
        PlaceEndMarker(_endCell);

        GenerateMaze(null, _mazeGrid[0, 0]);
    }

    private void PlaceEndMarker(MazeCell cell)
    {
        Vector3 cellPosition = cell.transform.position;
        Vector3 markerPosition = new Vector3(cellPosition.x, cellPosition.y + 1, cellPosition.z);

        Instantiate(_endMarkerPrefab, markerPosition, Quaternion.identity);
    }

    private void TryPlaceItem(GameObject itemPrefab, float spawnChance, MazeCell cell, float yPosition)
    {
        if (itemPrefab != null && Random.value < spawnChance && !cell.HasItem())
        {
            Vector3 cellPosition = cell.transform.position;
            // Usa un valore di Y fisso che hai fornito (in questo caso 0.2f)
            Vector3 itemPosition = new Vector3(cellPosition.x, yPosition, cellPosition.z);
            Instantiate(itemPrefab, itemPosition, Quaternion.identity);

            // Segna la cella come "con un oggetto" per evitare spawn multipli
            cell.MarkCellWithItem();
        }
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);
            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        if (x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];
            if (!cellToRight.IsVisited) yield return cellToRight;
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];
            if (!cellToLeft.IsVisited) yield return cellToLeft;
        }

        if (z + 1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x, z + 1];
            if (!cellToFront.IsVisited) yield return cellToFront;
        }

        if (z - 1 >= 0)
        {
            var cellToBack = _mazeGrid[x, z - 1];
            if (!cellToBack.IsVisited) yield return cellToBack;
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null) return;

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }
}
