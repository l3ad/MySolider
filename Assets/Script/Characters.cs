using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{
    public NeuralNetwork net;
    Rigidbody2D rb;
    private Transform target;
    float[] vel = new float[2];

    float oldDis;


    float timeElaps = 0;
    public bool done = false;

    public  void SetNewNetwork(int[] layers)
    {
        net = new NeuralNetwork(layers);
    }
    public  void SetOfspringNetwork(NeuralNetwork newNet)
    {
        net =new NeuralNetwork(newNet);
    }
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("DivarChap").transform;
        rb = GetComponent<Rigidbody2D>();
        oldDis = Vector2.Distance(this.transform.position , target.position);
    }
    //2 input 2 output 1 hidden lyer

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!done)
        {
            timeElaps += Time.deltaTime;

            vel = net.FeedForward(new float[] {target.position.x, target.position.y, transform.position.x, transform.position.y, rb.velocity.x, rb.velocity.y });
            rb.velocity=new Vector2(vel[0], vel[1]);

            net.SetFitness(oldDis - Vector2.Distance(this.transform.position, target.position));

        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DivarChap")
        {
            net.AddFitness(oldDis);
            done = true;
        }
        else if(collision.tag == "UpGround")
        {
            net.AddFitness(-oldDis);
            done = true;
        }
    }


}
