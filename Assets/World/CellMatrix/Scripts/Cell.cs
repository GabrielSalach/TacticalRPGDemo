using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cell : ISelectable
{
    // Which mode the cell is in (changes color)
    public enum CellMode {
        baseMode,
        rangeMode,
        attackMode
    }

    // Cell gameObject attached to the script
    GameObject cell;

    // has anything on top of the cell (not implemented)
    // Coordinates of the cell in the grid 
    Vector2Int cellCoords;
    
    // Cellmode
    CellMode cellMode;

    // Material of the cell
    Material cellMaterial;


    // Path Highlight
    SpriteRenderer pathRenderer;
    static Cell lastCellHovered;
    public PathHighlighter.Cardinal cardinalDirection;

    // Debug mode
    bool debugMode;


    // Constructor
    public Cell(int x, int y, GameObject cell) {
        // Assigns variables
        cellCoords = new Vector2Int(x, y);
        this.cell = cell;
        cell.GetComponent<CellWrapper>().cell = this;
        cellMaterial = cell.GetComponent<MeshRenderer>().material;

        // Used to determine if a unit is on the cell
        GetUnitOnTop();
        // Assign cellmode
        cellMode = CellMode.baseMode;
        // Assign Path renderer
        pathRenderer = cell.transform.Find("Path").GetComponent<SpriteRenderer>();

        // Debug Mode
        debugMode = false;
    }

    // Returns the coords of the cell in the grid 
    public Vector2Int GetCellCoords() {
        return cellCoords;
    }


    // Changes the cell mode and its color
    public void ChangeCellMode(CellMode cellMode) {
        this.cellMode = cellMode;
        Color color = cell.transform.parent.GetComponent<CellMatrixWrapper>().baseColor;
		switch(cellMode) {
            case CellMode.baseMode:
                color = cell.transform.parent.GetComponent<CellMatrixWrapper>().baseColor;
                break;
            case CellMode.rangeMode:
                color = cell.transform.parent.GetComponent<CellMatrixWrapper>().movementColor;
                break;
            case CellMode.attackMode:
                color = cell.transform.parent.GetComponent<CellMatrixWrapper>().attackColor;
                break;
            default:
                break;
        }
        if(debugMode == false) {
		    cellMaterial.SetColor("_Color", color);
        }
    }

    public CellMode GetCellMode() {
        return cellMode;
    }

    // When the cursor is above the cell 
	public void OnCursorEnter()
	{   
        // Calls OnCursorEnter on the unit if there's one on top
        if(GetUnitOnTop() != null) {
            GetUnitOnTop().OnCursorEnter();
        }
        // Sets the cell color to the hovered cell color
        if(debugMode == false) {
		    cellMaterial.SetColor("_Color", cell.transform.parent.GetComponent<CellMatrixWrapper>().selectedColor);
        }
        // If the CombatManager is in MoveUnitMode, Highlights the path from the selected unit to the current cell.
        if(CombatManager.instance.currentSubPhase == CombatManager.SubPhase.MoveUnitMode) {
            // Clears the path if it's a non walkable cell
            if(cellMode == CellMode.baseMode) {
                lastCellHovered = null;
                PathHighlighter.instance.ClearPath();
                CellMatrix.instance.pathfinder.ClearPath();
            }
            // Updates the path if it's a walkable cell
            if(cellMode == CellMode.rangeMode) {
                lastCellHovered = this;
                PathHighlighter.instance.ClearPath();
                PathHighlighter.instance.HighlightPath(CellMatrix.instance.pathfinder.GetPath(CellMatrix.instance.matrix[CombatManager.instance.selectedUnit.cellCoords.x, CombatManager.instance.selectedUnit.cellCoords.y], this));
            }
            if(cellMode == CellMode.attackMode) {
                // Gets all the valid positions for attacking this cell
                Pathfinder.DijkstraData dijkstraData = CellMatrix.instance.pathfinder.Dijkstra(this);
                List<Cell> validPositions = new List<Cell>();
                foreach(Cell cell in dijkstraData.cells) {
                    if(cell.GetCellMode() == CellMode.rangeMode && dijkstraData.distances[cell] <= CombatManager.instance.selectedUnit.attackRange) {
                        validPositions.Add(cell);
                    }
                }
                // If the last hovered cell is contained in the valid positions, keep it, otherwise select a new one 
                if(validPositions.Contains(lastCellHovered) == false) {
                    if(validPositions.Count > 0) {
                        lastCellHovered = validPositions[0];
                    } else {
                        Debug.Log("No valid path");
                    }
                }
                // Updates the path and the arrow
                PathHighlighter.instance.ClearPath();
                PathHighlighter.instance.HighlightPath(CellMatrix.instance.pathfinder.GetPath(CellMatrix.instance.matrix[CombatManager.instance.selectedUnit.cellCoords.x, CombatManager.instance.selectedUnit.cellCoords.y], lastCellHovered));
                PathHighlighter.instance.UpdateArrow(this, lastCellHovered);
            }
        }
	}

    // When the cursor isn't above the cell 
	public void OnCursorExit()
	{
        // Calls OnCursorExit on the unit if there's one on top
        if(GetUnitOnTop() != null) {
            GetUnitOnTop().OnCursorExit();
        }
        // Resets the cell color back to where it was
        Color color = cell.transform.parent.GetComponent<CellMatrixWrapper>().baseColor;
		switch(cellMode) {
            case CellMode.baseMode:
                color = cell.transform.parent.GetComponent<CellMatrixWrapper>().baseColor;
                break;
            case CellMode.rangeMode:
                color = cell.transform.parent.GetComponent<CellMatrixWrapper>().movementColor;
                break;
            case CellMode.attackMode:
                color = cell.transform.parent.GetComponent<CellMatrixWrapper>().attackColor;
                break;
            default:
                break;
        }
        if(debugMode == false) {
		    cellMaterial.SetColor("_Color", color);
        }
	}

    // When the user clicks on the cell 
	public void OnSelect()
	{
        // Check if the combat manager has a unit selected and is in moveUnitMode
        Unit selectedUnit = CombatManager.instance.selectedUnit;
        if(selectedUnit != null) {
            if(CombatManager.instance.currentSubPhase == CombatManager.SubPhase.MoveUnitMode) {
                if(cellMode == CellMode.rangeMode) {
                    // Clears the path arrows
                    List<Cell> path = CellMatrix.instance.pathfinder.path;
                    if(path != null) {
                        foreach(Cell cell in path) {
                            cell.ClearPath();
                        }
                    }

                    // Updates the position of the unit
                    selectedUnit.transform.position = cell.transform.position + new Vector3(0,0.4f,0);
                    selectedUnit.cellCoords = cellCoords;
                    GetUnitOnTop();

                    // Displays the menu and attack range.
                    CombatManager.instance.ChangeSubPhase(CombatManager.SubPhase.MenuPromptMode);
                    CellMatrix.instance.pathfinder.InitializeDijkstra(this);
                    CellMatrix.instance.ClearCellModes();
                    CellMatrix.instance.HighlightRange(0, selectedUnit.attackRange);
                    ContextualMenu.instance.InitializeMenu(selectedUnit);
                    ContextualMenu.instance.ShowMenu();
                }
            } else if(CombatManager.instance.currentSubPhase == CombatManager.SubPhase.AttackUnitMode) {
                selectedUnit.OnDeselect();
            }
        } else if(GetUnitOnTop() != null) {
            GetUnitOnTop().OnSelect();
        }
	}

    // When the user deselects the cell 
	public void OnDeselect()
	{
		throw new System.NotImplementedException();
	}

    // Makes the cell appear
    public void EnableCell() {
        cell.GetComponent<MeshRenderer>().enabled = true;
    }

    // Makes the cell disappear
    public void DisableCell() {
        cell.GetComponent<MeshRenderer>().enabled = false;
    }

    // Safely destroys the cell gameObject
    public void DestroyCell() {
        if(Application.isEditor) {
            GameObject.DestroyImmediate(cell);
        } else {
            GameObject.Destroy(cell);
        }
    }

    public Unit GetUnitOnTop() {
        Unit unit = null;
        Ray ray = new Ray(cell.transform.position + new Vector3(0, -0.5f, 0), Vector3.up);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("UnitRaycast"))) {
            if(hit.transform.CompareTag("Unit")) {
                unit= hit.transform.GetComponent<Unit>();
                unit.cellCoords = cellCoords;
            }
        }
        return unit;
    }

    public void HighlightPath(Sprite sprite, float orientation) {
        pathRenderer.sprite = sprite;
        pathRenderer.transform.localRotation = Quaternion.identity;
        pathRenderer.transform.Rotate(new Vector3(0, 0, orientation), Space.Self);
    }

    public void ChangePath(Sprite sprite) {
        pathRenderer.sprite = sprite;
    }

    public void ClearPath() {
        pathRenderer.sprite = null;
        pathRenderer.transform.Rotate(Vector3.zero, Space.Self);
    }

    public List<Cell> GetNeighbors() {
        List<Cell> neighbors = new List<Cell>();
        if(cellCoords.x > 0) {
            neighbors.Add(CellMatrix.instance.matrix[cellCoords.x - 1, cellCoords.y]);
        }
        if(cellCoords.y > 0) {
            neighbors.Add(CellMatrix.instance.matrix[cellCoords.x, cellCoords.y - 1]);
        }
        if(cellCoords.x < CellMatrix.instance.matrix.GetLength(0) - 1) {
            neighbors.Add(CellMatrix.instance.matrix[cellCoords.x + 1, cellCoords.y]);
        }
        if(cellCoords.y < CellMatrix.instance.matrix.GetLength(1) - 1) {
            neighbors.Add(CellMatrix.instance.matrix[cellCoords.x, cellCoords.y + 1]);
        }
        return neighbors;
    }

    public override string ToString() {
        return "x : " + cellCoords.x + " | y : " + cellCoords.y;
    }

    public void DebugMode(bool state) {
        if(state == true) {
            cellMaterial.SetColor("_Color", cell.transform.parent.GetComponent<CellMatrixWrapper>().debugColor);
        } else {
            // Resets the cell color back to where it was
            Color color = cell.transform.parent.GetComponent<CellMatrixWrapper>().baseColor;
            switch(cellMode) {
                case CellMode.baseMode:
                    color = cell.transform.parent.GetComponent<CellMatrixWrapper>().baseColor;
                    break;
                case CellMode.rangeMode:
                    color = cell.transform.parent.GetComponent<CellMatrixWrapper>().movementColor;
                    break;
                case CellMode.attackMode:
                    color = cell.transform.parent.GetComponent<CellMatrixWrapper>().attackColor;
                    break;
                default:
                    break;
            }
            cellMaterial.SetColor("_Color", color);
        }
    }
}
