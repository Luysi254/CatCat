using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("��������")]
    public int totalPineNuts = 5;
    [SerializeField] private int _collectedPineNuts = 0;

    [Header("Կ������")]
    public GameObject keyPrefab;
    private GameObject spawnedKey;
    public bool hasKey = false;
    public Vector3 keySpawnPosition = new Vector3(112f, 18f, 176f);

    [Header("UI ����")]
    public Text pineNutText;
    public GameObject keyGetMask; // ��������ק KeyGetMask ��嵽�˴�

    public bool shouldSpawnKey { get; private set; } = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            ResetGameState();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        hasKey = false;
        FindUIReferences();
        UpdatePineNutUI();

        if (shouldSpawnKey)
        {
            Debug.Log("�������أ�Ӧ�ڴ˳�������Կ��");
            SpawnKeyInScene();
        }
    }

    void FindUIReferences()
    {
        // ��̬���� UI Ԫ�أ����δ�ֶ����䣩
        if (pineNutText == null)
        {
            GameObject uiTextObj = GameObject.FindGameObjectWithTag("PineNutCounter");
            if (uiTextObj != null) pineNutText = uiTextObj.GetComponent<Text>();
        }

        if (keyGetMask == null)
        {
            keyGetMask = GameObject.Find("KeyGetMask"); // ȷ���������ƥ��
            if (keyGetMask != null) keyGetMask.SetActive(false); // ��ʼ����
        }
    }

    public void ResetGameState()
    {
        collectedPineNuts = 0;
        hasKey = false;
        shouldSpawnKey = false;

        if (keyGetMask != null) keyGetMask.SetActive(false);
        if (spawnedKey != null) Destroy(spawnedKey);
    }

    public int collectedPineNuts
    {
        get => _collectedPineNuts;
        private set
        {
            _collectedPineNuts = value;
            UpdatePineNutUI();
            CheckKeySpawnCondition();
        }
    }

    public void CollectPineNut(int value)
    {
        collectedPineNuts += value;
    }

    void UpdatePineNutUI()
    {
        if (pineNutText != null)
            pineNutText.text = $"����: {collectedPineNuts}/{totalPineNuts}";
    }

    void CheckKeySpawnCondition()
    {
        if (!shouldSpawnKey && collectedPineNuts >= totalPineNuts && !hasKey)
        {
            shouldSpawnKey = true;
            SpawnKeyInScene();
        }
    }

    void SpawnKeyInScene()
    {
        if (!shouldSpawnKey || hasKey || keyPrefab == null) return;

        if (spawnedKey != null) Destroy(spawnedKey);

        spawnedKey = Instantiate(keyPrefab, keySpawnPosition, Quaternion.identity);
        shouldSpawnKey = false;
        Debug.Log($"Կ��������λ��: {keySpawnPosition}");
    }

    public void ObtainKey()
    {
        hasKey = true;
        if (spawnedKey != null) Destroy(spawnedKey);

        if (keyGetMask != null)
        {
            keyGetMask.SetActive(true);
            Debug.Log("��ʾ KeyGetMask ���");
        }
        else
        {
            Debug.LogWarning("KeyGetMask ���δ���䣡");
        }
    }
}