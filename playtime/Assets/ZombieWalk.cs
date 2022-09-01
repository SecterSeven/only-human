using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ZombieWalk : MonoBehaviour
{
    // Start is called before the first frame update
    Transform target;
    //public Transform borderCheck;
    public int enemyHP = 200;
    public Animator animator;
    public GameObject BloodPrefab;
    public Transform bloodTransform;
    //public Slider enemyHealthBar;

    private Rigidbody rbody;


    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        //enemyHealthBar.value = enemyHP;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Physics.IgnoreCollision(target.GetComponent<Collider>(), GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
        if(target.position.x > transform.position.x)
            transform.localScale = new Vector2(1f, 1f);
        else
            transform.localScale = new Vector2(-1f, 1f);

    }
    public void TakeDamage(int damageAmount)
    {
        enemyHP -= damageAmount;
        //enemyHealthBar.value = enemyHP;
        if (enemyHP > 0)
        {
            var go = Instantiate(BloodPrefab);
            go.transform.position = bloodTransform.position;
            animator.SetTrigger("damage");
	        animator.SetBool("isChasing", true);
            FindObjectOfType<AudioManager>().Play("Hit");
        }
        else
        {
            animator.SetTrigger("death");
            GetComponent<CapsuleCollider>().enabled = false;
            this. GetComponent<Rigidbody>(). useGravity = false;
            //enemyHealthBar.gameObject.SetActive(false);
            ScoreManager.instance.AddPoint();
            this.enabled = false;
        }
    }
}
