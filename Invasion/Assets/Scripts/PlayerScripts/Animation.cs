using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace PlayerScripts
{

    [RequireComponent(typeof(CharacterController))]

    public class Animation : MonoBehaviour
    {
        private Animator _animator;

        private CharacterController _characterController;
        // Start is called before the first frame update

        private Player _player;
        void Start()
        {
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            bool isWalking = Input.GetKey(KeyCode.LeftShift);
            float vert = Input.GetAxis("Vertical");
            float hori = Input.GetAxis("Horizontal");
            _animator.SetBool("IsWalkingFront", isWalking && vert > 0);
            _animator.SetBool("IsWalkingBack", isWalking && vert < 0);
            _animator.SetBool("IsRunningFront", !isWalking && vert > 0);
            _animator.SetBool("IsRunningBack", !isWalking && vert < 0);
            _animator.SetBool("IsWalkingLeft", isWalking && hori < 0);
            _animator.SetBool("IsWalkingRight", isWalking && hori > 0);
            _animator.SetBool("IsRunningLeft", !isWalking && hori < 0);
            _animator.SetBool("IsRunningRight", !isWalking && hori > 0);
            _animator.SetBool("IsJumping", Input.GetKeyDown(KeyCode.Space));
        }
    }
}
