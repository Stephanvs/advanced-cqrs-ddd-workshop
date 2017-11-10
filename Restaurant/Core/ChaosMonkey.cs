using System;
using Restaurant.Events;

namespace Restaurant.Core
{
    public class ChaosMonkey<T> : IHandler<T> where T : Message
    {
        private readonly int _dropProbability;
        private readonly int _duplicateProbability;
        private readonly IHandler<T> _handler;
        private readonly Random _random = new Random();

        public ChaosMonkey(int dropProbability, int duplicateProbability, IHandler<T> handler)
        {
            _dropProbability = dropProbability;
            _duplicateProbability = duplicateProbability;
            _handler = handler;
        }
        public void Handle(T message)
        {
            var number = _random.Next(0, 100);

            if (number < _dropProbability)
            {
                // drop message
            }
            else if (number > 100 - _duplicateProbability)
            {
                _handler.Handle(message);
                _handler.Handle(message);
            }
            else
            {
                _handler.Handle(message);
            }
        }
    }
}