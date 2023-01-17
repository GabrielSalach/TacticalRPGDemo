using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMatrix
{
    public static CellMatrix instance;

    public Cell[,] matrix;
    GameObject cellPrefab;
    public Pathfinder pathfinder;

    int width, length;

    // Contructor
    public CellMatrix(int width, int length, Transform origin, GameObject cellPrefab) {
        // Generates a new matrix and Instantiate cells at their correct locations
        matrix = new Cell[width, length];
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < length; j++) {
                GameObject newCell = GameObject.Instantiate(cellPrefab, new Vector3(origin.position.x + i + 0.5f, 0.01f, origin.position.z + 0.5f + j), Quaternion.identity);
                newCell.transform.Rotate(Vector3.right, 90);
                newCell.transform.parent = origin;
                matrix[i,j] = new Cell(i, j, newCell);
            }
        }
        this.width = width;
        this.length = length;
        instance = this;
        pathfinder = new Pathfinder(this);
    }

    // Highlight cells in the movement range, centered at origin
    public void HighlightRange(int movementRange, int attackRange) {
        // Display Movement range
        List<Cell> availableCells = new List<Cell>();
        foreach(Cell cell in pathfinder.dijkstraData.cells) {
            if(pathfinder.dijkstraData.distances[cell] <= movementRange) {
                cell.ChangeCellMode(Cell.CellMode.rangeMode); 
                availableCells.Add(cell);
            } 
        }
        // Display Attack range
        for(int i = 0; i < attackRange; i++) {
            List<Cell> updatedCells = new List<Cell>();
            foreach(Cell cell in availableCells) {
                foreach(Cell neighbor in cell.GetNeighbors()) {
                    if(neighbor.GetCellMode() != Cell.CellMode.rangeMode) {
                        neighbor.ChangeCellMode(Cell.CellMode.attackMode);
                        updatedCells.Add(neighbor);
                    }
                }
            }
            availableCells = updatedCells;
        }
    }

    // Resets all the cells to baseMode
    public void ClearCellModes() {
        foreach(Cell cell in matrix) {
            cell.ChangeCellMode(Cell.CellMode.baseMode);
        }
    }

    ~CellMatrix() {
        foreach(Cell cell in matrix) {
            cell.DestroyCell();
        }
    }

    public List<Unit> GetPlayerUnits() {
        List<Unit> units = new List<Unit>();
        foreach(Cell cell in matrix) {
            Unit unit = cell.GetUnitOnTop();
            if(unit != null) {
                units.Add(unit);
            }
        }
        return units;
    }

    public List<Cell> GetCellNeighbors(Cell cell) {
        List<Cell> neighbors = new List<Cell>();
        for(int x = -1; x <= 1; x++) {
            for(int y = -1; y <= 1; y++) {
                if(x == 0 || y == 0) {
                    int checkX = cell.GetCellCoords().x + x;
                    int checkY = cell.GetCellCoords().y + y;

                    if(checkX >= 0 && checkX < width && checkY >= 0 && checkY < length) {
                        neighbors.Add(matrix[checkX, checkY]);
                    }
                }
            }
        }
        return neighbors;
    } 

}
