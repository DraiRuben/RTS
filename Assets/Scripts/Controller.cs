using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerMouse : MonoBehaviour
{
    private Selector selector;
    private bool ShiftIsHeld = false;
    private int tapcount = 0;
    private void Start()
    {
        selector = GetComponent<Selector>();
    }
    public void MousePosUpdate(InputAction.CallbackContext ctx)
    {
        selector.TryHighlight(Camera.main.ScreenToViewportPoint(ctx.ReadValue<Vector2>()));
    }
    public void GiveOrder(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Physics.Raycast(Camera.main.ScreenPointToRay(ctx.ReadValue<Vector2>()), out var Hit);
            Character.Order order = new Character.Order(Character.OrderType.Move, Hit.point);
            SelectedObjects.Instance.GiveOrder(order, ShiftIsHeld );
        }
    }
    public void Ctrl(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) selector.CtrlIsHeld = true;
        else if(ctx.canceled) selector.CtrlIsHeld = false;
    }
    public void Shift(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) { ShiftIsHeld = true; selector.ShiftIsHeld = true; }
        else if (ctx.canceled) { ShiftIsHeld = false; selector.ShiftIsHeld = false; }
    }
    public void MouseClick(InputAction.CallbackContext ctx) 
    {
        
        if(ctx.interaction is MultiTapInteraction)
            tapcount = (int)ctx.ReadValue<float>();
        if (ctx.started)
        {
            if(ctx.interaction is SlowTapInteraction)
                selector.TrySelectBulk(Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue()));
            if (tapcount == 1)
                selector.TrySelect(Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue()));
        }
        else if (ctx.performed)
        {
            selector.StopBulkSelect();
            if (ctx.interaction is MultiTapInteraction)
            {
                selector.TrySelectAll(Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue()));
            }
            

        }
        else if(ctx.canceled)
        {
            if(ctx.interaction is SlowTapInteraction)
            selector.StopBulkSelect();
        }
    }
}
