using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    /*FIELDS*/
    private static TurnManager instance;

    private Camera camera;
    private GameObject selectedPosition;
    private PointerEventData eventData;
    private List<RaycastResult> raycastResults;
    private List<string> playerOneInputs;
    private List<string> playerTwoInputs;
    private int turnsCompleted;
    private bool hasWon, hasTied;

    [SerializeField] SpinWheel spinWheel;
    [SerializeField] GameObject gamePage;
    [SerializeField] GameObject playerOneIndicator;
    [SerializeField] GameObject playerTwoIndicator;
    [SerializeField] GameObject circle;
    [SerializeField] GameObject cross;

    public string turnHolder;


    /*PROPERTIES*/
    public static TurnManager Instance
    {
        get
        { return instance; }
    }


    /*INITIAL METHODS*/
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }    
    }
    private void Start() 
    {
        camera = Camera.main;

        raycastResults = new List<RaycastResult>();
        playerOneInputs = new List<string>();
        playerTwoInputs = new List<string>();
        turnsCompleted = 0;
        hasWon = false;
        hasTied = false;
        
        eventData = new PointerEventData(EventSystem.current);
    }
    private void Update() 
    {
        Debug.Log(spinWheel.isSpinningDone);
        if(!hasWon && !hasTied && spinWheel.isSpinningDone)
        {
            PlayTurn(); 
        }
    }


    /*OTHER METHODS*/
    private void ShowGamePage()
    {
        gamePage.SetActive(true);
    }

    private void PlayTurn()
    {
        if(Input.GetMouseButtonDown(0))
        {
            eventData.position = Input.mousePosition;
            EventSystem.current.RaycastAll(eventData, raycastResults);

            if(raycastResults.Count > 0)
            {
                turnsCompleted++;
                
                selectedPosition = raycastResults[0].gameObject;
                selectedPosition.GetComponent<Image>().raycastTarget = false;
                // Debug.Log("Object name: " + selectedPosition.name);

                if(turnHolder == "PlayerOne")
                {
                    Instantiate(cross, selectedPosition.transform);
                    playerOneInputs.Add(selectedPosition.name);

                    if(playerOneInputs.Count >= 3)
                    {
                        GameEnder.Instance.CheckWinnerOrDraw(turnsCompleted, out hasTied, playerOneInputs, out hasWon, turnHolder);
                    }

                    turnHolder = "PlayerTwo";
                    playerOneIndicator.SetActive(false);
                    ShowIndicator(playerTwoIndicator);
                }
                else
                {
                    Instantiate(circle, selectedPosition.transform);
                    playerTwoInputs.Add(selectedPosition.name);
                    
                    if(playerTwoInputs.Count >= 3)
                    {
                        GameEnder.Instance.CheckWinnerOrDraw(turnsCompleted, out hasTied, playerTwoInputs, out hasWon, turnHolder);
                    }

                    turnHolder = "PlayerOne";
                    playerTwoIndicator.SetActive(false);
                    ShowIndicator(playerOneIndicator);
                }
            }
        }
    }
    private void ShowIndicator(GameObject turnIndicator)
    {
        if(!hasWon && !hasTied)
        {
            turnIndicator.SetActive(true);
        }
    }

    public void SelectFirstTurn(string firstTurnHolder)
    {
        turnHolder = firstTurnHolder;

        if(turnHolder == "PlayerOne")
        {
            playerOneIndicator.SetActive(true);
        }
        else
        {
            playerTwoIndicator.SetActive(true);
        }

        ShowGamePage();
    }
}
