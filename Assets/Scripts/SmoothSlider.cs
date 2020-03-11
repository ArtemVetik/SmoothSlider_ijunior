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
    private Coroutine _damage, _heal;
    private WaitForSeconds _delay;
    private float _targetValue;

    private void Awake()
    {
        _health = GetComponent<Slider>();
    }

    private void Start()
    {
        _delay = new WaitForSeconds(_smooth);
        _targetValue = _health.value = 1f;
        _damage = _heal = null;
    }

    public void GetDamage()
    {
        if (_heal != null)
        {
            StopCoroutine(_heal);
            _heal = null;
        }
        _targetValue = Mathf.Clamp(_targetValue - _step, 0, 1f);
        _damage = StartCoroutine(SmoothDamage());
    }

    public void GetHeal()
    {
        if (_damage != null)
        {
            StopCoroutine(_damage);
            _damage = null;
        }
        _targetValue = Mathf.Clamp(_targetValue + _step, 0, 1f);
        _heal = StartCoroutine(SmoothHeal());
    }

    private IEnumerator SmoothDamage()
    {
        while (_health.value > _targetValue)
        {
            _health.value -= 0.01f;
            yield return _delay;
        }
    }

    private IEnumerator SmoothHeal()
    {
        while (_health.value < _targetValue)
        {
            _health.value += 0.01f;
            yield return _delay;
        }
    }
}
