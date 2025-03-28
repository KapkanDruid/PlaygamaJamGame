using System;

namespace Project.Content.BuildSystem
{
    public class PlaceEffect : IDisposable
    {
        private GridPlaceComponent _placeComponent;
        private AudioController _audioController;
        private IPLaceEffectData _effectData;

        public PlaceEffect(GridPlaceComponent placeComponent, AudioController audioController, IPLaceEffectData effectData)
        {
            _placeComponent = placeComponent;
            _audioController = audioController;

            _placeComponent.OnPlaced += OnEffect;
            _effectData = effectData;
        }

        private void OnEffect()
        {
            _audioController.PlayOneShot(_effectData.PlaceSoundEffect);
        }

        public void Dispose()
        {
            _placeComponent.OnPlaced -= OnEffect;
        }
    }
}
