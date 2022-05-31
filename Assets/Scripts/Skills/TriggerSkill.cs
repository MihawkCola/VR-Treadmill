using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TriggerSkill : MonoBehaviour
{
    [SerializeField]
    private ShowSkillMenu skillMenu;
    private ActionBasedController controllerLeft;
    private ActionBasedController controllerRight;
    private bool isGripPressed = false;
    
    // Start is called before the first frame update
    void Start()
    {
        skillMenu = GameObject.Find("Player").GetComponent<ShowSkillMenu>();
        ActionBasedController[] controllerArray = ActionBasedController.FindObjectsOfType<ActionBasedController>();
        controllerRight = controllerArray[0];
        controllerLeft = controllerArray[1];
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if(skillMenu.menuHandLeft == true)
        {
            if (other.CompareTag("RightHand"))
            {


                controllerRight.selectAction.action.performed += Grip_performed;
                controllerRight.selectAction.action.canceled += Grip_canceled;


            }
        }
        else
        {
            if (other.CompareTag("LeftHand"))
            {

                controllerLeft.selectAction.action.performed += Grip_performed;
                controllerLeft.selectAction.action.canceled += Grip_canceled;
                

            }
        }
        
        
       
  
    }
    /*
    private void OnTriggerExit(Collider other)
    {
  
        if (skillMenu.menuHandLeft == true)
        {

            if (other.CompareTag("RightHand"))
            {

                controllerRight.selectAction.action.performed -= Grip_performed;
                controllerRight.selectAction.action.canceled -= Grip_canceled;
                isGripPressed = false;
            }
        }
        else
        {
            if (other.CompareTag("LeftHand"))
            {

                controllerLeft.selectAction.action.performed -= Grip_performed;
                controllerLeft.selectAction.action.canceled -= Grip_canceled;
                isGripPressed = false;
            }
        }
    }
    */
    private void Grip_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("show menu in");
        controllerRight.selectAction.action.performed -= Grip_performed;
        controllerLeft.selectAction.action.performed -= Grip_performed;
        skillMenu.isGripPressed = true;
        skillMenu.closeSkillMenu();
        skillMenu.SendMessage("SelectedSkill", this.name);

    }

    private void Grip_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("show menu out");
        controllerRight.selectAction.action.canceled -= Grip_canceled;
        controllerLeft.selectAction.action.canceled -= Grip_canceled;
        skillMenu.isGripPressed = false;


        skillMenu.resetHand();
    }

}
