using UnityEngine;

namespace Complete
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]
    public class TankMovement : MonoBehaviour
    {
        public int m_PlayerNumber = 1;
        public float m_Speed = 12f;
        public float m_TurnSpeed = 180f;

        public AudioSource m_MovementAudio;   // se auto-asigna en Awake si está vacío
        public AudioClip m_EngineIdling;
        public AudioClip m_EngineDriving;
        public float m_PitchRange = 0.2f;

        private string m_MovementAxisName;
        private string m_TurnAxisName;
        private Rigidbody m_Rigidbody;
        private float m_MovementInputValue;
        private float m_TurnInputValue;
        private float m_OriginalPitch;
        private ParticleSystem[] m_particleSystems;

        private void Awake()
        {
            // Auto-asignaciones seguras
            m_Rigidbody = GetComponent<Rigidbody>();
            if (m_MovementAudio == null)
                m_MovementAudio = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            m_Rigidbody.isKinematic = false;

            m_MovementInputValue = 0f;
            m_TurnInputValue = 0f;

            m_particleSystems = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < m_particleSystems.Length; ++i)
                m_particleSystems[i].Play();
        }

        private void OnDisable()
        {
            m_Rigidbody.isKinematic = true;

            for (int i = 0; i < m_particleSystems.Length; ++i)
                m_particleSystems[i].Stop();
        }

        private void Start()
        {
            // OJO: si no tienes ejes numerados, usa "Vertical"/"Horizontal" (línea comentada).
            m_MovementAxisName = "Vertical" + m_PlayerNumber;
            m_TurnAxisName     = "Horizontal" + m_PlayerNumber;
            // m_MovementAxisName = "Vertical";   // <- descomenta estas dos si solo tienes ejes estándar
            // m_TurnAxisName     = "Horizontal";

            if (m_MovementAudio != null)
            {
                m_OriginalPitch = m_MovementAudio.pitch;
                m_MovementAudio.loop = true;

                // Asegura que suene algo desde el principio (idle)
                if (m_EngineIdling != null)
                {
                    m_MovementAudio.clip = m_EngineIdling;
                    m_MovementAudio.Play();
                }
            }
        }

        private void Update()
        {
            m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
            m_TurnInputValue     = Input.GetAxis(m_TurnAxisName);

            EngineAudio();
        }

        private void EngineAudio()
        {
            if (m_MovementAudio == null) return;

            bool quieto = Mathf.Abs(m_MovementInputValue) < 0.1f &&
                          Mathf.Abs(m_TurnInputValue)     < 0.1f;

            if (quieto)
            {
                if (m_MovementAudio.clip != m_EngineIdling && m_EngineIdling != null)
                {
                    m_MovementAudio.clip  = m_EngineIdling;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
            else
            {
                if (m_MovementAudio.clip != m_EngineDriving && m_EngineDriving != null)
                {
                    m_MovementAudio.clip  = m_EngineDriving;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
        }

        private void FixedUpdate()
        {
            Move();
            Turn();
        }

        private void Move()
        {
            Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }

        private void Turn()
        {
            float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
        }

        // Opcional: ayuda en el Editor para evitar olvidos
        private void OnValidate()
        {
            if (m_Speed < 0f)      m_Speed = 0f;
            if (m_TurnSpeed < 0f)  m_TurnSpeed = 0f;
        }
    }
}
