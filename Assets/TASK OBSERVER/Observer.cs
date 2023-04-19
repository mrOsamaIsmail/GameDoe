using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Notification
{
    GROUND_HIT
}


public interface IObserver
{ 
    public void OnNotify(Notification _context);
}

public class Subject : MonoBehaviour
{
    private List<IObserver> _subscribers = new List<IObserver>();
    public void Subscribe(IObserver _obs) 
    {
        _subscribers.Add(_obs);
    }
    public void UnSubscribe(IObserver _obs)
    {
        _subscribers.Remove(_obs);
    }

    protected void Notify(Notification context)
    {
        if (_subscribers.Count == 0)
            return;

        foreach (IObserver obs in _subscribers)
        {
            obs.OnNotify(context);
        }
    }


}
