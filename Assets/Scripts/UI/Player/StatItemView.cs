#nullable enable

namespace Game.UI.Player
{
    using TMPro;
    using UnityEngine;
    using VampireSurvival.Core.Models;

    public class StatItemView : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText  = null!;
        [SerializeField] private TMP_Text valueText = null!;

        private ObservableValue? observableValue;

        public void Bind(string statName, ObservableValue value)
        {
            this.Unbind();

            this.observableValue = value;
            this.nameText.text   = FormatStatName(statName);
            this.UpdateValue(value.Value);

            this.observableValue.ValueChanged += this.OnValueChanged;
        }

        public void Unbind()
        {
            if (this.observableValue != null)
            {
                this.observableValue.ValueChanged -= this.OnValueChanged;
                this.observableValue              =  null;
            }
        }

        private void OnValueChanged(float _)
        {
            if (this.observableValue != null)
            {
                this.UpdateValue(this.observableValue.Value);
            }
        }

        private void UpdateValue(float value)
        {
            this.valueText.text = value.ToString("F1");
        }

        private static string FormatStatName(string statName)
        {
            return statName.Replace("_", " ");
        }

        private void OnDestroy()
        {
            this.Unbind();
        }
    }
}