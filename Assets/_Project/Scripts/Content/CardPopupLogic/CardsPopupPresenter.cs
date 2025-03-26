using Project.Content.ReactiveProperty;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Content.UI
{
    public class CardsPopupPresenter : MonoBehaviour
    {
        [SerializeField] private CardsChoose _cardsReference;
        [SerializeField] private List<CoreProgressCard> _cardsToTest;

        private CoreProgressCard[] _cards;

        public ReactiveProperty<IReadOnlyList<CoreProgressCard>> _currentCards = new();
        public IReactiveProperty<IReadOnlyList<CoreProgressCard>> CurrentCards => _currentCards;

        public enum CardsChoose
        {
            Gameplay,
            Test,
        }

        public void Initialize()
        {
            _cards = GetComponentsInChildren<CoreProgressCard>();

            foreach (var card in _cards)
            {
                card.gameObject.SetActive(false);
            }
        }

        public void Show()
        {
            switch (_cardsReference)
            {
                case CardsChoose.Gameplay:
                    _currentCards.Value = ChooseCards();
                    break;
                case CardsChoose.Test:
                    var testCardsArray = _cardsToTest.ToArray();
                    _currentCards.Value = testCardsArray;
                    break;
            }
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
