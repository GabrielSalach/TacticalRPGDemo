using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : Unit
{
    RectTransform crossedSwords;
    Camera mainCam;

	protected override void Start()
	{
		base.Start();
        crossedSwords = CombatManager.instance.crossedSwordsIcon;
        mainCam = Camera.main;
	}

	public override void OnCursorEnter()
	{
		base.OnCursorEnter();
        crossedSwords.gameObject.SetActive(true);

        Vector2 canvasPos;
        Vector2 swordPos = mainCam.WorldToScreenPoint(transform.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(crossedSwords.transform.parent.GetComponent<RectTransform>(), swordPos, null, out canvasPos);
        crossedSwords.localPosition = canvasPos;
	}

	public override void OnCursorExit()
	{
		base.OnCursorExit();
        crossedSwords.gameObject.SetActive(false);
	}
}
