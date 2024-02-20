using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SpinWheel : MonoBehaviour
{
    private int elapsedTime;
    private int torqueAmount;

    [SerializeField] GameObject spinButton;
    [SerializeField] GameObject background;
    [SerializeField] Rigidbody2D wheelRigidbody;
    [SerializeField] Animator animator;

    [HideInInspector] public bool isSpinningDone;
    
    
    private void Start() 
    {
        StartCoroutine(ShowAllParts());

        elapsedTime = 0;
        torqueAmount = Random.Range(-70, -100);
    }


    public void ShowBackground()
    {
        background.SetActive(true);   
    }

    public void OnSpinButtonClick()
    {
        spinButton.GetComponent<Button>().interactable = false;
        spinButton.transform.DOKill();

        StartCoroutine(Spin());
        StartCoroutine(StartTimer());
    }


    private IEnumerator ShowAllParts()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.transform.DOLocalMoveY(0f, 3f).SetEase(Ease.InOutBounce);
        
        yield return new WaitForSeconds(2.75f);
        wheelRigidbody.transform.DOPunchRotation(Vector3.forward * 270f, 3f, 0);

        yield return new WaitForSeconds(4f);
        StartCoroutine(ShowSpinItButton());
    }
    private IEnumerator ShowSpinItButton()
    {
        AnimationManager.Instance.MakeStateTransitionByTrigger(AnimationManager.AllAnimatorControllers.spinWheelController, "FillButtonTrigger");

        yield return new WaitForSeconds(2f);
        spinButton.transform.DOShakeScale(1.5f, 0.1f, 3).SetLoops(-1);
        spinButton.GetComponent<Button>().interactable = true;
    }
    private IEnumerator Spin()
    {
        yield return new WaitForSeconds(1f);
        while(elapsedTime < 10)
        {
            wheelRigidbody.AddTorque(torqueAmount * Time.deltaTime, ForceMode2D.Force);
            yield return null;
        }

        StartCoroutine(CheckStoppage());
    }
    private IEnumerator CheckStoppage()
    {
        while(wheelRigidbody.angularVelocity != 0)
        {
            yield return null;
        }

        TurnManager.Instance.SelectFirstTurn(FindObjectOfType<TurnChooser>().firstTurnHolder);

        yield return new WaitForSeconds(2f);
        gameObject.transform.DOLocalMoveY(-1070f, 3f).SetEase(Ease.InOutBounce);

        yield return new WaitForSeconds(4f);
        AnimationManager.Instance.MakeStateTransitionByTrigger(AnimationManager.AllAnimatorControllers.spinWheelController, "HideBackgroundTrigger");

        yield return new WaitForSeconds(1f);
        background.SetActive(false);
        isSpinningDone = true;
    }
    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(1f);
        AnimationManager.Instance.MakeStateTransitionByTrigger(AnimationManager.AllAnimatorControllers.spinWheelController, "UnfillButtonTrigger");

        for(int i = 1; i <= 10; i++)
        {
            yield return new WaitForSeconds(1f);
            elapsedTime++;
        }
    }
}
