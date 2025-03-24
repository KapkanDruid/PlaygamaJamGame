using System;

namespace Project.Content.ReactiveProperty
{
    public class ReactiveProperty<T> : IReactiveProperty<T>
    {
        private T _value;

        public event Action<T> OnValueChanged;
        private Func<T, bool> _predicate;

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

        public ReactiveProperty(T initialValue = default)
        {
            _value = initialValue;
        }

        public ReactiveProperty<T> SetPredicate(Func<T, bool> predicate)
        {
            _predicate = predicate;
            return this;
        }
    }
}
