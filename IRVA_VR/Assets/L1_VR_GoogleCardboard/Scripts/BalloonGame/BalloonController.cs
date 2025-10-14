using L1_VR_GoogleCardboard.Scripts.Helpers;
using NaughtyAttributes;
using UnityEngine;

namespace L1_VR_GoogleCardboard.Scripts.BalloonGame
{
    /// <summary>
    /// Script which controls the behavior of balloon game objects.
    /// </summary>
    public class BalloonController : MonoBehaviour
    {
        // Extra info:
        // --> The [SerializeField] attribute enables you to keep variables private while exposing their value in the editor's inspector.
        // --> The [Range(...)] attribute is a nice way to set limits on the value of a variable & it also exposes it as a slider in the editor's inspector.
        // --> The [BoxGroup(...)] attribute is used to group variables in the editor's inspector. It comes from the 'NaughtyAttributes' package.
        // --> The [Required] attribute (also from 'NaughtyAttributes') is a nice way to indicate that a variable must be assigned in the editor's inspector.
        
        [SerializeField] 
        [Range(0.25f, 5f)] 
        [BoxGroup("Settings")]
        private float speed = 1f;
        
        [SerializeField] 
        [Range(0.25f, 2f)] 
        [BoxGroup("Settings")]
        private float requiredTimeToPop = 0.75f;
        
        [SerializeField]
        [BoxGroup("Settings")]
        private Gradient colors;
        
        [SerializeField] 
        [BoxGroup("External components")]
        [Required]
        private AudioClip popSound;
        
        [SerializeField] 
        [BoxGroup("External components")]
        [Required]
        private AudioClip failSound;
        
        private float _balloonPopTimeInternal = 0f;
        private MeshRenderer _balloonMeshRenderer;
        
        private bool _isTargeted = false; 

        private void Awake() => _balloonMeshRenderer = GetComponent<MeshRenderer>();
        
        /// <summary>
        /// This is automatically called from 'CardboardReticlePointer' when the reticle (crosshair) raycasts the collider of this object.
        /// In short, called when the balloon is targeted.
        /// </summary>
        public void OnPointerEnter()
        {
            // TODO 5.1 : Set a value which indicates this game object is targeted.
            _isTargeted = true;
        }
        
        /// <summary>
        /// This is automatically called from 'CardboardReticlePointer' when the reticle (crosshair) ends raycasting with the collider of this object.
        /// In short, called when the balloon is not targeted anymore.
        /// </summary>
        public void OnPointerExit()
        {
            // TODO 5.2 : Set a value which indicates this game object is not targeted anymore.
            _isTargeted = false;
            // TODO 5.3 : Reset the internal timer.
            _balloonPopTimeInternal = 0f;
        }
        
        private void Update()
        {
            MoveBalloon();
            UpdateBalloonTimer();
            SetBalloonColor();
        }

        private void MoveBalloon()
        {
            // TODO 4.1 : Make the balloons move upwards. Don't forget you have a speed variable already defined.
            transform.Translate(Vector3.up * (speed * Time.deltaTime));
        }

        private void UpdateBalloonTimer()
        {
            // TODO 5.4 : Increment the internal timer when the balloon is targeted (gazed at using the reticle).
            //            Pop the balloon if the internal timer exceeds the maximum timer ('requiredTimeToPop' variable).
            //            Hint: There's already a dedicated method to pop the balloon.
            if (_isTargeted)
            {
                _balloonPopTimeInternal += Time.deltaTime;
                if (_balloonPopTimeInternal >= requiredTimeToPop)
                {
                    PopBalloon();
                }
            }
        }
        
        private void SetBalloonColor() => _balloonMeshRenderer.material.color = colors.Evaluate(_balloonPopTimeInternal.Remap(0f, requiredTimeToPop, 0f, 1f));

        private void PopBalloon() => DestroyBalloon(hasBeenPoppedByPlayer: true);
        
        public void DestroyBalloon(bool hasBeenPoppedByPlayer)
        {
            // TODO 6.1 : Change the score - if popped by player, increase it, otherwise decrease it.
            //            Hints: - There's already a `ScoreController` class defined.
            //                   - Notice what class the `ScoreController` extends, and study it to understand how
            //                     the `ScoreController` can be referenced!
            var scoreController = ScoreController.Instance;
            
            if (hasBeenPoppedByPlayer)
            {
                scoreController.IncrementScore();
            }
            else
            {
                scoreController.DecrementScore();
            }
            
            PlayBalloonSound(hasBeenPoppedByPlayer);
            
            gameObject.SetActive(false);
            Destroy(gameObject, 1f);
        }
        
        private void PlayBalloonSound(bool hasBeenPoppedByPlayer) => AudioSource.PlayClipAtPoint(hasBeenPoppedByPlayer ? popSound : failSound, transform.position);
    }
}