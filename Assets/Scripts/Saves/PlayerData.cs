using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

    [Serializable]
    public class PlayerData
    {
        [field: Preserve]
        public int TotalScore;
        [field: Preserve]
        public int Money;
        [field: Preserve]
        public UnlockedCars UnlockedCarsId;
        [field: Preserve] 
        public int CurrentCar;
        [field: Preserve] 
        public int CurrentLevel;
        
        [Serializable]
        public class UnlockedCars
        {
            [field: Preserve]
            public List<int> UnlockedCarsIdArray;
        }
    }
