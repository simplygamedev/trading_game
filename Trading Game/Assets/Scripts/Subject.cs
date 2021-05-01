using System.Collections.Generic;
using UnityEngine;

public abstract class Subject<T>
{
	protected List<Observer<T>> observers = new List<Observer<T>>();
	protected T state;

	public T getState() {
		return state;
	}

	public void setState(T state) {
        T prevState = this.state;
		this.state = state;
		notifyAllObservers(prevState, this.state);
	}

	public void attach(Observer<T> observer){
		observers.Add(observer);		
	}

	public void notifyAllObservers(T prevState, T currentState)
    {
		foreach (Observer<T> observer in observers) {
			observer.update(prevState,state);
		}
	} 

    public bool removeObserver(Observer<T> observer)
    {
        return observers.Remove(observer);
    }

    public void removeAllObservers()
    {
        observers.RemoveAll((observer)=> { return true; });
    }
}
