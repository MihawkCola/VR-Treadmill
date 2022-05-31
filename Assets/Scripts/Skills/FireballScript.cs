using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class FireballScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private ShowSkillMenu skillMenu;
    public GameObject leftHandRef;
    public GameObject rightHandRef;
    public GameObject fireBall;
    private GameObject g1;
    private Vector3 initialPos;
    private ActionBasedController controllerLeft;
    private ActionBasedController controllerRight;
    private bool fired = false;
    private bool _isDone = false;

    void Start()
    {
        skillMenu = GameObject.Find("Player").GetComponent<ShowSkillMenu>();
        ActionBasedController[] controllerArray = ActionBasedController.FindObjectsOfType<ActionBasedController>();
        controllerRight = controllerArray[0];
        controllerLeft = controllerArray[1];

      

    }

    private void Update()
    {
        if(skillMenu.isGripPressed && !_isDone)
        {

       
        if (skillMenu.menuHandLeft)
        {
            controllerRight.activateAction.action.performed += activateAction_performed;
            controllerRight.activateAction.action.canceled += activateAction_cancelled;
            controllerLeft.activateAction.action.performed -= activateAction_performed;
            controllerLeft.activateAction.action.canceled -= activateAction_cancelled;
            }
        else
        {
            controllerLeft.activateAction.action.performed += activateAction_performed;
            controllerLeft.activateAction.action.canceled += activateAction_cancelled;
            controllerRight.activateAction.action.performed -= activateAction_performed;
            controllerRight.activateAction.action.canceled -= activateAction_cancelled;
            }
            _isDone = true;
        }
        else
        {
            _isDone = false;
        }
    }

    private void activateAction_cancelled(InputAction.CallbackContext obj)
    {
        Debug.Log("FireballScript" + obj.action);
    }

    private void activateAction_performed(InputAction.CallbackContext obj)
    {
        if (skillMenu._isFireBallActive) { 
            if (fired == false)
            {
            
                if (skillMenu.menuHandLeft)
            {
            
                    fired = true;
                    g1 = Instantiate(fireBall, rightHandRef.transform.position + rightHandRef.transform.forward * 0.5f, Quaternion.identity);
                    initialPos = rightHandRef.transform.forward;
                    g1.GetComponent<Rigidbody>().AddForce(initialPos + rightHandRef.transform.forward * 1000, ForceMode.Acceleration);

                }
            else
            {
                fired = true;
                g1 = Instantiate(fireBall, leftHandRef.transform.position + leftHandRef.transform.forward * 0.5f, Quaternion.identity);
                initialPos = leftHandRef.transform.forward;
                g1.GetComponent<Rigidbody>().AddForce(initialPos + leftHandRef.transform.forward * 1000, ForceMode.Acceleration);
            }
                StartCoroutine(waitForSeconds());


            }
        }



    }



    IEnumerator waitForSeconds()
    {
        yield return new WaitForSeconds(2);
        fired = false;
        Destroy(g1);

    }
}
