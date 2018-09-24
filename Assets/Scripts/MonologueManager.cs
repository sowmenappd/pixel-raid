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

    List<Monologue> monologues; // this holds all the monologues in the game
    Monologue holder; // this holds the current monologue being played

    //events for UI handling
    public event System.Action OnMonologueStart;
    public event System.Action OnMonologueEnd;

    Text dialogueBoxText;
    Button dialogueButton;
    Animator dialogueAnimator;

    bool pressed = false;
    bool locked = false;
    public bool Locked { get { return locked; } }

    float charUpdateRate;

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

        foreach(StoryEvent _event in FindObjectsOfType<StoryEvent>()){
        }
    }

    public void GetNextMonologue(){
        if(OnMonologueStart != null) OnMonologueStart();
        holder = monologues[0];
        monologues.RemoveAt(0);
        StartCoroutine(DisplayNextMonologueText(holder));
        holder.gameObject.SetActive(false);
    }

    public void GetMonologueByOrder(int order){
        if(order >= monologues.Count) return;
        if(OnMonologueStart != null) OnMonologueStart();
        holder = monologues.Find(e => e.storyOrder == order);
        if(monologues.RemoveAll(e => e.storyOrder == order)>0){
            print("removed");
        }
        StartCoroutine(DisplayNextMonologueText(holder));
        holder.gameObject.SetActive(false);
    }

    IEnumerator DisplayNextMonologueText(Monologue monologue){
        if(holder != null){
            int currentIndex = 0;
            locked = true;
            charUpdateRate = .003f;
            while(currentIndex < monologue.lines.Length){
                if(monologue.lines[currentIndex].name != ""){
                    dialogueBoxText.text = monologue.lines[currentIndex].name + ": ";
                } else {
                    dialogueBoxText.text = "";
                }
                int i=0;
                while(i < monologue.lines[currentIndex].text.Length){
                    dialogueBoxText.text += monologue.lines[currentIndex].text[i];
                    i++;
                    yield return new WaitForSeconds(charUpdateRate);
                } 
                yield return new WaitUntil(() => pressed == true);
                locked = false;
                currentIndex++;
            }
            if(OnMonologueEnd != null) OnMonologueEnd();
        }
    }

    public void OnPressButton(){
        pressed = true;
        StartCoroutine(Wait());
        if(locked) charUpdateRate = .001f;
    }
    
    IEnumerator Wait(){
        yield return new WaitForSeconds(.05f);    
        pressed = false;        
    }
}