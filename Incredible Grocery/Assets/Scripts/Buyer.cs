using UnityEngine;
using System.Collections;

public class Buyer : MonoBehaviour
{
    public float Step;
    public float Progress;

    private Vector2 PointBuy;
    private Vector2 StartPos;

    public GameObject[] ObjEmotions;

    private Animator p_animControll;

    private Storage p_storage;
    private AudioManager p_audioManager;

    private GameController p_gameController;

    private void Start()
    {
        p_animControll = GetComponent<Animator>();
        StartPos = transform.position;
        PointBuy = GameObject.Find("PointBuy").GetComponent<Transform>().position;
        PointBuy += new Vector2(0, 1f);

        p_audioManager = FindObjectOfType<AudioManager>();
        p_gameController = FindObjectOfType<GameController>();
        p_storage = FindObjectOfType<Storage>();
        p_storage.onEmotion += SetEmotion;

    }

    private void FixedUpdate()
    {
        if (Progress < 1)
        {
            p_animControll.SetBool("Walk", true);
            transform.position = Vector2.Lerp(StartPos, PointBuy, Progress);
            Progress += Step;
        }
        else
        {
            p_animControll.SetBool("Walk", false);
        }
    }

    public void SetEmotion(int varEmotion) //0 - angry 1 - good
    {
        p_audioManager.PlayClip(1,0);
        ObjEmotions[varEmotion].SetActive(true);
        StartCoroutine(ExitGrocery());
    }

    IEnumerator ExitGrocery()
    {
        yield return new WaitForSeconds(1f);
        transform.localScale = new Vector2(-1, 1);
        yield return new WaitForSeconds(.5f);
        StartPos = transform.position; 
        PointBuy = GameObject.Find("PointExit").GetComponent<Transform>().position;
        Progress = 0;
        yield return new WaitForSeconds(1.5f);
        p_audioManager.PlayClip(1, 1);
        foreach (var item in ObjEmotions)
        {
            item.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PointExit")
        {
            p_gameController.SpawnBuyer();
            p_storage.onEmotion -= SetEmotion;
            Destroy(gameObject);
        }
    }

}
