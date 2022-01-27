using UnityEngine;

class Road : MonoBehaviour
{
    [SerializeField] private RoadType _type;

    public RoadType Type => _type;
}