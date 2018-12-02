using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaCh_SceneController : MonoBehaviour
{

    public AudioSource backgroundmusic;
    public AudioSource lose;

    public const int gridRows = 2;
    public const int gridCols = 3;
    public const float offsetX = 11f;
    public const float offsetY = 15f;

    private float timer;
    private int wholetime;

    public TextMesh scoreText;
    public TextMesh winText;
    public TextMesh endText;
    private int score;

    [SerializeField] private SaCh_MainCard originalCard;
    [SerializeField] private Sprite[] images;


    private void Start()
    {
        backgroundmusic.PlayDelayed(2.0f);
        score = 0;
        SetScoreText();
        winText.text = "";

        Vector3 startPos = originalCard.transform.position;

        int[] numbers = { 0, 0, 1, 1, 2, 2 };
        numbers = ShuffleArray(numbers);

        for (int i = 0; i < gridCols; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                SaCh_MainCard card;
                if (i == 0 && j == 0)
                {
                    card = originalCard;
                }
                else
                {
                    card = Instantiate(originalCard) as SaCh_MainCard;
                }
                int index = j * gridCols + i;
                int id = numbers[index];
                card.ChangeSprite(id, images[id]);

                float posX = (offsetX * i) + startPos.x;
                float posY = (offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);
            }
        }
    }

    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }

    void FixedUpdate()
    {

        timer = timer + Time.deltaTime;
        if (timer == 10)
        {
            endText.text = "You Lose!";
            lose.Play();
            StartCoroutine(ByeAfterDelay(2));

        }

    }



    //-----------------------------------------------------------------//



    private SaCh_MainCard _firstRevealed;
    private SaCh_MainCard _secondRevealed;

    public AudioSource match;
    public AudioSource win;

    public bool canReveal
    {
        get { return _secondRevealed == null; }
    }

    public void CardRevealed(SaCh_MainCard card)
    {
        if (_firstRevealed == null)
        {
            _firstRevealed = card;
        }
        else
        {
            NewMethod(card);
            StartCoroutine(CheckMatch());
        }
    }

    private void NewMethod(SaCh_MainCard card)
    {
        _secondRevealed = card;
    }

    private IEnumerator CheckMatch()
    {
        if (_firstRevealed.id == _secondRevealed.id)
        {
            match.Play();
            score = score + 2;
            SetScoreText();
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            _firstRevealed.Unreveal();
            _secondRevealed.Unreveal();
        }
        _firstRevealed = null;
        _secondRevealed = null;
    }

    void SetScoreText()
    {
        scoreText.text = "Cards Matched: " + score.ToString();
        if (score == 6)
        {
            GameLoader.AddScore(10);
            winText.text = "You Win 10 Points!";
            win.Play();
            StartCoroutine(ByeAfterDelay(2));
        }
    }

    IEnumerator ByeAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        GameLoader.gameOn = false;
    }

}
