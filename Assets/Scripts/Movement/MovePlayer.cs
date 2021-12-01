using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MovePlayer : MonoBehaviour
{
    [SerializeField] private CameraStateMachine instance;
    [SerializeField] public CharacterController2D controller;
    [SerializeField] private PauseMenu pause;
    [SerializeField] public Animator animator;

    [SerializeField] private GameObject m_AttackPrefab;
    [SerializeField] private GameObject blackOutSquare;

    [Range(0, 100f)] [SerializeField] private float m_MoveSpeed = 40f;
    [SerializeField] private float m_AttackSpeed;

    CameraStateMachine.Zone m_MyZone;

    private float m_HorizontalMove = 0;
    private float m_LastY;
    private bool m_Jumped = false;
    public bool m_GameOver = false;
    public bool m_CanClimb = false;
    private bool m_DeathAnimation = false;
    private bool m_Bubble = false;
    public bool mReset = false;

    public bool m_InCutscene = false;
    public bool m_CanJump = true;

    private int mHealth = 3;
    private float mInvincibleTimer = 0.0f;
    private bool mInvincible = false;

    private void Awake()
    {
        if (MetricManager.instance.m_NumDeaths == 0)
        {
            AudioManager.instance.spawnPoint = transform.position;
            AudioManager.instance.warpZone = CameraStateMachine.Zone.Heart;
        } else
        {
            transform.position = AudioManager.instance.spawnPoint;
            instance.m_Zone = AudioManager.instance.warpZone;
            Time.timeScale = 1.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mReset)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (m_GameOver)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerDie"))
            {
                m_DeathAnimation = true;
            }
            if (m_DeathAnimation && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerDie"))
            {
                Time.timeScale = 0.0f;
                mReset = true;
            }
        }
        else if (!m_InCutscene && !pause.m_Paused)
        {
            m_HorizontalMove = Input.GetAxisRaw("Horizontal") * m_MoveSpeed;

            if (m_HorizontalMove != 0 && controller.m_Grounded == true)
            {
                AudioManager.instance.PlayFootstep();
            }

            if (Input.GetKeyDown(KeyCode.Space) && m_CanJump)
            {
                animator.SetTrigger("Jumping");
                m_Jumped = true;
                // Landing animation trigger is handled by CharacterController2D
            }
            if (m_CanClimb && Input.GetKey(KeyCode.W))
            {
                controller.m_Climbing = true;
            } else
            {
                controller.m_Climbing = false;
            }

            /*
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (instance.m_Zone != CameraStateMachine.Zone.ZOOM)
                {
                    m_MyZone = instance.m_Zone;
                    instance.m_Zone = CameraStateMachine.Zone.ZOOM;
                }
                else
                {
                    instance.m_Zone = m_MyZone;
                }
            }
            */

            if (Input.GetMouseButtonDown(0) && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttack"))
            {
                animator.SetTrigger("Attacking");
                AudioManager.instance.Attack();
                GameObject attack = Instantiate(m_AttackPrefab, transform.position, m_AttackPrefab.transform.rotation);
                if (!controller.m_FacingRight)
                {
                    Vector3 theScale = attack.transform.localScale;
                    theScale.x *= -1;
                    attack.transform.localScale = theScale;
                    attack.GetComponent<Rigidbody2D>().velocity = new Vector3(-1 * m_AttackSpeed, 0.0f, 0.0f);
                } else
                {
                    attack.GetComponent<Rigidbody2D>().velocity = new Vector3(m_AttackSpeed, 0.0f, 0.0f);
                }
            }

            animator.SetFloat("Horizontal", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
            m_LastY = transform.position.y;
        }
    }

    private void FixedUpdate()
    {
        if (!m_InCutscene)
        {
            controller.Move(m_HorizontalMove * Time.fixedDeltaTime, false, m_Jumped);
            m_Jumped = false;
            if (!Input.anyKey && controller.m_Grounded && !m_Bubble)
            {
                transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            }
            else if (Input.anyKey && !m_GameOver)
            {
                transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GameOver") && !m_GameOver)
        {
            Die();
        }
        if (collision.gameObject.CompareTag("Bubble"))
        {
            MetricManager.instance.AddBubble();
            m_Bubble = true;
        } else
        {
            m_Bubble = false;
        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            StartCoroutine("FinalFade");
        }
    }

    public void Die()
    {
        animator.SetTrigger("Died");
        MetricManager.instance.AddDeath();
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        AudioManager.instance.Play("GameOver");
        m_GameOver = true;
    }

    public void TakeDamage()
    {
        if (!mInvincible)
        {
            mHealth--;
            if (mHealth > 0)
            {
                mInvincibleTimer = 0.0f;
                mInvincible = true;
                AudioManager.instance.PlayAcid();
                StartCoroutine(Blinking());
            }
            else
            {
                Die();
            }
        }
    }

    public IEnumerator Blinking()
    {
        while (mInvincibleTimer < 3.0f)
        {
            Color invincible = transform.gameObject.GetComponent<SpriteRenderer>().color;
            invincible.a = 0.5f;
            transform.gameObject.GetComponent<SpriteRenderer>().color = invincible;
            mInvincibleTimer += Time.deltaTime;
            if (mInvincibleTimer >= 3.0f)
            {
                invincible.a = 1.0f;
                transform.gameObject.GetComponent<SpriteRenderer>().color = invincible;
                mInvincible = false;
            } 
            yield return null;
        }
    }

    public IEnumerator FinalFade()
    {
        m_InCutscene = true;
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;
        while (blackOutSquare.GetComponent<Image>().color.a < 1)
        {
            fadeAmount = objectColor.a + (0.5f * Time.deltaTime);

            if (fadeAmount >= 1)
            {
                fadeAmount = 1;
                StopCoroutine("FadeOut");
                AudioManager.instance.Stop();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackOutSquare.GetComponent<Image>().color = objectColor;
            yield return null;
        }
    }
}
