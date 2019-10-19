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
		this.state = state;
		notifyAllObservers();
	}

	public void attach(Observer<T> observer){
		observers.Add(observer);		
	}

	public void notifyAllObservers(){
		foreach (Observer<T> observer in observers) {
			observer.update();
		}
	} 
}
