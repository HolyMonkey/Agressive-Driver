using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private Road[] _templates;
    [SerializeField] private Player _player;

    private readonly int _tileLength = 500;
    private readonly int _activePlatforms = 3;
    private float _spawnPointZ;
    private Quaternion _identity;
    private Transform _transform;

    private Queue<Road> _activeLevelPlatforms = new Queue<Road>();

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _identity = Quaternion.identity;
        TryCreateTemplate(RoadType.StartRoad);
    }

    private void Update()
    {
        float playerPositionZ = _player.transform.position.z;

        if (playerPositionZ > _spawnPointZ - (_activePlatforms * _tileLength))
        {
            TryCreateTemplate(RoadType.ChallengeRoad);
        }

        if (playerPositionZ - _tileLength * 2 >= _activeLevelPlatforms.Peek().transform.position.z)
        {
            DeletePlatform();
        }
    }

    private void TryCreateTemplate(RoadType type)
    {
        var template = GetRandomTemplate(type);

        if (template == null)
            return;

        var levelPlatform = Instantiate(template, template.gameObject.transform.position + _transform.forward * _spawnPointZ, _identity, _transform);
        _activeLevelPlatforms.Enqueue(levelPlatform);

        _spawnPointZ += _tileLength;
    }

    private Road GetRandomTemplate(RoadType type)
    {
        List<Road> roads = new List<Road>();

        for (int i = 0; i < _templates.Length; i++)
        {
            if (_templates[i].Type == type)
            {
                roads.Add(_templates[i]);
            }
        }

        if (roads.Count == 1)
            return roads[0];

        var template = roads[Random.Range(0, roads.Count)];
        return template;
    }

    private void DeletePlatform()
    {
        Destroy(_activeLevelPlatforms.Dequeue().gameObject);
    }
}