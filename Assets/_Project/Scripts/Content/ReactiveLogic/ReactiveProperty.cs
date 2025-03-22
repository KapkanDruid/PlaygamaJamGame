using System;

namespace Project.Content.ReactiveProperty
{
    public class ReactiveProperty<T> : IReactiveProperty<T>
    {
        private T _value;

        public event Action<T> OnValueChanged;
        private readonly Func<T, bool> _predicate;

        public T Value
        {
            get => _value;
            set
            {
                if (_predicate != null && !_predicate(value))
                    return;

                if (!Equals(_value, value))
                {
                    _value = value;
                    OnValueChanged?.Invoke(_value);
                }
            }
        }

        public ReactiveProperty(T initialValue = default, Func<T, bool> predicate = null)
        {
            _value = initialValue;
            _predicate = predicate;
        }
    }
}
