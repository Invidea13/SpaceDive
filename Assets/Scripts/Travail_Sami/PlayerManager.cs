using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public Slider oxBar;
    public CharacterController controller;
    public Transform groundCheck;
    public Image fadeIn;
    public RawImage oxUI;
    public Texture[] oxTextures;

    public AudioSource[] AudioList;
    public AudioClip[] ClipList;

    public GameObject flashLight;
    public GameObject mapUI;
    public GameObject[] oxBars;

    public Animator FlashlightAnim;

    public Vector3 velocity;
    bool isGrounded;

    public float jetpackForce = 10f;
    public float jetpackDelay = 0f;

    public float moveSpeed;
    public float sprintSpeed = 10f;
    public float moveSpeedBase = 5f;
    public float jumpHeight = 3f;

    public float gravity = -9.81f;
    public float groundDistance = 0.4f;

    public float fuelUse = 0.0001f;

    public float oxBreatheBase = 0.00001f;
    public float oxBreathe;
    public float oxSprint = 0.00005f;
    public float oxTimeDelay = 3f;

    public int OxBarNumber = 6;

    public bool canJump = true;
    public bool usingJetpack = false;
    public bool consOx = true;
    public bool flashOn = false;
    public bool mapOn = false;

    public LayerMask groundMask;

    void Start()
    {
        moveSpeed = moveSpeedBase;
        oxBreathe = oxBreatheBase;

        Destroy(fadeIn, 1f);
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        oxBar.value -= oxBreatheBase;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = sprintSpeed;
            oxBreathe = oxSprint;
        }

        else
        {
            moveSpeed = moveSpeedBase;
            oxBreathe = oxBreatheBase;
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
            jetpackDelay = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        oxTimeDelay -= Time.deltaTime;

        Jetpack();
        Flashlight();
        OxBarModif();
        MovementAudio();
        MapRollup();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Pickup_1") && oxBar.value < 1.2f && !AudioList[5].isPlaying)
        {
            oxBar.value += 0.2f;
            AudioList[5].PlayOneShot(ClipList[0]);
            Destroy(other.gameObject);
        }

        if(other.CompareTag("Pickup_2") && !AudioList[5].isPlaying)
        {
            consOx = false;
            AudioList[5].PlayOneShot(ClipList[0]);
            Destroy(other.gameObject);
        }
    }

    public void Flashlight()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && flashOn == false)
        {
            flashLight.SetActive(true);
            FlashlightAnim.SetTrigger("isActive");
            AudioList[8].PlayOneShot(ClipList[4]);
            flashOn = true;
        }

        else if (Input.GetKeyDown(KeyCode.Mouse1) && flashOn == true)
        {
            flashLight.SetActive(false);
            FlashlightAnim.SetTrigger("isActive");
            AudioList[9].PlayOneShot(ClipList[5]);
            flashOn = false;
        }
    }

    public void Jetpack()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            jetpackDelay += Time.deltaTime;

            if(jetpackDelay >= 0.35f)
            {
                usingJetpack = true;

                if (usingJetpack == true)
                {
                    velocity.y += jetpackForce * Time.deltaTime;

                    if (!AudioList[2].isPlaying)
                    {
                        AudioList[2].Play();
                    }

                    if (velocity.y >= 9f)
                    {
                        velocity.y = 9f;
                    }

                    if (consOx)
                    { 
                        oxBar.value -= fuelUse; 
                    }
                }
            }
        }

        else
        {
            usingJetpack = false;
            velocity.y -= Time.deltaTime;
            AudioList[2].Stop();
        }

    }

    public void OxBarModif()
    {
        if(oxBar.value <= 1.2f)
        {
            oxUI.texture = oxTextures[0];
            OxBarNumber = 6;
        }

        if(oxBar.value <= 1f)
        {
            oxUI.texture = oxTextures[1];
            OxBarNumber = 5;
        }

        if (oxBar.value <= 0.8f)
        {
            oxUI.texture = oxTextures[2];
            OxBarNumber = 4;
        }

        if (oxBar.value <= 0.6f)
        {
            oxUI.texture = oxTextures[3];
            OxBarNumber = 3;
        }

        if (oxBar.value <= 0.4f)
        {
            oxUI.texture = oxTextures[4];
            OxBarNumber = 2;
        }

        if (oxBar.value <= 0.2f)
        {
            oxUI.texture = oxTextures[5];

            OxBarNumber = 1;
        }
        
        if(oxBar.value <= 0f)
        {
            oxUI.texture = oxTextures[6];
            OxBarNumber = 0;

            AudioList[0].Stop();
            AudioList[1].Stop();
            AudioList[2].Stop();
        }
    }

    public void MovementAudio()
    {
        if(Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (!AudioList[0].isPlaying)
            {
                AudioList[0].Play();
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                AudioList[0].Stop();
                    
                if (!AudioList[1].isPlaying)
                {
                    AudioList[1].Play();
                }
            }
        }

        else
        {
            AudioList[0].Stop();
        }

        if (isGrounded == false)
        {
            AudioList[0].Stop();
            AudioList[1].Stop();
        }

        if(AudioList[0].isPlaying)
        {
            AudioList[1].Stop();
        }
    }

    public void MapRollup()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && mapOn == false)
        {
            mapUI.SetActive(true);
            mapOn = true;
        }

        else if (Input.GetKeyDown(KeyCode.Tab) && mapOn == true)
        {
            mapUI.SetActive(false);
            mapOn = false;
        }
    }

}
