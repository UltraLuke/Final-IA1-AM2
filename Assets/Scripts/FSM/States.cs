using System.Collections;
using System.Collections.Generic;

public class States<T>
{
    public virtual void Awake() { }
    public virtual void Execute() { }
    public virtual void Exit() { }
    Dictionary<T, States<T>> statesDictionary = new Dictionary<T, States<T>>();

    public void AddTransitionState(T input, States<T> state)
    {
        if (!statesDictionary.ContainsKey(input))
            statesDictionary.Add(input, state); //TO DO builder con return;
    }

    public void RemoveTransition(T input, States<T> state)
    {
        if (statesDictionary.ContainsKey(input))
            statesDictionary.Remove(input);
    }

    public States<T> GetState(T input)
    {
        if (statesDictionary.ContainsKey(input))
            return statesDictionary[input];
        return null;
    }


}
