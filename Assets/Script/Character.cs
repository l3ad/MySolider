using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Transform cameraMain;
    private Animator anim;
    private int orginalStat = 0; // -1 mord , 0 ro hava , 1 ro zamin, 2 be upground , 3 divar chap,4 divar rast
    private int stat = 0;
    private float horizontalSpeed = 5;
    private float Vertical = 5;
    private float x=0;
    private float y=0;
    bool normal = true;
    Rigidbody2D rb;
    float timeOnGround=0;
    float timeIdle=0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ground":
                if (orginalStat == 0) orginalStat = 1;
                else if (x > 0 && orginalStat == 3) orginalStat = 1;
                else if (x < 0 && orginalStat == 4) orginalStat = 1;
                break;
            case "UpGround":
                if (orginalStat == 0) orginalStat = 2;
                else if (x > 0 && orginalStat == 3) orginalStat = 2;
                else if (x < 0 && orginalStat == 4) orginalStat = 2;
                break;
            case "DivarChap":
                if(orginalStat==0) orginalStat = 3;
                else if(x<0) orginalStat = 3;
                break;
            case "DivarRast":
                if (orginalStat == 0) orginalStat = 4;
                else if (x > 0) orginalStat = 4;
                break;
            default:
                Debug.Log("ridi in collider tag nadare");
                break;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ground":
                if(orginalStat==1) StartCoroutine(JumpFromGround());
                break;
            case "UpGround":
                if (orginalStat == 2) StartCoroutine(JumpFromGround());
                break;
            case "DivarChap":
                if (orginalStat == 3 && x>0)
                {
                    RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.down, 1.5f,1<<LayerMask.NameToLayer("Tools"));
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.CompareTag("Ground"))
                        {
                            orginalStat = 1;
                        }
                        else if (hit.collider.gameObject.CompareTag("UpGround"))
                        {
                            orginalStat = 2;
                        }
                    }
                    else { orginalStat = 0; }
                }
                break;
            case "DivarRast":
                if (orginalStat == 4 && x < 0)
                {
                    RaycastHit2D hitt = Physics2D.Raycast(this.transform.position, Vector2.down, 1.5f, 1 << LayerMask.NameToLayer("Tools"));
                    if (hitt.collider != null)
                    {
                        if (hitt.collider.gameObject.CompareTag("Ground"))
                        {
                            orginalStat = 1;
                        }
                        else if (hitt.collider.gameObject.CompareTag("UpGround"))
                        {
                            orginalStat = 2;
                        }
                    }
                    else { orginalStat = 0; }
                }
                break;
            default:
                Debug.Log("ridi in collider tag nadare");
                break;
        }
    }

    IEnumerator JumpFromGround()
    {
        yield return new WaitForSeconds(.1f);
        RaycastHit2D hits = Physics2D.Raycast(this.transform.position, Vector2.down, 0.1f, 1 << LayerMask.NameToLayer("Tools"));
        if (hits.collider != null)
        {
            if (hits.collider.gameObject.CompareTag("Ground"))
            {
                orginalStat = 1;
            }
            else if (hits.collider.gameObject.CompareTag("UpGround"))
            {
                orginalStat = 2;
            }
        }
        else
        {
            orginalStat = 0;
            timeOnGround = 0;

        }
    }

    void Start()
    {
        cameraMain = Camera.main.transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        cameraMain.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
    private void FixedUpdate()
    {
        TransformChangeOnStat();
    }

    private void TransformChangeOnStat()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        if (x == 0 ) timeIdle += Time.deltaTime;
        else timeIdle = 0;
        switch (orginalStat)
        {
            case -1:
                Debug.Log("death");
                break;
            case 0:
                if (x > 0) transform.localScale = new Vector3(1, 1, 1);
                else if (x < 0) transform.localScale = new Vector3(-1, 1, 1);
                transform.Translate(x * horizontalSpeed * Time.deltaTime, 0, 0);
                RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.down, 3.4f, 1 << LayerMask.NameToLayer("Tools"));
                if (hit.collider != null)
                {
                    if ( rb.velocity.y < 0) anim.SetFloat("AnimStat", 5);
                    else if (rb.velocity.y > 0 ) anim.SetFloat("AnimStat", 4);
                }
                else {anim.SetFloat("AnimStat", 4); }
                break;
            case 1:
                timeOnGround += Time.deltaTime;
                if(timeOnGround>.3f) transform.Translate(x * horizontalSpeed * Time.deltaTime, 0, 0);
                if (x>0) transform.localScale = new Vector3(1, 1, 1);
                else if(x<0) transform.localScale = new Vector3(-1, 1, 1);
                if (x != 0 && timeOnGround > .3f)
                {
                    anim.SetFloat("AnimStat", 1);
                }
                else if (timeIdle > .05f && orginalStat==1) { anim.SetFloat("AnimStat", 0);  } 
                break;
            case 2:
                timeOnGround += Time.deltaTime;
                if (timeOnGround > .3f) transform.Translate(x * horizontalSpeed * Time.deltaTime, 0, 0);
                if (x > 0) transform.localScale = new Vector3(1, 1, 1);
                else if (x < 0) transform.localScale = new Vector3(-1, 1, 1);
                if (x != 0 && timeOnGround > .3f)
                {
                    anim.SetFloat("AnimStat", 1);
                }
                else if (timeIdle > .05f && orginalStat == 1) anim.SetFloat("AnimStat", 0);
                break;
            case 3:
                anim.SetFloat("AnimStat", 3);
                transform.localScale = new Vector3(1, 1, 1);
                if (x>0) transform.Translate(x * horizontalSpeed * Time.deltaTime, 0, 0);
                break;
            case 4:
                anim.SetFloat("AnimStat", 3);
                transform.localScale = new Vector3(-1, 1, 1);
                if (x < 0) transform.Translate(x * horizontalSpeed * Time.deltaTime, 0, 0);
                break;
            default:
                Debug.Log("statesh malom nist");
                break;
        }
    }

    public void JumpFunction()
    {
        if((orginalStat==1  || (orginalStat == 2 && normal)) && y>0)
        {
            timeOnGround = 0;
            orginalStat = 0;
            rb.AddForce(new Vector2(0, y)*30, ForceMode2D.Impulse);
            anim.SetFloat("AnimStat", 2);
        }
    }


}
