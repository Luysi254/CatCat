using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemycontrol : MonoBehaviour
{
        [Header("Ŀ������")]
        public Transform player;

        [Header("׷������")]
    public float detectionRange = 50f; // ������
    public float chargeRange = 20f;    // ��̷�Χ
    public float chargeSpeed = 35f;    // ����ٶ�
    public float normalSpeed = 15f;    // ��ͨ׷���ٶ�
    public float cooldownAfterCharge = 2f; // ��ȴʱ��
    public float rotationSpeed = 10f;  // ��ת�ٶ�


    [Header("���")]
        public AudioSource chainSound;

        private Rigidbody rb;
        private bool isCharging = false;
        private bool isChasing = false;
        private float chargeTimer = 0f;

        void Start()
        {
            rb = GetComponent<Rigidbody>();

            // ��ȡAudioSource�����δ���ã�
            if (chainSound == null)
                chainSound = GetComponent<AudioSource>();

            // �޸���ײ��
            BoxCollider collider = GetComponent<BoxCollider>();
            if (collider != null)
            {
                collider.size = new Vector3(1f, 2f, 1f);
                collider.center = new Vector3(0, 1f, 0);
            }

            // ��������Լ��
            rb.constraints = RigidbodyConstraints.FreezePositionY |
                             RigidbodyConstraints.FreezeRotation;
        }

        void Update()
        {
            // ȷ��player������Ч
            if (player == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null) player = playerObj.transform;
                else return;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            Debug.Log($"����Ҿ���: {distanceToPlayer}");

            // ������
            if (distanceToPlayer <= detectionRange)
            {
                isChasing = true;

                // ��̴���
                if (distanceToPlayer <= chargeRange && !isCharging && chargeTimer <= 0)
                {
                    StartCharge();
                }
            }
            else
            {
                isChasing = false;
            }

            // ��ȴ����
            if (chargeTimer > 0) chargeTimer -= Time.deltaTime;

            // ��������
            UpdateSound();

            // ״̬��ʾ
            UpdateStatusDisplay();
        }

        void FixedUpdate()
        {
            // ȷ����׷��״̬�²��ƶ�
            if (isChasing && player != null)
            {
                Vector3 direction = (player.position - transform.position).normalized;

                if (isCharging)
                {
                    ChargeTowards(direction);
                }
                else
                {
                    NormalChase(direction);
                }
            }
            else
            {
                // ��׷��״ֹ̬ͣ�ƶ�
                rb.velocity = Vector3.zero;
            }
        }

        private void StartCharge()
        {
            isCharging = true;
            chargeTimer = 0;
            chainSound?.Stop();
        }

        private void ChargeTowards(Vector3 direction)
        {
            rb.velocity = direction * chargeSpeed;
        }

        private void NormalChase(Vector3 direction)
        {
            // ��ȡ��ȫ���򣨱����ϰ���
            Vector3 safeDirection = GetSafeDirection(direction);

            // ������ת
            if (safeDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(safeDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                                                   rotationSpeed * Time.fixedDeltaTime);
            }

            // ��Ŀ�귽���ƶ�
            rb.velocity = safeDirection * normalSpeed;
        }

        private Vector3 GetSafeDirection(Vector3 targetDirection)
        {
            // �򵥵��ϰ������ر�
            RaycastHit hit;
            Vector3[] testDirections = new Vector3[]
            {
            targetDirection,
            Quaternion.Euler(0, 30, 0) * targetDirection,
            Quaternion.Euler(0, -30, 0) * targetDirection,
            Quaternion.Euler(0, 60, 0) * targetDirection,
            Quaternion.Euler(0, -60, 0) * targetDirection
            };

            foreach (Vector3 dir in testDirections)
            {
                if (!Physics.Raycast(transform.position, dir, out hit, 3f))
                {
                    return dir.normalized;
                }
            }

            // ���з������裬ת���
            return -targetDirection;
        }

        void OnCollisionEnter(Collision collision)
        {
            // ���״̬����ײ���
            if (isCharging && collision.gameObject.CompareTag("Player"))
            {
                CatController cat = collision.gameObject.GetComponent<CatController>();
                if (cat != null)
                {
                    cat.GetTrapped();
                    StopCharge();
                }
            }
            // ��ײǽ�ڽ������
            else if (isCharging && collision.gameObject.CompareTag("Wall"))
            {
                StopCharge();
            }
        }

        private void StopCharge()
        {
            isCharging = false;
            rb.velocity = Vector3.zero;
            chargeTimer = cooldownAfterCharge;
        }

        private void UpdateSound()
        {
            if (isChasing && !chainSound.isPlaying && !isCharging)
            {
                chainSound.Play();
            }
            else if ((!isChasing || isCharging) && chainSound.isPlaying)
            {
                chainSound.Stop();
            }
        }

        private void UpdateStatusDisplay()
        {
            string state = isChasing ? (isCharging ? "���" : "׷��") : "����";
            Debug.Log($"����״̬: {state}, ��ȴ: {chargeTimer.ToString("F2")}");
        }

        // ���Կ��ӻ�
        void OnDrawGizmosSelected()
        {
            // ��ⷶΧ
            Gizmos.color = new Color(1, 1, 0, 0.3f); // ��͸����ɫ
            Gizmos.DrawWireSphere(transform.position, detectionRange);

            // ��̷�Χ
            Gizmos.color = new Color(1, 0, 0, 0.3f); // ��͸����ɫ
            Gizmos.DrawWireSphere(transform.position, chargeRange);

            // ����ҵ���
            if (player != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, player.position);
            }
        }
    }

