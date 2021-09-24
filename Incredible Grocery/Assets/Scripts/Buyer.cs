using UnityEngine;
using System.Collections;

public class Buyer : MonoBehaviour
{
    [SerializeField] private float _stepBuyer;
    [SerializeField] private float _progress;
    [SerializeField] private Vector2 _pointBuy;
    [SerializeField] private Vector2 _pointExit;
    [SerializeField] private GameObject _bubleEmotions;
    [SerializeField] private SpriteRenderer _rendererEmotions;
    [SerializeField] private Sprite[] _badgeEmotions;
    private Animator _animatorBuyer;
    private Storage _storage;
    private AudioManager _audioManager;
    private GameController _gameController;

    private void Start()
    {
        _animatorBuyer = GetComponent<Animator>();
        StartCoroutine(MoveBuyer(currentPosition: transform.position, desiredPosition: _pointBuy));
    }

    private IEnumerator MoveBuyer(Vector2 currentPosition, Vector2 desiredPosition)
    {
        _progress = 0;
        _animatorBuyer.SetBool(name: "Walk", value: true);
        while (_progress < 1)
        {
            transform.position = Vector2.Lerp(currentPosition, desiredPosition, _progress);
            _progress += _stepBuyer;
            yield return new WaitForFixedUpdate();
        }
        _animatorBuyer.SetBool(name: "Walk", value: false);
    }

    private void OnShowEmotion(bool emotion)
    {
        _audioManager.PlayClip(1, AudioManager.Clip.SpawnBuble);
        _bubleEmotions.SetActive(true);
        switch (emotion)
        {
            case true:  _rendererEmotions.sprite = _badgeEmotions[1];
                break;
            case false: _rendererEmotions.sprite = _badgeEmotions[0];
                break;
        }
        StartCoroutine(ExitGrocery());
    }

    private IEnumerator ExitGrocery()
    {
        yield return new WaitForSeconds(1f);
        transform.localScale = new Vector2(-1, 1);
        yield return new WaitForSeconds(.5f);
        StartCoroutine(MoveBuyer(currentPosition: transform.position, desiredPosition: _pointExit));
        yield return new WaitForSeconds(1f);
        _audioManager.PlayClip(1, AudioManager.Clip.CloseBuble);
        _bubleEmotions.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PointBuy pointBuy))
        {
            _audioManager = collision.GetComponent<AudioManager>();
            _gameController = collision.GetComponent<GameController>();
            _storage = collision.GetComponent<Storage>();
            _storage.Emotion += OnShowEmotion;
            _gameController.ServeBuyer();
        }
        if (collision.TryGetComponent(out Exit exit))
        {
            _gameController.GenerateBuyer();
            _storage.Emotion -= OnShowEmotion;
            Destroy(gameObject);
        }
    }

}
