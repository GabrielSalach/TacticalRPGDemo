using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Unit : MonoBehaviour, ISelectable
{
    public int movementRange;
    public int attackRange;


    [SerializeField] CellMatrixWrapper CellMatrix;
    protected CellMatrix cellMatrix;

    public Vector2Int cellCoords;

    protected Outline outliner;

    void Awake() {
        outliner = GetComponent<Outline>();
        outliner.enabled = false;
    }

    protected virtual void Start() {
        cellMatrix = CellMatrix.GetCellMatrix();
    }

	public virtual void OnCursorEnter()
	{
	}

	public virtual void OnCursorExit()
	{
	}

	public virtual void OnSelect()
	{
	}

	public virtual void OnDeselect()
	{
	}

}
