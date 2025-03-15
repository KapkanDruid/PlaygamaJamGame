namespace Assets.Scripts.Content
{
    public interface IEntity
    {
        public T ProvideComponent<T>() where T : class;
    }
}
