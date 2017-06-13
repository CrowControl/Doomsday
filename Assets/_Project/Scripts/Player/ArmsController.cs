using System;
using PowerTools;
using UnityEngine;

namespace _Project.Scripts.Player
{
    [RequireComponent(typeof(SpriteAnimNodes))]
    [RequireComponent(typeof(ICharacterAimSource))]
    public class ArmsController : MonoBehaviour
    {
        #region Editor
        [SerializeField] private bool _rotateFrontArmToAim = true;
        [SerializeField] private bool _rotateBackArmToAim = false;
        #endregion

        #region Components
        private SpriteAnimNodes _animationNodes;
        private ICharacterAimSource _aimSource;

        private Arm _frontArm;
        private Arm _backArm;
        #endregion

        #region Properties

        public bool RotateFrontArmToAim
        {
            get { return _frontArm.CanRotate; }
            set { _frontArm.CanRotate = value; }
        }

        public bool RotateBackArmToAim
        {
            get { return _backArm.CanRotate;  }
            set { _backArm.CanRotate = value; }
        }
        
        public Quaternion FrontArmRotation
        {
            get { return _frontArm.Rotation; }
            set { _frontArm.Rotation = value; }
        }

        public Quaternion BackArmRotation
        {
            get { return _backArm.Rotation; }
            set { _backArm.Rotation = value; }
        }

        #endregion

        private void Awake()
        {
            _animationNodes = GetComponent<SpriteAnimNodes>();
            _aimSource = GetComponent<ICharacterAimSource>();

            //Get arms.
            _frontArm = new Arm(transform.Find("Front Arm"), _rotateFrontArmToAim);
            _backArm = new Arm(transform.Find("Back Arm"), _rotateBackArmToAim);
        }

        private void LateUpdate()
        {
            //Set arm positions.
            _frontArm.Update(_animationNodes.GetPosition(0), _aimSource);
            _backArm.Update(_animationNodes.GetPosition(1), _aimSource);
        }

        [Serializable]
        private class Arm
        {
            private readonly SpriteRenderer _renderer;
            private readonly Transform _transform;

            #region Properties
            public bool CanRotate { get; set; }

            public Quaternion Rotation
            {
                get { return _transform.rotation; }
                set { _transform.rotation = value; }
            }
            #endregion

            public Arm(Transform transform, bool canRotate = true)
            {
                _transform = transform;
                _renderer = transform.GetComponent<SpriteRenderer>();

                CanRotate = canRotate;
            }

            public void Update(Vector3 newPosition, ICharacterAimSource aimSource)
            {
                _transform.position = newPosition;

                if(CanRotate)
                    RotateTowards(aimSource);
            }

            private void RotateTowards(ICharacterAimSource aimSource)
            {
                //Rotate arm to where the player is aiming.
                float degree = aimSource.AimingDegree;
                _transform.rotation = Quaternion.AngleAxis(degree, Vector3.forward);

                //The shooting arm sprite needs to be flipped if we're aiming at the right side of the screen. 
                //If your wondering why, try commenting the line below.
                _renderer.flipY = Mathf.Abs(degree) < 90;
            }
        }
    }
}
