#if UNITY_STANDALONE 
using System;
using UnityEngine;
using Valve.VR;

namespace L3_VR_SteamVR_Advanced.Scripts.Input
{
    public class SteamVRInputActionsTesting : MonoBehaviour
    {
        // TODO 1 : Setup input for the already-defined `GrabPinch` action.
        //          Write a message in the console which signifies this input is correctly read.
        //          Use either the polling method or an event-based mechanism

        private void OnEnable()
        {
            SteamVR_Actions._default.GrabPinch.onChange += OnGrabPinchChanged;
            SteamVR_Actions._default.TouchTrigger.onChange += OnTouchTriggerChanged;
            SteamVR_Actions._default.JoystickInput.onChange += OnJoystickInputChanged;
        }
        private void OnDisable()
        {
            SteamVR_Actions._default.GrabPinch.onChange -= OnGrabPinchChanged;
            SteamVR_Actions._default.TouchTrigger.onChange -= OnTouchTriggerChanged;
            SteamVR_Actions._default.JoystickInput.onChange -= OnJoystickInputChanged;
        }

        private void OnGrabPinchChanged(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool grabPinchState)
        {
            print($"[SteamVRInputActionsTesting] Events: grabPinchState = {grabPinchState}");
        }

        // TODO 2 : Setup input for the `TouchTrigger` action (you'll have to first create it & bind it accordingly)
        //          Write a message in the console which signifies this input is correctly read.
        //          Use either the polling method or an event-based mechanism.
        
        private void OnTouchTriggerChanged(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool touchTriggerState)
        {
            print($"[SteamVRInputActionsTesting] Events: touchTriggerState = {touchTriggerState}");
        }

        private void OnJoystickInputChanged(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource,
            Vector2 value, Vector2 delta)
        {
            print($"[SteamVRInputActionsTesting] Events: value = {value}, delta = {delta}");
        }
    }
}
#endif