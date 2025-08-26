using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public static readonly int IdReload = Animator.StringToHash("Reload");

    public Gun gun;

    private Vector3 gunInitPos;
    private Quaternion gunInitRot;

    private Rigidbody gunRb;
    private Collider gunCollider;

    public Transform gunPivot;
    public Transform leftHandMount;
    public Transform rightHandMount;

    private PlayerInput input;
    private Animator animator;


    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        gunRb = gun.GetComponent<Rigidbody>();
        gunCollider = gun.GetComponent<Collider>();

        gunInitPos = gun.transform.localPosition;
        gunInitRot = gun.transform.localRotation;
    }

    private void OnEnable()
    {
        gunRb.isKinematic = true;
        gunCollider.enabled = false;
        gun.transform.localPosition = gunInitPos;
        gun.transform.localRotation = gunInitRot;
    }

    private void OnDisable()
    {
        // 초기값으로 돌리기위해 
        gunRb.linearVelocity = Vector3.zero;
        gunRb.angularVelocity = Vector3.zero;

        gunRb.isKinematic = false;
        gunCollider.enabled = true;
    }

    private void Update()
    {
        if (input.Fire)
        {
            gun.Fire();
        }
        else if (input.Reload)
        {
            if (gun.Reload())
            {
                animator.SetTrigger(IdReload);
            }
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        gunPivot.position = animator.GetIKHintPosition(AvatarIKHint.RightElbow);

        // 왼쪽
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);

        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

        // 오른쪽
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);

        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }
}