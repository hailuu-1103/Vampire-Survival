#nullable enable
namespace VampireSurvival.Core.Models
{
    using System;
    using System.Globalization;
    using UnityEngine;

    public delegate void ValueChanged(float changedAmount);

    [Serializable]
    public sealed class ObservableValue : IFormattable
    {
        [SerializeField] private float value;

        public event ValueChanged? ValueChanged;

        public float Value
        {
            get => this.value;
            set
            {
                if (Mathf.Approximately(this.value, value)) return;
                var oldValue = this.value;
                this.value = value;
                this.ValueChanged?.Invoke(value - oldValue);
            }
        }

        public ObservableValue(float value)
        {
            this.value = value;
        }

        public static implicit operator float(ObservableValue? observableValue) => observableValue?.value ?? 0;

        public override string ToString()
        {
            return this.value.ToString(CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.value.ToString(format, formatProvider);
        }
    }
}