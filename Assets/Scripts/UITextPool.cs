using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UITextPool : MonoBehaviour
{
    [SerializeField] private UITextTemplate _template;
    [SerializeField] private Transform _container;
    [SerializeField] private int _capacity;
    
    private readonly List<UITextTemplate> _pool = new List<UITextTemplate>();
    
    private void Awake()
    {
        Fill();
    }
    
    public bool TryGetActiveObject(out UITextTemplate activeObject)
    {
        foreach (var item in _pool.Where(item => item.gameObject.activeSelf == true))
        {
            activeObject = item;
            return true;
        }
        activeObject = null;
        return false;
    }
    
    private void Fill()
    {
        for (int i = 0; i < _capacity; i++)
        {
            var spawned = Instantiate(_template, _container);
            spawned.gameObject.SetActive(false);
        }
    }
}
