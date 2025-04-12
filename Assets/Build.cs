using UnityEngine;

public class Build : MonoBehaviour
{
    int _health = 10;
    Renderer _renderer;
    Color _color;
    float _alpha;

    void Update()
    {
        _alpha = (float)_health / 10;
        if (!_renderer)
        {
            _renderer = this.GetComponent<Renderer>();
        }
        _color = _renderer.material.color;
        if(_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && _health > 0)
        {
            _health--;
            _color.a = _alpha;
            _renderer.material.color = _color;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GameManager._isCol = true;
    }

    void OnTriggerExit(Collider other)
    {
        GameManager._isCol = false;
    }
}
