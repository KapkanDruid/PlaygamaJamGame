using Project.Content.ReactiveProperty;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Content.UI
{
    public class CardsPopupPresenter : MonoBehaviour
    {
        private CoreProgressCard[] _cards;

        public ReactiveProperty<IReadOnlyList<CoreProgressCard>> _currentCards = new();
        public IReactiveProperty<IReadOnlyList<CoreProgressCard>> CurrentCards => _currentCards;

        public void Initialize()
        {
            _cards = GetComponentsInChildren<CoreProgressCard>();
        }

        public void Show()
        {
            _currentCards.Value = ChooseCards();
        }

        public void CardSelected(ICoreProgressStrategy progressStrategy) 
        {
            progressStrategy.ExecuteProgress();
        }

        private List<CoreProgressCard> ChooseCards()
        {
            int maxExclusive = _cards.Length;
            if (maxExclusive < 3)
            {
                Debug.LogError("Cards number must be more than 2! Current count: " + _cards.Length);
                return null;
            }

            List<int> numbers = new();

            for (int i = 0; i < maxExclusive; i++)
            {
                numbers.Add(i);
            }

            for (int i = 0; i < numbers.Count; i++)
            {
                int j = Random.Range(i, numbers.Count);
                (numbers[i], numbers[j]) = (numbers[j], numbers[i]);
            }

            return new List<CoreProgressCard> { _cards[numbers[0]], _cards[numbers[1]], _cards[numbers[2]] };
        }
    }
}
