using System.Collections.Generic;
using UnityEngine;

public class SkidmarksEffect : MonoBehaviour
{
    [SerializeField] private List<TrailRenderer> _tireMarks;

    private bool _isTireMarks;

    public void StartEmitterSkidmarks(bool isTireMarks)
    {
        _isTireMarks = isTireMarks;
        if (_isTireMarks)
            return;

        for (int i = 0; i < _tireMarks.Count; i++)
        {
            _tireMarks[i].emitting = true;
        }

        _isTireMarks = true;
    }

    public void StopEmitterSkidmarks(bool isTireMarks)
    {
        _isTireMarks = isTireMarks;
        if (!_isTireMarks)
            return;

        for (int i = 0; i < _tireMarks.Count; i++)
        {
            _tireMarks[i].emitting = false;
        }

        _isTireMarks = false;
    }
}
