using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MonologueManager : MonoBehaviour {
    #region Singleton
    private static GameObject holdingObject;
    private static MonologueManager _instance;
    public static MonologueManager Instance{
        get {
            if(_instance == null) {
                holdingObject = new GameObject("_MonoMan");
                _instance = holdingObject.AddComponent<MonologueManager>();
            }
            return _instance;
        }
    }

    public MonologueManager(){
        print("MonologueManager singleton initiated");
    }

    public void Wakeup(){

    }

    #endregion

    public List<Monologue> monologues;

    Queue<Monologue> storySequence = new Queue<Monologue>();
    Monologue holder;

    //for UI handling
    public event System.Action OnMonologueStart;
    public event System.Action OnMonologueEnd;

    void Start(){
        if(monologues != null){
        foreach(Monologue m in monologues){
            storySequence.Enqueue(m);
        }

            GetNextMonologue();
        }
    }

    public void GetNextMonologue(){
        if(storySequence.Count == monologues.Count){
            if(OnMonologueStart != null) OnMonologueStart();
        }

        holder = storySequence.Dequeue();
    }

    IEnumerator DisplayMonologueText(string text){
        if(holder != null){
            string str = "";
            //Do stuff
            yield return null;
        }
    }

}
