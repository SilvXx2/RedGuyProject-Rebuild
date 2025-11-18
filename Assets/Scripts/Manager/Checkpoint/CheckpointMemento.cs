using System.Collections.Generic;
using UnityEngine;

namespace Checkpoints
{
    [System.Serializable]
    public class CheckpointMemento
    {
        public string SceneName { get; }
        public Vector3 PlayerPosition { get; }
        public int Coins { get; }
        public HashSet<string> CollectedCoinIds { get; }

        public CheckpointMemento(string sceneName, Vector3 playerPosition, int coins, HashSet<string> collectedCoinIds)
        {
            SceneName = sceneName;
            PlayerPosition = playerPosition;
            Coins = coins;
            CollectedCoinIds = new HashSet<string>(collectedCoinIds);
        }
    }
}
