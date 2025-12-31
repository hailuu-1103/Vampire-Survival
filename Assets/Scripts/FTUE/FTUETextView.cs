#nullable enable
namespace Game.FTUE
{
    using Core.FTUE;
    using TMPro;
    using UnityEngine;

    public sealed class FTUETextView : MonoBehaviour, IFTUETextView
    {
        [SerializeField] private GameObject panel = null!;
        [SerializeField] private TMP_Text   text  = null!;

        private void Awake()
        {
            this.HideText();
        }

        public void ShowText(string message)
        {
            this.text.text = message;
            this.panel.SetActive(true);
        }

        public void HideText()
        {
            this.panel.SetActive(false);
        }
    }
}