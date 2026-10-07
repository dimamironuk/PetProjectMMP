using System.Collections;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    private float currentSpeed;
    private bool isStunned;

    private void Start() => currentSpeed = moveSpeed;

    private void Update()
    {
        if (!isStunned)
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    public void ApplySlow(float percent, float duration)
    {
        StartCoroutine(SlowRoutine(percent, duration));
    }

    private IEnumerator SlowRoutine(float percent, float duration)
    {
        currentSpeed = moveSpeed * (1f - percent);
        yield return new WaitForSeconds(duration);
        currentSpeed = moveSpeed;
    }

    public void PushBack(float distance) => transform.position -= transform.forward * distance;
}