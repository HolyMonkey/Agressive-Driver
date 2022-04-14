using System;
using UnityEngine.Scripting;

    [Serializable]
    public class PlayerData
    {
        [field: Preserve]
        public int TotalScore;
        [field: Preserve]
        public int Score;
        [field: Preserve] 
        public int[] UnlockedCars;
        [field: Preserve] 
        public int CurrentCar;
    }
