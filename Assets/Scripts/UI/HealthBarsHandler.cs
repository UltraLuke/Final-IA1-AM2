using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarsHandler : MonoBehaviour
{
    private static HealthBarsHandler _instance;
    public static HealthBarsHandler Instance { get => _instance; }

    public Vector2 offset;
    public Slider healthbarPrefab;
    public Slider miniHealthbarPrefab;
    Camera _camera;

    Dictionary<Transform, HealthBarData> _entities = new Dictionary<Transform, HealthBarData>();
    Queue<Transform> _incomingEntities = new Queue<Transform>();
    Queue<HealthBarData> _incomingData = new Queue<HealthBarData>();
    Queue<Transform> _outgoingEntities = new Queue<Transform>();

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        _camera = Camera.main;
    }

    private void Update()
    {
        //var posInScreen = _camera.WorldToScreenPoint(entity.transform.position);
        //posInScreen.z = 0;
        //Debug.Log(posInScreen);
        //healthbar.GetComponent<RectTransform>().anchoredPosition = posInScreen;
        //healthbar.GetComponent<RectTransform>().position = posInScreen + new Vector3(offset.x, offset.y);

        if (_incomingEntities.Count > 0 && _incomingData.Count > 0)
        {
            for (int i = 0; i < _incomingEntities.Count; i++)
            {
                _entities[_incomingEntities.Dequeue()] = _incomingData.Dequeue();
            }
        }
        if(_outgoingEntities.Count > 0)
        {
            for (int i = 0; i < _outgoingEntities.Count; i++)
            {
                _entities.Remove(_outgoingEntities.Dequeue());
            }
        }

        foreach (var item in _entities)
        {
            if (item.Key != null)
            {
                var tr = item.Key;
                var hd = item.Value;
                var posInScreen = _camera.WorldToScreenPoint(tr.position);
                hd.sliderTransform.anchoredPosition = posInScreen;
                hd.sliderTransform.position = posInScreen + new Vector3(offset.x, offset.y);
                hd.healthBar.value = hd.lifeGetter();
            }
            else
            {
                Destroy(item.Value.healthBar);
                _entities.Remove(item.Key);
            }
        }
    }

    public void SubscribeHPListener(Transform transform, float min, float max, HealthBarData.LifeGetter lg, bool tiny = false)
    {
        var hBar = tiny ? miniHealthbarPrefab : healthbarPrefab;
        var hb = Instantiate(hBar, this.transform);
        hb.minValue = min;
        hb.maxValue = max;
        var tr = hb.GetComponent<RectTransform>();
        tr.anchoredPosition = _camera.WorldToScreenPoint(transform.position);
        tr.position = tr.anchoredPosition + offset;
        var hd = new HealthBarData
        {
            healthBar = hb,
            sliderTransform = tr,
            lifeGetter = lg
        };

        _incomingEntities.Enqueue(transform);
        _incomingData.Enqueue(hd);
        //_entities[transform] = hd;
    }

    public void UnsubscribeHPListener(Transform transform)
    {
        if (_entities.ContainsKey(transform))
        {
            Destroy(_entities[transform].healthBar.gameObject);
            _outgoingEntities.Enqueue(transform);
        }
    }
}

public struct HealthBarData
{
    public delegate float LifeGetter();

    public Slider healthBar;
    public RectTransform sliderTransform;
    public LifeGetter lifeGetter;
}
