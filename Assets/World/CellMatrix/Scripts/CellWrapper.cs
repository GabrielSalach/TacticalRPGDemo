using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellWrapper : MonoBehaviour, ISelectable 
{
    public Cell cell;

	public void OnCursorEnter()
	{
        cell.OnCursorEnter();
	}

	public void OnCursorExit()
	{
        cell.OnCursorExit();
	}

	public void OnDeselect()
	{
		throw new System.NotImplementedException();
	}

	public void OnSelect()
	{
        cell.OnSelect();
	}
}
