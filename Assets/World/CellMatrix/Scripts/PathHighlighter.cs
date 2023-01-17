using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathHighlighter : MonoBehaviour
{
    public static PathHighlighter instance;

    void Awake() {
        instance = this;
    }

    public enum Cardinal{North, South, East, West}


    [SerializeField] Sprite line, arrow, turnLeft, turnRight, turnRightArrow, turnLeftArrow;
    Grid grid;

    public void HighlightPath(List<Cell> path) {
        if(path.Count > 1) {

            //Setup lastCardinalDirection for the first loop
            Cardinal lastMovementDirection = GetMovementDirection(path[0], path[1]);


            for(int i = 1; i < path.Count; i++) {
                // For each cell in the path except the one the unit is standing on
                Cell previousCell = path[i-1];
                Cell currentCell = path[i];

                float orientation = 0;

                Cardinal currentMovementDirection = GetMovementDirection(previousCell, currentCell);
                switch(currentMovementDirection) {
                    case Cardinal.North:
                        orientation = 0;
                        currentCell.cardinalDirection = Cardinal.North;
                        break;
                    case Cardinal.South:
                        currentCell.cardinalDirection = Cardinal.South;
                        orientation = 180;
                        break;
                    case Cardinal.East:
                        currentCell.cardinalDirection = Cardinal.East;
                        orientation = -90;
                        break;
                    case Cardinal.West:
                        currentCell.cardinalDirection = Cardinal.West;
                        orientation = 90;
                        break;
                    default:
                        break;
                }

                currentCell.HighlightPath(line, orientation);

                if(currentMovementDirection != lastMovementDirection) {
                    if((currentMovementDirection == Cardinal.East && lastMovementDirection == Cardinal.North) || (currentMovementDirection == Cardinal.South && lastMovementDirection == Cardinal.West)) {
                        previousCell.HighlightPath(turnRight, 0);
                    } else if((currentMovementDirection == Cardinal.West && lastMovementDirection == Cardinal.North) || (currentMovementDirection == Cardinal.South && lastMovementDirection == Cardinal.East)) {
                        previousCell.HighlightPath(turnLeft, 0);
                    } else if((currentMovementDirection == Cardinal.East && lastMovementDirection == Cardinal.South) || (currentMovementDirection == Cardinal.North && lastMovementDirection == Cardinal.West)) {
                        previousCell.HighlightPath(turnLeft, 180);
                    } else if((currentMovementDirection == Cardinal.West && lastMovementDirection == Cardinal.South) || (currentMovementDirection == Cardinal.North && lastMovementDirection == Cardinal.East)) {
                        previousCell.HighlightPath(turnRight, 180);
                    }
                }
                lastMovementDirection = currentMovementDirection;
            }
            path[path.Count - 1].ChangePath(arrow);
        }
    }

    Cardinal GetMovementDirection(Cell cell1, Cell cell2) {
            Cardinal direction;
            if(cell1.GetCellCoords().x == cell2.GetCellCoords().x) {
                if(cell1.GetCellCoords().y < cell2.GetCellCoords().y) {
                    direction = Cardinal.North;
                } else {
                    direction = Cardinal.South;
                }
            } else {
                if(cell1.GetCellCoords().x < cell2.GetCellCoords().x) {
                    direction = Cardinal.East;
                } else {
                    direction = Cardinal.West;
                }
            }
            return direction;
    }    

    public void UpdateArrow(Cell targetCell, Cell lastCellFromPath) {
        if(lastCellFromPath.GetCellCoords().x == targetCell.GetCellCoords().x) {
            // Same row
            if(lastCellFromPath.cardinalDirection == Cardinal.West && targetCell.GetCellCoords().y > lastCellFromPath.GetCellCoords().y) {
                lastCellFromPath.ChangePath(turnRightArrow);
            } else if(lastCellFromPath.cardinalDirection == Cardinal.East && targetCell.GetCellCoords().y > lastCellFromPath.GetCellCoords().y) {
                lastCellFromPath.ChangePath(turnLeftArrow);
            } else if(lastCellFromPath.cardinalDirection == Cardinal.West && targetCell.GetCellCoords().y < lastCellFromPath.GetCellCoords().y) {
                lastCellFromPath.ChangePath(turnLeftArrow);
            } else if(lastCellFromPath.cardinalDirection == Cardinal.East && targetCell.GetCellCoords().y < lastCellFromPath.GetCellCoords().y) {
                lastCellFromPath.ChangePath(turnRightArrow);
            }
            
        } 

        if(lastCellFromPath.GetCellCoords().y == targetCell.GetCellCoords().y) {
            // Same column
            if(lastCellFromPath.cardinalDirection == Cardinal.North && targetCell.GetCellCoords().x > lastCellFromPath.GetCellCoords().x) {
                lastCellFromPath.ChangePath(turnRightArrow);
            } else if(lastCellFromPath.cardinalDirection == Cardinal.South && targetCell.GetCellCoords().x > lastCellFromPath.GetCellCoords().x) {
                lastCellFromPath.ChangePath(turnLeftArrow);
            } else if(lastCellFromPath.cardinalDirection == Cardinal.North && targetCell.GetCellCoords().x < lastCellFromPath.GetCellCoords().x) {
                lastCellFromPath.ChangePath(turnLeftArrow);
            } else if(lastCellFromPath.cardinalDirection == Cardinal.South && targetCell.GetCellCoords().x < lastCellFromPath.GetCellCoords().x) {
                lastCellFromPath.ChangePath(turnRightArrow);
            }
        }

    }

    public void ClearPath() {
        List<Cell> path = CellMatrix.instance.pathfinder.path;
        if(path != null) {
            foreach(Cell cell in path) {
                cell.ClearPath();
            }
        }
    }
}
