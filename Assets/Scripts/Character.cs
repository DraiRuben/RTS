using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : Selectable
{
    private NavMeshAgent agent;
    private Queue<Order> Orders = new();
    private Coroutine ProcessOrders;
    public bool ForceChangePath = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    
    public struct Order
    {
        public Order(OrderType type, Vector3 position) => (Type, Position) = (type, position);
        public OrderType Type;
        public Vector3 Position;
    }
    public enum OrderType
    {
        Move,
        Attack
    }
    
    public void AddOrder(Order order)
    {
        if(ForceChangePath) Orders.Clear();

        Orders.Enqueue(order);
        ProcessOrders ??= StartCoroutine(CharacterObey());
    }
    private void OnDisable()
    {
        SelectedObjects.Selectables.Remove(this);
    }
    private void OnEnable()
    {
        SelectedObjects.Selectables.Add(this);
    }
    private IEnumerator CharacterObey()
    {
        int orderNumber = 1;
        while (orderNumber > 0)
        {
            if (!agent.hasPath || ForceChangePath) { agent.SetDestination(Orders.Peek().Position); ForceChangePath = false; }
            else if (agent.remainingDistance < 0.5f)
            {
                Orders.Dequeue();
                if (Orders.Count <= 0) break;

                orderNumber = Orders.Count;
                agent.SetDestination(Orders.Peek().Position);

            }
            yield return null;

        }
        ProcessOrders = null;
    }
}
