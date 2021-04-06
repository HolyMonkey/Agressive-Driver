using System.Collections;
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

        foreach (TrailRenderer trail in _tireMarks)
        {
            trail.emitting = true;
        }


        _isTireMarks = true;
    }

    public void StopEmitterSkidmarks(bool isTireMarks)
    {
        _isTireMarks = isTireMarks;
        if (!_isTireMarks)
            return;

        foreach (TrailRenderer trail in _tireMarks)
        {
            trail.emitting = false;
        }
;
       
        _isTireMarks = false;
    }
}
