
using UnityEngine;

public class AvatarDebug : MonoBehaviour
{
    private void Start()
    {
        Animator animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("No Animator found!");
            return;
        }

        Avatar avatar = animator.avatar;

        Debug.Log($"Avatar assigned: {avatar != null}");

        if (avatar != null)
        {
            Debug.Log($"Avatar valid: {avatar.isValid}");
            Debug.Log($"Avatar humanoid: {avatar.isHuman}");
        }

        Debug.Log(
            $"Animator initialized: {animator.isInitialized}"
        );
    }
}
