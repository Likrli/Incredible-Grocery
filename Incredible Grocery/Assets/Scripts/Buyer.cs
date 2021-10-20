using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class Buyer : MonoBehaviour
{
    [SerializeField] private float progress;
    [SerializeField] private GameObject buyerBubble;
    [SerializeField] private GameObject[] orderedProducts;
    [SerializeField] private GameObject emotionBuyer;
    [SerializeField] private Sprite positiveEmotion;
    [SerializeField] private Sprite negativeEmotion;
    [SerializeField] private SpriteRenderer buyerRendered;
    [SerializeField] private Slider timeSlider;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private Image scaleImg;
    private const string Walk = "Walk";
    private readonly float buyerStep = 0.005f;
    private float _waitingTime;
    private bool _isBuys;
    private bool _isServed;
    private bool _isWaiting;
    private AudioSource _buyerAudioSource;
    private Animator _buyerAnimator;
    private Storage _storage;
    private AudioManager _audioManager;
    private GameController _gameController;

    private void Start()
    {
        _isServed = false;
        _isWaiting = false;
        _isBuys = false;
        _waitingTime = Random.Range(13f, 17f);
        timeSlider.maxValue = _waitingTime;
        timeSlider.value = timeSlider.maxValue;
        scaleImg.color = new Color(0, 1, 0);
        _buyerAnimator = GetComponent<Animator>();
        _buyerAudioSource = GetComponent<AudioSource>();
        InvokeRepeating(nameof(WasteWaitingTime),0f, 0.1f);
    }

    private IEnumerator MoveBuyer(Vector2 currentPosition, Vector2 desiredPosition, float maxProgress)
    {
        progress = 0;
        _isWaiting = false;
        if(currentPosition - new Vector2(0, 1f) != desiredPosition)
        {
            _buyerAnimator.SetBool(name: Walk, value: true);
            while (progress < maxProgress)
            {
                transform.position = Vector2.Lerp(currentPosition, desiredPosition + new Vector2(0, 1f), progress);
                progress += buyerStep;
                yield return new WaitForFixedUpdate();
            }
        }
        _buyerAnimator.SetBool(name: Walk, value: false);
        _isWaiting = true;
    }

    private void WasteWaitingTime()
    {
        if (_isWaiting)
        {
            if(_waitingTime > 0.1f)
            {
                _waitingTime -= 0.1f;
                timeSlider.value = _waitingTime;
                Color color = Color.Lerp(Color.green, Color.red, 1 - _waitingTime / 10);  
                scaleImg.color = color; 
            }
            else
            {
                if (_isBuys)
                {
                    _storage.ResetStorage();
                    _storage.ReceivedEmotion -= OnShowEmotion;
                }
                _buyerAudioSource.PlayOneShot(_audioManager.AllClips[5]);
                OnShowEmotion(false);
            }
        }
    }

    private void StoppedWaitingTime()
    {
        CancelInvoke(nameof(WasteWaitingTime));
        progressBar.SetActive(false);
    }

    private void OnShowEmotion(bool emotion)
    {
        StoppedWaitingTime();
        _audioManager.PlayClip(AudioManager.Clip.SpawnBubble);
        foreach (var product in orderedProducts)
        {
            product.SetActive(false);
        }
        buyerBubble.SetActive(true);
        emotionBuyer.SetActive(true);
        emotionBuyer.GetComponent<Image>().sprite = emotion ? positiveEmotion : negativeEmotion;
        StartCoroutine(ExitGrocery());
        _isServed = true;
    }

    private IEnumerator ExitGrocery()
    {
        yield return new WaitForSeconds(.5f);
        transform.localScale = new Vector2(-1, 1);
        yield return new WaitForSeconds(.5f);
        buyerRendered.sortingOrder = -1;
        _gameController.ChangeBuyersQueue(this);
        StartCoroutine(MoveBuyer(currentPosition: transform.position, desiredPosition: _gameController.Points[2].position, maxProgress: 1));
        yield return new WaitForSeconds(1f);
        _audioManager.PlayClip(AudioManager.Clip.CloseBubble);
        buyerBubble.SetActive(false);
    }

    private void CheckQueue()
    {
        if (!_isServed)
        {
           if(this == _gameController.BuyersQueue[0])
            {
                StartCoroutine(MoveBuyer(currentPosition: transform.position, desiredPosition: _gameController.Points[1].position, maxProgress: 1));
                buyerRendered.sortingOrder = 0;
            }
            else if(this == _gameController.BuyersQueue[1])
            {
                StartCoroutine(MoveBuyer(currentPosition: transform.position, desiredPosition: _gameController.Points[1].position, maxProgress: .55f));
                buyerRendered.sortingOrder = 1;
            }
            else
            {
                StartCoroutine(MoveBuyer(currentPosition: transform.position, desiredPosition: _gameController.Points[1].position, maxProgress: 0.1f));
                buyerRendered.sortingOrder = 2;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PointEntry pointEntry))
        {
            _audioManager = AudioManager.Instance;
            _gameController = collision.GetComponent<GameController>();
           _gameController.ChangedQueue += CheckQueue;
            CheckQueue();
        }
        if (collision.TryGetComponent(out PointBuy pointBuy))
        {
            if (!_isServed)
            {
                _storage = collision.GetComponent<Storage>();
                MakeOrder();
                _storage.ReceivedEmotion += OnShowEmotion;
                _storage.StoppedBuyersTime += StoppedWaitingTime;
            }
        }
        if(collision.TryGetComponent(out PointToExit pointToExit))
        {
            StartCoroutine(MoveBuyer(currentPosition: transform.position, desiredPosition: _gameController.Points[3].position, maxProgress: 1));
        }
        if (collision.TryGetComponent(out Exit exit))
        {
            if(_storage != null)
            {
                _storage.ReceivedEmotion -= OnShowEmotion;
                _storage.StoppedBuyersTime -= StoppedWaitingTime;
            }
            _gameController.SayGoodbyeBuyer(this);
            Destroy(gameObject);
        }
    }

    private void MakeOrder()
    {
        Debug.Log("Buyer selects a product..");
        _isBuys = true;
        _buyerAudioSource.PlayOneShot(_audioManager.AllClips[0]);
        buyerBubble.SetActive(true);
        int orderedProductsNumber = Random.Range(1, 4);
        List<int> orderedProductsId = new List<int>();
        List<int> allProductsId = new List<int>();
        List<Sprite> allProductsSprite = new List<Sprite>();
        foreach (var product in _gameController.AllProducts)
        {
            allProductsId.Add(product.ProductId);
            allProductsSprite.Add(product.ProductSprite);
        }
        for (int i = 0; i < orderedProductsNumber; i++)
        {
            int selectedProduct = Random.Range(0, allProductsId.Count);
            orderedProductsId.Add(allProductsId[selectedProduct]);
            orderedProducts[i].SetActive(true);
            orderedProducts[i].GetComponent<Image>().sprite = allProductsSprite[selectedProduct];
            Debug.Log($"Ordered product with id: {allProductsId[selectedProduct]}");
            allProductsId.RemoveAt(selectedProduct);
            allProductsSprite.RemoveAt(selectedProduct);
        }
        StartCoroutine(TransferOrder(orderedProductsId));
    }
    private IEnumerator TransferOrder(List<int> orderedProductsId)
    {
        CancelInvoke(nameof(WasteWaitingTime));
        yield return new WaitForSeconds(3f);
        InvokeRepeating(nameof(WasteWaitingTime), 0f, 0.1f);
        buyerBubble.SetActive(false);
        _buyerAudioSource.PlayOneShot(_audioManager.AllClips[1]);
        _gameController.ServeBuyer(orderedProductsId);
    }
}
