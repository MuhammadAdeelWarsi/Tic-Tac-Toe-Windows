using System.Collections;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager instance;

    [SerializeField] SpinWheel spinWheel;
    [SerializeField] GameObject mainMenu;


    public static MainMenuManager Instance
    {
        get{return instance;}
    }


    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void Start() 
    {
        mainMenu.SetActive(true);    
    }


    public void StartGame()
    {
        spinWheel.ShowBackground();
        StartCoroutine(DisableMainMenu());
    }


    private IEnumerator DisableMainMenu()
    {
        yield return new WaitForSeconds(2f);
        mainMenu.SetActive(false);
    }
}