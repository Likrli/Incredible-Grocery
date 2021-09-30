using UnityEngine;
using System.Collections;

public class Buyer : MonoBehaviour
{
    [SerializeField] private float buyerStep;
    [SerializeField] private float progress;
    [SerializeField] private Vector2 buyPoint;
    [SerializeField] private Vector2 exitPoint;
    [SerializeField] private GameObject emotionBubble;
    [SerializeField] private SpriteRenderer emotionRenderer;
    [SerializeField] private Sprite positiveEmotion;
    [SerializeField] private Sprite negativeEmotion;
    private const string Walk = "Walk";
    private Animator _buyerAnimator;
    private Storage _storage;
    private AudioManager _audioManager;
    private GameController _gameController;

    private void Start()
    {
        _buyerAnimator = GetComponent<Animator>();
        StartCoroutine(MoveBuyer(currentPosition: transform.position, desiredPosition: buyPoint));
    }

    private IEnumerator MoveBuyer(Vector2 currentPosition, Vector2 desiredPosition)
    {
        progress = 0;
        _buyerAnimator.SetBool(name: Walk, value: true);
        while (progress < 1)
        {
            transform.position = Vector2.Lerp(currentPosition, desiredPosition, progress);
            progress += buyerStep;
            yield return new WaitForFixedUpdate();
        }
        _buyerAnimator.SetBool(name: Walk, value: false);
    }

    private void OnShowEmotion(bool emotion)
    {
        _audioManager.PlayClip(AudioManager.Clip.SpawnBuble);
        emotionBubble.SetActive(true);
        emotionRenderer.sprite = emotion? positiveEmotion : negativeEmotion;
        StartCoroutine(ExitGrocery());
    }

    private IEnumerator ExitGrocery()
    {
        yield return new WaitForSeconds(1f);
        transform.localScale = new Vector2(-1, 1);
        yield return new WaitForSeconds(.5f);
        StartCoroutine(MoveBuyer(currentPosition: transform.position, desiredPosition: exitPoint));
        yield return new WaitForSeconds(1f);
        _audioManager.PlayClip(AudioManager.Clip.CloseBuble);
        emotionBubble.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PointBuy pointBuy))
        {
            _audioManager = collision.GetComponent<AudioManager>();
            _gameController = collision.GetComponent<GameController>();
            _storage = collision.GetComponent<Storage>();
            _storage.ReceivedEmotion += OnShowEmotion;
            _gameController.ServeBuyer();
        }
        if (collision.TryGetComponent(out Exit exit))
        {
            _gameController.GenerateBuyer();
            _storage.ReceivedEmotion -= OnShowEmotion;
            Destroy(gameObject);
        }
    }

}
