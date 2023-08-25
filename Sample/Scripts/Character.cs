using UnityEngine;

namespace Navigation.Sample.Scripts
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private CharacterController _characterController;
        
        public void Move(Vector3 direction)
        {
            _characterController.SimpleMove(direction * _speed);
        }
    }
}