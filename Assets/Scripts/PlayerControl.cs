using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    private Rigidbody rb;
    public Text countText;
    public float speed;
    private int count;

    void Start()
    {
        count = 0;
        SetCountText();
        rb = GetComponent<Rigidbody>();
              
    }

    void FixedUpdate()
    {
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }
    void OnTriggerEnter( Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count +1;
            SetCountText();
        }
    }
    void SetCountText() 
    {
        countText.text = "Count: " + count.ToString(); 
    }

}
