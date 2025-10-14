using L1_VR_GoogleCardboard.Scripts.Helpers;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace L1_VR_GoogleCardboard.Scripts.BalloonGame
{
    /// <summary>
    /// Used to keep track of the game score & update a world space UI canvas.
    /// </summary>
    public class ScoreController : Singleton<ScoreController>
    {
        // Check the comments in 'BalloonController' for more info about the attributes used below.
        
        [SerializeField] 
        [BoxGroup("External components")] 
        [Required]
        private TextMeshProUGUI scoreText;

        private int score = 0;

        public void IncrementScore()
        {
            // TODO 6.2 : Keep track of the score & set the UI text.
            score += 1;
            scoreText.text = score.ToString();
        }

        public void DecrementScore()
        {
            // TODO 6.3 : Keep track of the score & set the UI text.
            score -= 1;
            scoreText.text = score.ToString();
        }
    }
}