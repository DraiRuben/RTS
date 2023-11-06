using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{
    private Vector2 MousePos;
    private Vector2 HoldInitialPos;
    public bool CtrlIsHeld = false;
    public bool ShiftIsHeld = false;
    [SerializeField]
    private RectTransform SelectWindow;
    [SerializeField]
    private RectTransform Canvas;
    public void TryHighlight(Vector2 _MousePos)
    {
        MousePos = _MousePos;
        Physics.Raycast(Camera.main.ViewportPointToRay(_MousePos), out var Hit);
        Character comp = Hit.collider?.GetComponent<Character>();
        if (comp != null && !SelectedObjects.Instance.Highlighted.Contains(comp))
        {
            SelectedObjects.Instance.Highlighted.Clear();
            comp.IsHighlighted = true;
        }
        else if(Hit.collider == null ||(Hit.collider!=null && comp == null))
            SelectedObjects.Instance.Highlighted.Clear();
        

    }
    public void TrySelect(Vector2 _MousePos)
    {
        HoldInitialPos = _MousePos;
        Physics.Raycast(Camera.main.ViewportPointToRay(_MousePos), out var Hit);
        Character comp = Hit.collider?.GetComponent<Character>();
        if (comp  != null && !SelectedObjects.Instance.Selected.Contains(comp))
        {
            //SelectedObjects.Instance.Highlighted.Clear();
            if(!ShiftIsHeld && !CtrlIsHeld)
                SelectedObjects.Instance.Selected.Clear();

            comp.IsSelected = true;
        }
        else if(Hit.collider == null || (comp == null))
        {
            if (!ShiftIsHeld && !CtrlIsHeld)
                SelectedObjects.Instance.Selected.Clear();
        }    
        else
        {
            SelectedObjects.Instance.Selected.Remove(comp);
            comp.IsSelected = false;
        }
            

    }
    public void TrySelectAll(Vector2 _MousePos)
    {
        HoldInitialPos = _MousePos;
        Physics.Raycast(Camera.main.ViewportPointToRay(_MousePos), out var Hit);
        Character comp = Hit.collider?.GetComponent<Character>();
        if (comp  != null)
        {
            //SelectedObjects.Instance.Highlighted.Clear();
            foreach (var item in SelectedObjects.Selectables) item.IsSelected = true;
        }    

    }
    public void TrySelectBulk(Vector2 _MousePos)
    {
        if(!(CtrlIsHeld || ShiftIsHeld))
            SelectedObjects.Instance.Selected.Clear();

        HoldInitialPos = _MousePos;
        StartCoroutine(EBulkSelectRoutine());
    }
    public void StopBulkSelect()
    {
        StopAllCoroutines();
        SelectWindow.gameObject.SetActive(false);
        if (SelectWindow.sizeDelta.magnitude > 180f)
        {
            if (SelectedObjects.Instance.Highlighted.GetItems().Count > 0)
            {
                if(CtrlIsHeld)
                    foreach (var v in SelectedObjects.Instance.Highlighted.GetItems())
                    {
                        v.IsSelected = false;
                    }
                else
                    foreach (var v in SelectedObjects.Instance.Highlighted.GetItems())
                    {
                        v.IsSelected = true;
                    }
                SelectedObjects.Instance.Highlighted.Clear();
            }
            
        }
    }
    private IEnumerator EBulkSelectRoutine()
    {
        SelectWindow.gameObject.SetActive(true);
        Vector2 ScreenHoldInitialPos = new(HoldInitialPos.x * Canvas.sizeDelta.x, HoldInitialPos.y * Canvas.sizeDelta.y);
        Vector2 ScreenMousePos = new(MousePos.x * Canvas.sizeDelta.x, MousePos.y * Canvas.sizeDelta.y);
        float w;
        float h;
        while(true)
        {
            ScreenMousePos = new(MousePos.x * Canvas.sizeDelta.x, MousePos.y * Canvas.sizeDelta.y);
            w = ScreenMousePos.x - ScreenHoldInitialPos.x;
            h = ScreenMousePos.y - ScreenHoldInitialPos.y;
            SelectWindow.anchoredPosition = ScreenHoldInitialPos + new Vector2(w / 2, h / 2);
            SelectWindow.sizeDelta = new(Mathf.Abs(w), Mathf.Abs(h));
            SelectedObjects.Instance.Highlighted.Clear();
           foreach(var s in SelectedObjects.Selectables)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(SelectWindow,Camera.main.WorldToScreenPoint(s.transform.position)))
                {
                    s.IsHighlighted = true;
                }
                else
                    s.IsHighlighted= false;
            }
            yield return null;
        }
    }
}
