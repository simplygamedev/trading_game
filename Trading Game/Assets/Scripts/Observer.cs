using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Observer<T> {
	protected Subject<T> subject;
	public abstract void update(T prevState, T currentState);
}