using UnityEngine;

//for any object that will be used on object pooling
public interface IPoolable
{
    //called by the pool before handing the object to a requester
    void OnRent();

    //called just before the object is returned to the pool
    void OnReturn();

    //fully reset runtime state and unsubscribe from all events
    void ResetState();
}
