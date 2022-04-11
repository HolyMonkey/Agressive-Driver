using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntryViewPool : MonoBehaviour
{
    [SerializeField] private int _objectsNumber;
    [SerializeField] private EntryView _template;
    [SerializeField] private Transform _container;

    private EntryView[] _objects;

    public void Start()
    {
        _objects = new EntryView[_objectsNumber];

        for (int i = 0; i < _objects.Length; i++)
        {
            _objects[i] = Instantiate(_template, _container);
            _objects[i].gameObject.SetActive(false);
        }
    }

    public EntryView GetFreeObject()
    {
        return _objects.FirstOrDefault(obj => !obj.gameObject.activeInHierarchy);
    }
}
