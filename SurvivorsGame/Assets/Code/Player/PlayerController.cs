using UnityEngine;

namespace Code.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] 
        private float movementSpeed = 5f;
        
        private Rigidbody2D _rb;
        
        private Vector2 _movement;
        
        private GameObject _attackArea;

        private void Start()    //instantiate player values
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.position = new Vector2(0, 0);
            _attackArea =  transform.GetChild(0).gameObject;
        }
    
        private void Update()       //checks input and adds it into a movement vector
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            _movement = new Vector2(horizontal, vertical);
        }

        private void FixedUpdate()  //moves player in space and rotates the attack hitbox dependant on the direction
        {
            MoveCharacter(_movement);
            var velocity = _rb.velocity;
            if (velocity == Vector2.zero) return;
            var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            _attackArea.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    
        public void IncreaseMovementSpeed(float speed)   //increases movement speed
        {
            movementSpeed += speed;
        }

        private void MoveCharacter(Vector2 direction)   //velocity function
        {
            _rb.velocity = direction * movementSpeed;
        }
    }
}
