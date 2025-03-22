using Project.Content.ReactiveProperty;

namespace Project.Content.BuildSystem
{
    public interface IHealthData
    {
        public IReactiveProperty<float> Health { get; }
    }
}
