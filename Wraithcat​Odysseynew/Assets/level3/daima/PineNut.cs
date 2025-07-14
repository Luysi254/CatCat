using UnityEngine;
using System.Collections;

public class PineNut : MonoBehaviour
{
    public int pointValue = 1;
    [SerializeField] private ParticleSystem collectParticles;
    [SerializeField] private AudioClip collectSound;

    [Header("��Ч����")]
    [Tooltip("��Ч����")] [Range(0f, 1f)] public float volume = 1f; // �����������

    private Collider nutCollider;
    private MeshRenderer nutRenderer;
    private AudioSource audioSource;
    private bool isCollected = false;

    void Start()
    {
        nutCollider = GetComponent<Collider>();
        nutRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();

        // ȷ��AudioSource��ȷ����
        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.volume = volume; // Ӧ����������
        }

        if (collectParticles == null)
            collectParticles = GetComponentInChildren<ParticleSystem>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            StartCoroutine(CollectSequence());
        }
    }

    IEnumerator CollectSequence()
    {
        // �����ռ�Ч��
        PlayCollectEffect();

        // ֪ͨGameManager
        if (GameManager.instance != null)
        {
            GameManager.instance.CollectPineNut(pointValue);
        }
        else
        {
            Debug.LogError("GameManagerʵ��������!");
        }

        // �������ӣ������ֽű����У�
        if (nutCollider) nutCollider.enabled = false;
        if (nutRenderer) nutRenderer.enabled = false;

        // �ȴ���Ч���
        float delay = CalculateEffectDuration();
        yield return new WaitForSeconds(delay);

        // �������Ӷ���
        Destroy(gameObject);
    }

    void PlayCollectEffect()
    {
        // ����Ч��
        PlayParticleEffect();

        // ��Ч
        PlayCollectSound();
    }

    void PlayParticleEffect()
    {
        if (collectParticles != null)
        {
            collectParticles.transform.parent = null;
            collectParticles.Play();
            Destroy(collectParticles.gameObject, collectParticles.main.duration);
        }
    }

    void PlayCollectSound()
    {
        if (collectSound == null)
        {
            Debug.LogWarning("����ȱ���ռ���Ч!");
            return;
        }

        // ʹ��AudioSource������Ч�����������ƣ�
        if (audioSource != null)
        {
            audioSource.volume = volume; // ȷ��Ӧ�õ�ǰ����
            audioSource.PlayOneShot(collectSound, volume);
        }
        else
        {
            // ���û��AudioSource��ʹ�ñ��÷�������
            AudioSource.PlayClipAtPoint(collectSound, transform.position, volume);
        }
    }

    float CalculateEffectDuration()
    {
        float maxDuration = 1.0f;

        if (collectParticles != null)
        {
            float particleDuration = collectParticles.main.duration + collectParticles.main.startLifetime.constantMax;
            maxDuration = Mathf.Max(maxDuration, particleDuration);
        }

        if (collectSound != null)
        {
            float soundDuration = collectSound.length;
            maxDuration = Mathf.Max(maxDuration, soundDuration);
        }

        return maxDuration + 0.1f; // ����0.1�뻺��
    }

    // �ڱ༭���в�����Ч
    [ContextMenu("������Ч����")]
    public void TestSoundPlay()
    {
        if (collectSound == null)
        {
            Debug.LogError("δ������Ч����!");
            return;
        }

        if (audioSource != null)
        {
            audioSource.PlayOneShot(collectSound, volume);
        }
        else
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position, volume);
        }
    }
}