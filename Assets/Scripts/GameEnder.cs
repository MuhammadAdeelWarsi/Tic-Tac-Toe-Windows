using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnder : MonoBehaviour
{
    #region FIELDS
    private static GameEnder instance;

    private List<string[]> allSuccessfulInputs;
    private string[] userSuccessfulInput;
    
    [Header("ENDING")]
    [SerializeField] GameObject winningLine;

    [Header("POPUPS")]
    [SerializeField] GameObject popupsParent;
    [SerializeField] GameObject gameTiedPopup;
    [SerializeField] GameObject playerOneWonPopup;
    [SerializeField] GameObject playerTwoWonPopup;
    [SerializeField] GameObject nextButton;
    
    #endregion


    #region PROPERTIES    
    public static GameEnder Instance
    {
        get{return instance;}
    }
    #endregion

    
    #region INITIATING METHODS
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }    
    }
    private void Start() 
    {
        allSuccessfulInputs = new List<string[]> 
        {
            new string[] {"Position1", "Position2", "Position3"},
            new string[] {"Position4", "Position5", "Position6"},
            new string[] {"Position7", "Position8", "Position9"},
            new string[] {"Position1", "Position4", "Position7"},
            new string[] {"Position2", "Position5", "Position8"},
            new string[] {"Position3", "Position6", "Position9"},
            new string[] {"Position1", "Position5", "Position9"},
            new string[] {"Position3", "Position5", "Position7"},
        };
    }
    #endregion

    
    #region GAME WINNING METHODS
    public void CheckWinnerOrDraw(int turnsCompleted, out bool hasTied, List<string> playerInputs, out bool hasWon, string winnerName)
    {
        userSuccessfulInput = allSuccessfulInputs.SingleOrDefault(successfulInput => successfulInput.All(eachInput => playerInputs.Contains(eachInput)));

        if(userSuccessfulInput != null)
        {
            Debug.Log("Game Won!");

            hasWon = true;
            hasTied = false;
            CheckWinningLinePositionAndRotation(winnerName);
        }
        else
        {
            if(turnsCompleted == 9)
            {
                Debug.Log("Game Tied!");

                hasWon = false;
                hasTied = true;
                StartCoroutine(ShowGameEndPopup(gameTiedPopup));
            }
            else
            {
                Debug.Log("Game Continues!");
                
                hasWon = false;
                hasTied = false;
            }
        }
    }
    
    //Position and rotation of winning line is being set here
    //Winning line is shown over three successful boxes (positions)
    //First box (position) is used to start drawing the winning line
    //One starting position of winning line might have different rotations
    //The correct rotation is being set by checking the second box (position)
    private void CheckWinningLinePositionAndRotation(string winnerName)
    {
        string firstInput = userSuccessfulInput[0];
        string secondInput = userSuccessfulInput[1];

        GameObject startingPositionReferenceObject = GameObject.Find(firstInput);
        Quaternion[] rotationReferences = new Quaternion[]
        {
            Quaternion.identity,
            Quaternion.Euler(new Vector3(0f, 0f, -45f)),
            Quaternion.Euler(new Vector3(0f, 0f, -90f)),
            Quaternion.Euler(new Vector3(0f, 0f, -135f))
        };

        if(firstInput == "Position4" || firstInput == "Position7")
        {
            ShowWinningLine(startingPositionReferenceObject, rotationReferences[0], winnerName);
        }
        else if(firstInput == "Position2")
        {
            ShowWinningLine(startingPositionReferenceObject, rotationReferences[2], winnerName);
        }
        else if(firstInput == "Position1")
        {
            if(secondInput == "Position4")
            {
                ShowWinningLine(startingPositionReferenceObject, rotationReferences[2], winnerName);
            }
            else if(secondInput == "Position2")
            {
                ShowWinningLine(startingPositionReferenceObject, rotationReferences[0], winnerName);
            }
            else
            {
                ShowWinningLine(startingPositionReferenceObject, rotationReferences[1], winnerName);
            }
        }
        else if(firstInput == "Position3")
        {
            if(secondInput == "Position6")
            {
                ShowWinningLine(startingPositionReferenceObject, rotationReferences[2], winnerName);
            }
            else if(secondInput == "Position5")
            {
                ShowWinningLine(startingPositionReferenceObject, rotationReferences[3], winnerName);
            }
        }
    }

    //Winning line is shown after finalizing its position and rotation
    private void ShowWinningLine(GameObject startingPositionReferenceObject, Quaternion rotationReference, string winnerName)
    {
        winningLine.transform.position = startingPositionReferenceObject.transform.position;
        winningLine.transform.rotation = rotationReference;
        winningLine.SetActive(true);

        if(winnerName == "PlayerOne")
        {
            StartCoroutine(ShowGameEndPopup(playerOneWonPopup));
        }
        else if(winnerName == "PlayerTwo")
        {
            StartCoroutine(ShowGameEndPopup(playerTwoWonPopup));
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
    #endregion
    
    
    #region POPUP COROUTINES
    private IEnumerator ShowGameEndPopup(GameObject popupType)
    {
        yield return new WaitForSeconds(2f);
        popupsParent.SetActive(true);
        popupType.SetActive(true);

        yield return new WaitForSeconds(2f);
        nextButton.SetActive(true);
    }
    #endregion
}
