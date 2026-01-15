using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windControl : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isleft=false;
    private float hurt;
    private float speed = 3;
    float time = 0;
    void Start()
    {
        
    }

    public void SetInfo(bool IsLeft, float Hurt)
    {
        isleft = IsLeft;
        hurt = Hurt;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 5)
        {
            Destroy(this.gameObject);
        }
        if (isleft)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }


    //Åöµ½µÐÈË¾Í¿ÛÑª

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyControl>().beHurt(hurt);
        }
    }
    
}
