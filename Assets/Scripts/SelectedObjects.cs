using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.Assertions;

public class SelectedObjects : MonoBehaviour
{
    public static List<Selectable> Selectables= new();
    public struct HighlightableWithUpdater
    {
        private HashSet<Character> Items;
        private Action<Character> onClearAction;
        public HighlightableWithUpdater(Action<Selectable> onClearAction)//fuck you C#, I shouldn't have to put parameters here
        {
            this.onClearAction = onClearAction;
            Items = new HashSet<Character>();
        }
        public void Add(Character item) 
        {
            Assert.IsNotNull(item);
            Items.Add(item); 
        }
        public void Remove(Character item) 
        {
            Assert.IsNotNull(item);
            Assert.IsFalse(!Items.Contains(item));
            Items.Remove(item); 
        }
        public void Clear()
        {
            if (Items.Count > 0)
            {
                foreach (var v in Items)
                {
                    onClearAction.Invoke(v);
                    if (Items.Count == 0) break;
                }
                Items.Clear();
            }     
        }
        public bool Contains(Character item)
        {
            return Items.Contains(item);
        }
        public HashSet<Character> GetItems() { return Items; }
        //public static implicit operator HashSet<Selectable>(HighlightableWithUpdater selectable) => selectable.Items;
    }
    public static SelectedObjects Instance;
    public HighlightableWithUpdater Highlighted = new(Selectable =>
    {
        Selectable.IsHighlighted = false;
    });
    public HighlightableWithUpdater Selected = new(Selectable =>
    {
        Selectable.IsSelected = false;
    });
    
    public void GiveOrder(Character.Order order, bool queue)
    {
        foreach(var chara in Selected.GetItems())
        {
            chara.ForceChangePath = queue;
            chara.AddOrder(order);
        }
    }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


}
