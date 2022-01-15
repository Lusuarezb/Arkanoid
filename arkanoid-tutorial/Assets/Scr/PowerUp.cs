using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUp : MonoBehaviour
{
    // Basico
   [SerializeField] private float _speed = 1;
    private Rigidbody2D _rb;
    private Collider2D _collider;

    // Tipo
    private List<string> _types = new List<string>{"Small", "Large", "Fast", "Slow", "Multiball"};
    int _Rand;

    // Elementos

    public void Initialize()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _rb.velocity = Vector2.down * _speed;
        SelectType();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Si choca con el paddle se apaga y activa el efecto
        if(other.transform.TryGetComponent<Paddle>(out Paddle paddle)){
            _collider.enabled = false;
            gameObject.SetActive(false);

            // Activar el efecto
            PickUp();
        }

        // Si choca con la pared solo se apaga
        if(other.transform.TryGetComponent<DeadZone>(out DeadZone deadZone)){
            _collider.enabled = false;
            gameObject.SetActive(false);
        }
    }

    public void SelectType()
    {
        _Rand = Random.Range(0,_types.Count);
        string type = _types[_Rand];

        // Si el tipo es Small
        if (type == "Small"){
            gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PowerUps/PowerUp_Small");
        }
        
        // Si el tipo es Large
        if (type == "Large"){
            gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PowerUps/PowerUp_Large");            
        }

        // Si el tipo es Fast
        if (type == "Fast"){
            gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PowerUps/PowerUp_Fast");            
        }

        // Si el tipo es Slow
        if (type == "Slow"){
            gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PowerUps/PowerUp_Slow");
        }

        // Si el tipo es Multiball
        if (type == "Multiball"){
            gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PowerUps/PowerUp_Multiball");
        }

    }
    
    private void PickUp()
    {
        string type = _types[_Rand];

        if (type == "Small"){
            GameObject paddle = GameObject.Find("Paddle");
            Vector3 size = paddle.transform.localScale;
            
            if(size.x >= 0.8f){
                paddle.transform.localScale = size - new Vector3(0.2f, 0, 0);
            }
        }
        
        // Si el tipo es Large
        if (type == "Large"){
            GameObject paddle = GameObject.Find("Paddle");
            Vector3 size = paddle.transform.localScale;
            
            if(size.x <= 1.2f){
                paddle.transform.localScale = size + new Vector3(0.2f, 0, 0);
            }
        }

        // Si el tipo es Fast
        if (type == "Fast"){
            GameObject ball = GameObject.Find("Ball(Clone)");
            Ball script = ball.GetComponent<Ball>();

            Vector2 velocity = script.GetCurrentSpeed();
            if(velocity.magnitude <= script.GetMaxSpeed()){
                velocity = velocity * 1.2f;
                script.SetVelocity(velocity);
            }
        }

        // Si el tipo es Slow
        if (type == "Slow"){
            GameObject ball = GameObject.Find("Ball(Clone)");
            Ball script = ball.GetComponent<Ball>();

            Vector2 velocity = script.GetCurrentSpeed();
            if(velocity.magnitude >= script.GetMinSpeed()){
                velocity = velocity * 0.8f;
                script.SetVelocity(velocity);
            }
        }

        // Si el tipo es Multiball
        if (type == "Multiball"){
            GameObject arkanoidController = GameObject.Find("ArkanoidController");
            GameObject ball = GameObject.Find("Ball(Clone)");
            ArkanoidController script = arkanoidController.GetComponent<ArkanoidController>();
            
            int ballCount = script._balls.Count;
            while(ballCount < 3){
                Ball newBall = script.CreateBallAt(ball.transform.position);
                newBall.Init();
                script._balls.Add(newBall);
                ballCount++;
            }
        }
    }
}
