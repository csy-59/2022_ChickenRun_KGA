using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEncounter : MonoBehaviour
{
    public Material model;
    public Animator anim;
    public AudioClip DeadSound;
    public AudioClip GainFlowerSound;
    public AudioClip GurnishHitSound;

    public float StannedTime = 1.5f;
    public bool isStanned = false;
    public bool isPlayerDead = false;

    private Rigidbody rigid;
    private AudioSource audioSource;

    private int flowerCount = 0;
    public int FlowerCount
    {
        get { return flowerCount; }
        private set
        {
            flowerCount = value;
            OnGainFlower.Invoke();
        }
    }
    public UnityEvent OnGainFlower = new UnityEvent();

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        model.color = new Color(1f, 1f, 1f, 1f);

        if (!PlayerPrefs.HasKey("FlowerCount"))
        {
            PlayerPrefs.SetInt("FlowerCount", 0);
        }
        FlowerCount = PlayerPrefs.GetInt("FlowerCount");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Flower")
        {
            other.gameObject.SetActive(false);
            ++FlowerCount;
            audioSource.PlayOneShot(GainFlowerSound);
        }

        else if (other.tag == "Gurnish")
        {
            gameObject.layer = 8;
            isStanned = true;
            StartCoroutine(ColorChange());
            anim.SetTrigger("Encounter");
            anim.SetBool("isStanned", true);
            audioSource.PlayOneShot(GurnishHitSound);
            Invoke("UnStanned", StannedTime);
        }

        else if (other.tag == "Lava")
        {
            PlayerDead();
            audioSource.PlayOneShot(DeadSound);
        }
    }

    private void UnStanned()
    {
        isStanned = false;
        gameObject.layer = 7;
        model.color = new Color(1f, 1f, 1f, 1f);
        anim.SetBool("isStanned", false);
    }
    private void PlayerDead()
    {
        gameObject.tag = "PlayerDie";
        gameObject.layer = 8;
        anim.SetTrigger("Die");
        GameManager.Instance.PlayerDead();
        isPlayerDead = true;
        rigid.velocity = Vector3.zero;
        rigid.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        Invoke("DisableSelf", 1f);
        PlayerPrefs.SetInt("FlowerCount", FlowerCount);
    }

    private IEnumerator ColorChange()
    {
        while(isStanned)
        {
            model.color = new Color(1f, 0f, 0f, 0.5f);
            yield return new WaitForSeconds(0.1f);
            model.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}
