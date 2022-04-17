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
        public List<CarData> UnlockedCars;
        [field: Preserve] 
        public int CurrentCar;
    }
