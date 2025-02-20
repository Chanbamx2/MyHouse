using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator ani;
    SpriteRenderer render;

    bool isLoadScene = false;
    bool canEvent = false;

    public GameObject miniGame;

    private Vector3 savedPosition;

    private void Awake()
    {
        Time.timeScale = 1.0f;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        render = rb.GetComponent<SpriteRenderer>();

        if (PlayerPrefs.HasKey("PlayerPosX"))
        {
            float x = PlayerPrefs.GetFloat("PlayerPosX");
            float y = PlayerPrefs.GetFloat("PlayerPosY");
            float z = PlayerPrefs.GetFloat("PlayerPosZ");

            savedPosition = new Vector3(x, y, z);
            transform.position = savedPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 moveDirection = new Vector2(horizontal, vertical).normalized;
        rb.velocity = moveDirection * 5f;

        if (rb.velocity != Vector2.zero)
        {
            ani.SetBool("IsRun", true);
        }
        else ani.SetBool("IsRun", false);

        if (horizontal != 0)
        {
            render.flipX = horizontal < 0; 
        }

        if (canEvent)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SavePlayerPosition();
                Debug.Log("Input K");
                SceneManager.LoadScene("MiniGameScene");
                isLoadScene = true;
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Event"))
        {
            transform.Find("Canvas").gameObject.SetActive(true);
            miniGame.SetActive(true);
            canEvent = true;
        }

        if (collision.CompareTag("MiniGameButton"))
        {
            Vector3 miniGamePos = miniGame.transform.position;
            miniGamePos.x += miniGame.transform.position.x < -4.2 ? 1.5f : -1.5f;

            miniGame.transform.position = miniGamePos;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isLoadScene)
        {
            transform.Find("Canvas").gameObject.SetActive(false);
            miniGame.SetActive(false);
            canEvent = false;
        }
    }

    public void SavePlayerPosition()
    {
        PlayerPrefs.SetFloat("PlayerPosX", transform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", transform.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", transform.position.z);
        PlayerPrefs.Save();
    }
}
