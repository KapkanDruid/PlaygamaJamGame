using System;

namespace Project.Content.ReactiveProperty
{
    public interface IReactiveProperty<T>
    {
        public T Value { get; }
        public event Action<T> OnValueChanged;
    }
}
