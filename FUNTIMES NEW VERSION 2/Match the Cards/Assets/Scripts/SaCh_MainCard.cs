using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaCh_MainCard : MonoBehaviour {

	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {	
	}

    [SerializeField] private SaCh_SceneController controller;
    [SerializeField] private GameObject SaCh_BackCard;

    public AudioSource click;

    public void OnMouseDown()
    {

        if (SaCh_BackCard.activeSelf && controller.canReveal)
        {
            SaCh_BackCard.SetActive(false);
            controller.CardRevealed(this);
            click.Play();
        }
    }

    private int _id;
    public int id
    {
        get { return _id; }
    }

    public void ChangeSprite(int id, Sprite image)
    {
        _id = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }

    public void Unreveal()
    {
        SaCh_BackCard.SetActive(true);
    }
}
