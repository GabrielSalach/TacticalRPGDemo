using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMatrixWrapper : MonoBehaviour
{
    CellMatrix cellMatrix;

    [SerializeField] int width, length;
    [SerializeField] GameObject cellPrefab;
    public Color baseColor, selectedColor, movementColor, attackColor, debugColor;

    void Awake() {
        cellMatrix = new CellMatrix(width, length, transform, cellPrefab);
    }

    public CellMatrix GetCellMatrix() {
        return cellMatrix;
    }
}
