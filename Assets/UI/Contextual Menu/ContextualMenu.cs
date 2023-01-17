using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContextualMenu : MonoBehaviour
{
    public static ContextualMenu instance;

    [SerializeField] Button attackButton;
    [SerializeField] Button cancelButton;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }
        HideMenu();
    }

    public void InitializeMenu(Unit currentUnit) {
        attackButton.onClick.RemoveAllListeners();
        attackButton.onClick.AddListener(delegate {
            CombatManager.instance.ChangeSubPhase(CombatManager.SubPhase.AttackUnitMode);
            HideMenu();
        });

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(delegate {
            currentUnit.OnDeselect();
            HideMenu();
        });
    }

    public void ShowMenu() {
        attackButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);
    }

    public void HideMenu() {
        attackButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
    }
}
