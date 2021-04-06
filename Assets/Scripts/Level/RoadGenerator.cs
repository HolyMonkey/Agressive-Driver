using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private Road[] _templates;
    [SerializeField] private Player _player;

    private readonly int _tileLength = 500;
    private readonly int _activePlatforms = 3;
    private float _spawnPointZ;

    private Queue<Road> _activeLevelPlatforms = new Queue<Road>();

    private void Start()
    {
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

        var levelPlatform = Instantiate(template, template.gameObject.transform.position + transform.forward * _spawnPointZ, Quaternion.identity, transform);
        _activeLevelPlatforms.Enqueue(levelPlatform);

        _spawnPointZ += _tileLength;
    }

    private Road GetRandomTemplate(RoadType type)
    {
        var variants = _templates.Where(levelPlatform => levelPlatform.Type == type);

        if (variants.Count() == 1)
            return variants.First();

        var template = variants.ElementAt(Random.Range(0, variants.Count()));

        return template;
    }

    private void DeletePlatform()
    {
        Destroy(_activeLevelPlatforms.Dequeue().gameObject);
    }
}