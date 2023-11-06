using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    private bool _isSelected = false;
    public bool IsSelected
    {
        get { return _isSelected; }
        set
        {
            _isSelected = value;
            ShowHideObject.SetActive(_isSelected);
            if (_isSelected)
                SelectedObjects.Instance.Selected.Add(GetComponent<Character>());
            /*else
                SelectedObjects.Instance.Selected.Remove(this);*/
        }
    }
    private bool _isHighlighted = false;
    public bool IsHighlighted
    {
        get { return _isHighlighted; }
        set
        {
            _isHighlighted = value;
            Highlight.SetActive(_isHighlighted);
            if (_isHighlighted)
                SelectedObjects.Instance.Highlighted.Add(GetComponent<Character>());
            /*else
                SelectedObjects.Instance.Highlighted.Remove(this);*/

        }
    }

    public GameObject ShowHideObject;
    public GameObject Highlight;
}
   
