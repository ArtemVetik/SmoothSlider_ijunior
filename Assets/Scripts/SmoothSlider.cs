using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SmoothSlider : MonoBehaviour
{
    [SerializeField] private float _smooth;
    [SerializeField] [Range(0.01f,0.5f)] private float _step;

    private Slider _health;
    private Coroutine _decrease, _increase;
    private WaitForSeconds _delay;
    private float _targetValue;
    private float _delta;

    private void Awake()
    {
        _health = GetComponent<Slider>();
    }

    private void Start()
    {
        _delay = new WaitForSeconds(_smooth);
        _targetValue = _health.value = 1f;
        _delta = 0.01f;
        _decrease = _increase = null;
    }

    public void DecreaseValue()
    {
        TryStopCoroutine(ref _increase);
        TryStopCoroutine(ref _decrease);

        _targetValue = Mathf.Clamp(_targetValue - _step, 0, 1f);
        _decrease = StartCoroutine(ChangeValue(_delta));
    }

    public void IncreaseValue()
    {
        TryStopCoroutine(ref _increase);
        TryStopCoroutine(ref _decrease);

        _targetValue = Mathf.Clamp(_targetValue + _step, 0, 1f);
        _increase = StartCoroutine(ChangeValue(_delta));
    }

    private IEnumerator ChangeValue(float delta)
    {
        while (_health.value != _targetValue)
        {
            _health.value = Mathf.MoveTowards(_health.value, _targetValue, delta);
            yield return _delay;
        }
    }

    private void TryStopCoroutine(ref Coroutine coroutine)
    {
        if (coroutine == null)
            return;

        StopCoroutine(coroutine);
        coroutine = null;
    }
}
