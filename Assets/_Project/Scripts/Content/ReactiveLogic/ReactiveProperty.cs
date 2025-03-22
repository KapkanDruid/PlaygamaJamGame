using System;

namespace Project.Content.ReactiveProperty
{
    public class ReactiveProperty<T> : IReactiveProperty<T>
    {
        private T _value;

        public event Action<T> OnValueChanged;

        public T Value
        {
            get => _value;
            set
            {
                if (!Equals(_value, value))
                {
                    _value = value;
                    OnValueChanged?.Invoke(_value);
                }
            }
        }

        public ReactiveProperty(T initialValue = default)
        {
            _value = initialValue;
        }
    }

    public interface IReactiveProperty<T>
    {
        public T Value { get; }
        public event Action<T> OnValueChanged;
    }
}
