using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    public Vector2 mousePosition;
    Camera mainCamera;

    ISelectable hoveringSelectable;

    void Awake() {
        mainCamera = Camera.main;
    } 

    public void OnMouseMovement(InputAction.CallbackContext context) {
        mousePosition = context.ReadValue<Vector2>();
    }

    public void OnMouseSelect(InputAction.CallbackContext context) {
        if(context.performed) {
            if(hoveringSelectable != null) {
                hoveringSelectable.OnSelect();
            }
        }
    }


    void FixedUpdate() {
        if(hoveringSelectable != null) {
            hoveringSelectable.OnCursorExit();
            hoveringSelectable = null;
        }


        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, ~LayerMask.GetMask("UnitRaycast"))) {
            Transform objectHit = hit.transform;
            if(objectHit.CompareTag("Selectable")) {
                if(hoveringSelectable != null && hoveringSelectable != objectHit.GetComponent<ISelectable>()) {
                    hoveringSelectable.OnCursorExit();
                }
                hoveringSelectable = objectHit.GetComponent<ISelectable>();
                hoveringSelectable.OnCursorEnter();
            } else {
                if(hoveringSelectable != null) {
                    hoveringSelectable.OnCursorExit();
                    hoveringSelectable = null;
                }
            }
        }        


    }

}
