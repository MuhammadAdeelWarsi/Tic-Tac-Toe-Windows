using System;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    /*----------------------------------------------------------------------------------*/
    public enum AllAnimatorControllers
    {
        circleController, crossController, popupController, spinWheelController,winController, mainMenuController
    }

    [Serializable]
    private class AllAnimatorControllersInfoHolder
    {
        public AllAnimatorControllers animatorControllerType;
        public Animator animatorComponent;
    }
    /*----------------------------------------------------------------------------------*/


    private static AnimationManager instance;

    [SerializeField] AllAnimatorControllersInfoHolder[] allAnimatorControllersInfoHolders;


    public static AnimationManager Instance
    {
        get { return instance; }
    }


    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }    
    }


    public void MakeStateTransitionByTrigger(AllAnimatorControllers animatorControllerType, string transitionParameterName)
    {
        Animator animator = Array.Find(allAnimatorControllersInfoHolders, info => info.animatorControllerType == animatorControllerType).animatorComponent;

        animator.SetTrigger(transitionParameterName);
    }
}
