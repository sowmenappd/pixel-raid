using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

    List<Monologue> monologues;
    Queue<Monologue> storySequence = new Queue<Monologue>();
    Monologue holder;

    //events for UI handling
    public event System.Action OnMonologueStart;
    public event System.Action OnMonologueEnd;

    Text dialogueBoxText;
    Button dialogueButton;
    Animator dialogueAnimator;

    bool pressed = false;

    void Start(){
        dialogueBoxText = GameObject.Find("DialogueTextBox").GetComponent<Text>();
        dialogueButton  = dialogueBoxText.transform.parent.GetComponent<Button>();
        dialogueButton.onClick.AddListener(OnPressButton);
        
        dialogueAnimator = dialogueButton.GetComponent<Animator>();

        OnMonologueStart += ( () => dialogueAnimator.SetTrigger("OnMonologueStart") );
        
        OnMonologueEnd += ( () => {
            dialogueAnimator.SetTrigger("OnMonologueEnd") ;
            dialogueBoxText.text = "";
            } );

        
        monologues = FindObjectsOfType<Monologue>().OrderBy(o => o.storyOrder).ToList();
        if(monologues != null){
            foreach(Monologue m in monologues){
                storySequence.Enqueue(m);
            }
        }
    }

    public void GetNextMonologue(){
        if(storySequence.Count == monologues.Count){
            if(OnMonologueStart != null) OnMonologueStart();
        }
        holder = storySequence.Dequeue();
        StartCoroutine(DisplayNextMonologueText(holder));
    }

    IEnumerator DisplayNextMonologueText(Monologue monologue){
        if(holder != null){
            if(OnMonologueStart != null) OnMonologueStart();
            int currentIndex = 0;
            while(currentIndex < monologue.text.Length){
                dialogueBoxText.text = "";
                int i=0;
                while(i < monologue.text[currentIndex].Length){
                    dialogueBoxText.text += monologue.text[currentIndex][i];
                    i++;
                    yield return new WaitForSeconds(0.05f);
                } 
                yield return new WaitUntil(() => pressed == true);
                currentIndex++;
            }
            if(OnMonologueEnd != null) OnMonologueEnd();
        }
    }

    public void OnPressButton(){
        pressed = true;
        print("Pressed");
        StartCoroutine(Wait());
    }
    
    IEnumerator Wait(){
        yield return new WaitForSeconds(.25f);    
        pressed = false;        
    }



}
