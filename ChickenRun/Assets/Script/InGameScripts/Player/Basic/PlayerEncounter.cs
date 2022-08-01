using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Assets;

public class PlayerEncounter : MonoBehaviour
{
    // 충돌 효과 관련
    public Material Material;
    public Animator Anim;

    private Rigidbody rigid;

    // 오디오 관련
    public AudioClip DeadSound;
    public AudioClip GainFlowerSound;
    public AudioClip GurnishHitSound;

    private AudioSource audioSource;

    private Color originalColor = Color.white;

    // 스턴 관련
    private float stannedTime = 1.5f;
    public bool IsStanned = false;

    // 생존 유무
    public bool IsPlayerDead = false;

    // 꽃 획득 관련
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
        Material.color = originalColor;
        rigid = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        FlowerCount = PlayerPrefsKey.GetIntByKey(PlayerPrefsKey.FlowerCountKey);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Flower")
        {
            other.gameObject.SetActive(false);
            
            audioSource.PlayOneShot(GainFlowerSound);
            
            ++FlowerCount;
        }

        else if (other.tag == "Gurnish")
        {
            IsStanned = true;
            Invoke("UnStanned", stannedTime);
            
            gameObject.layer = (int)LayerType.PlayerDamaged;

            // 충돌 효과
            StartCoroutine(ColorChange());
            
            Anim.SetTrigger(AnimationID.Encounter);
            Anim.SetBool(AnimationID.IsStanned, true);
            
            audioSource.PlayOneShot(GurnishHitSound);
        }

        else if (other.tag == "Lava")
        {
            audioSource.PlayOneShot(DeadSound);

            PlayerDead();
        }
    }

    private void UnStanned()
    {
        IsStanned = false;

        gameObject.layer = (int)LayerType.Player;
        
        Material.color = originalColor;
        Anim.SetBool(AnimationID.IsStanned, false);
    }
    private void PlayerDead()
    {
        gameObject.tag = "PlayerDie";
        gameObject.layer = (int)LayerType.PlayerDie;

        IsPlayerDead = true;

        rigid.velocity = Vector3.zero;
        rigid.AddForce(Vector3.up * 5f, ForceMode.Impulse);

        Anim.SetTrigger(AnimationID.Die);
        
        GameManager.Instance.PlayerDead();
        
        PlayerPrefs.SetInt("FlowerCount", FlowerCount);

        Invoke("DisableSelf", 1f);
    }

    private IEnumerator ColorChange()
    {
        Color damagedColor = new Color(1f, 0f, 0f, 0.5f);

        while (IsStanned)
        {
            Material.color = damagedColor;
            yield return new WaitForSeconds(0.1f);

            Material.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}
