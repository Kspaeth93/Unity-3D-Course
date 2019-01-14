using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] private float period = 5f;
    [SerializeField] private Vector3 movementVector = new Vector3(-7.25f, 27.25f, 5.25f);

    [Range(0,1)] private float movementFactor; // 0 for not moved, 1 for fully moved
    private Vector3 initVector;
    private const float TAU = Mathf.PI * 2;

    private void Start()
    {
        initVector = transform.position;
    }

    private void Update()
    {
        if (period <= Mathf.Epsilon) { return; }

        float cycles = Time.time / period;
        float rawSinWave = Mathf.Sin(cycles * TAU);

        movementFactor = rawSinWave / 2f + 0.5f;
        transform.position = initVector + (movementVector * movementFactor);
    }
}
