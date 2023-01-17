using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit, ISelectable
{
	protected override void Start()
	{
		base.Start();
	}

	public override void OnSelect()
	{
		base.OnSelect();
        outliner.enabled = true;
		cellMatrix.pathfinder.InitializeDijkstra(cellMatrix.matrix[cellCoords.x, cellCoords.y]);
        cellMatrix.HighlightRange(movementRange, attackRange);
		CombatManager.instance.selectedUnit = this;
		CombatManager.instance.ChangeSubPhase(CombatManager.SubPhase.MoveUnitMode);
/*         ContextualMenu.instance.InitializeMenu(this);
        ContextualMenu.instance.ShowMenu(); */
	}

	public override void OnDeselect()
	{
		base.OnDeselect();
        cellMatrix.ClearCellModes();
        outliner.enabled = false;
        CombatManager.instance.ChangeSubPhase(CombatManager.SubPhase.UnitSelectionMode);
		CombatManager.instance.selectedUnit = null;
	}
}
