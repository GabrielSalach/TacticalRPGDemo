using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;
    [SerializeField] CellMatrixWrapper cellMatrix;
    
    public enum CombatPhase {
        PlayerPhase,
        EnemyPhase
    }

    public enum SubPhase {
        UnitSelectionMode,
        MoveUnitMode,
        MenuPromptMode,
        AttackUnitMode
    }

    public Unit selectedUnit;

    int playerBaseActionPoints;
    int playerCurrentActionPoints;

    public CombatPhase currentPhase;
    public SubPhase currentSubPhase;
    
    public RectTransform crossedSwordsIcon;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

    }

    void InitializeCombat() {
        selectedUnit = null;
        playerBaseActionPoints = cellMatrix.GetCellMatrix().GetPlayerUnits().Count;
        playerCurrentActionPoints = playerBaseActionPoints;
        currentPhase = CombatPhase.PlayerPhase;
        currentSubPhase = SubPhase.UnitSelectionMode;
    }


    IEnumerator Combat() {
        // Player phase animation
        

        // Unit selection
        // Contextual menu appears
        // Move Mode/Attack Mode
        // Enemy Phase
        yield return null;
    }

    public void ChangeSubPhase(SubPhase newSubphase) {
        currentSubPhase = newSubphase;
    }
}
