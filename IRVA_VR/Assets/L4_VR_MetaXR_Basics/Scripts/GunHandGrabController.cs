using System;
using System.Linq;
using NaughtyAttributes;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction.Input;
using Oculus.Interaction.PoseDetection;
using UnityEngine;

namespace L4_VR_MetaXR_Basics.Scripts
{
    /// <summary>
    /// Script used to track which hand is currently grabbing the gun.
    /// </summary>
    public class GunHandGrabController : MonoBehaviour
    {
        [SerializeField]
        [BoxGroup("Hand components")]
        [Required]
        public HandGrabInteractable handGrabInteractable;

        [SerializeField]
        [BoxGroup("Hand components")]
        [Required]
        private FingerFeatureStateProvider leftHandFingerStateProvider;
        
        [SerializeField]
        [BoxGroup("Hand components")]
        [Required]
        private FingerFeatureStateProvider rightHandFingerStateProvider;

        private IHand currentHand;

        // TODO: → Subscribe to 'WhenStateChanged' event off the 'handGrabInteractable' object to track grab/release events.
        //       → In the invoked method use the 'InteractableStateChangeArgs' 'NewState' and 'PreviousState' properties to determine if the gun was grabbed or released.
        //           → If 'NewState' is 'Select', the gun was grabbed.
        //           → If 'PreviousState' is 'Select' and 'NewState' is not 'Select', the gun was released.
        //       → On grab
        //           → First get the selecting interactor from the 'handGrabInteractable' (use 'SelectingInteractors' property).
        //           → From the interactor, get the 'Hand' property to determine which hand grabbed the gun.
        //           → Store the reference to the grabbing hand in the 'currentHand' variable.
        //       → On release
        //           → Clear the 'currentHand' reference.

        private void OnEnable()
        {
            if (handGrabInteractable != null)
            {
                handGrabInteractable.WhenStateChanged += HandleStateChanged;
            }
        }
        
        private void OnDisable()
        {
            // Unsubscribe to prevent memory leaks
            if (handGrabInteractable != null)
            {
                handGrabInteractable.WhenStateChanged -= HandleStateChanged;
            }
        }

        private void HandleStateChanged(InteractableStateChangeArgs state)
        {
            if (state.NewState == InteractableState.Select)
            {
                var handInteractor = handGrabInteractable.SelectingInteractors.FirstOrDefault();

                if (handInteractor != null)
                {
                    currentHand = handInteractor.Hand;
                }
            }
            else if (state.PreviousState == InteractableState.Select && state.NewState != InteractableState.Select)
            {
                currentHand = null;
                GetComponent<GunFingerCurlController>().ReleaseStop();
            }
        }

        public bool IsGunGrabbed() => currentHand != null;
        
        public void GetCurrentFingerStateProvider(out FingerFeatureStateProvider fingerFeatureStateProvider)
        {
            if (currentHand == null)
            {
                fingerFeatureStateProvider = null;
                return;
            }
            fingerFeatureStateProvider = currentHand.Handedness == Handedness.Left
                ? leftHandFingerStateProvider
                : rightHandFingerStateProvider;
        }
    }
}
