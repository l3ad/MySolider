using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CStat : MonoBehaviour
{
    public Stat healthPoint;
    public Stat strenght;
    public Stat defence;
    public Stat Aglity;
    public Stat moveSpeed;
    public Stat dexterity;
    public Stat dodge;
    public Stat recovery;

    public int Health
    {
        get
        {
            return _Health;
        }
        set
        {
            _Health = value;
            if (_Health <= 0)
            {
                Die();
            }
        }
    }
    private int _Health;
    private void Start()
    {
        
    }

    public void TakeDamage(int damage,int dex)
    {
        if (Random.Range(0, dex + dodge.GetValue()) > dodge.GetValue() || dodge.GetValue()==0)
        {
            damage -= defence.GetValue();
            damage = Mathf.Clamp(damage, 0, int.MaxValue);
            Health -= damage;
        }
        else
        {
            //dodge
        }
    }

    public virtual void Die()
    {
        //this method for ovverwrite
    }
}
