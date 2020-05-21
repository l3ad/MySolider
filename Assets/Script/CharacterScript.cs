using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : CStat
{
    public ETCJoystick eTC;
    Rigidbody2D rb;
    Transform tr;
    Animator anim;
    public string thisTag;
    public int traningStat;
    public float Xtrans;
    public bool[] normalMove = new bool[7] {true,false,false,false, false, false, false }; // Idle,Walk Jump atack skill1 skill2 skill3

    public float[] skillCooldown=new float[3];

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        if (this.gameObject.tag == "Player")
        {
            anim.SetFloat("SpeedAtack", 1 + (Aglity.GetValue() / 10));
        }
        else Destroy(this);
    }

    private void FixedUpdate()
    {

            Xtrans = eTC.axisX.axisValue;
            if (Xtrans > 0 && this.gameObject.transform.localScale.x < 0) this.gameObject.transform.localScale = new Vector3(.6f, .6f, 1);
            else if (Xtrans < 0 && this.gameObject.transform.localScale.x > 0) this.gameObject.transform.localScale = new Vector3(-.6f, .6f, 1);
            rb.velocity = new Vector2(Xtrans * (1+moveSpeed.GetValue()/10), rb.velocity.y);
            //AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (!normalMove[2])
            {
                if (Xtrans == 0)
                {
                    normalMove[0] = true;
                    normalMove[1] = false;
                    anim.SetBool("Walk", false);
                    anim.SetBool("Idle", true);
                }
                else
                {
                    normalMove[0] = false;
                    normalMove[1] = true;
                    anim.SetBool("Idle", false);
                    anim.SetBool("Walk", true);
                    anim.SetFloat("SpeedMove", Xtrans * (1 + moveSpeed.GetValue() / 100));
                }
            }
    }

    public void JumpFunction()
    {
        if (!normalMove[2]){normalMove[2]=true; normalMove[0] = false;normalMove[1] = false;
            anim.SetBool("Jump", true); anim.SetBool("Idle", false); anim.SetBool("Walk", false);
            rb.AddForce(new Vector2(0,1)*10,ForceMode2D.Impulse);}
    }
    public void AtackFunction()
    {
        if (!normalMove[3]) { normalMove[3]=true; anim.SetBool("Atack", true); }
    }

    

    public void AtackActivation()
    {
        anim.SetBool("Atack", false);
        normalMove[3] = false; 
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            normalMove[2] = false;
            anim.SetBool("Jump", false);
        }
    }

    IEnumerable  SkillCoolDown(int index,float timeCooldown)
    {
        yield return new WaitForSeconds(timeCooldown);
        normalMove[3+index] = false;
    }

}
