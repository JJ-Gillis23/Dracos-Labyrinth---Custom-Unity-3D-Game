using UnityEngine;
using UnityEngine.SceneManagement;

public class SpikeAnimator : MonoBehaviour
{
    private float riseHeight;
    private float riseSpeed;
    private float stayDuration;
    private float returnSpeed;

    private Vector3 startPos;
    private Vector3 topPos;
    private enum State { Rising, Staying, Returning }
    private State state = State.Rising;
    private float stayTimer;

    public void Init(float riseHeight, float riseSpeed, float stayDuration, float returnSpeed)
    {
        this.riseHeight = riseHeight;
        this.riseSpeed = riseSpeed;
        this.stayDuration = stayDuration;
        this.returnSpeed = returnSpeed;

        startPos = transform.position;
        topPos = startPos + Vector3.up * riseHeight;
    }

    void Update()
    {
        if(transform.position.y < startPos.y-2)
            Destroy(gameObject);
        switch (state)
        {
            case State.Rising:
                transform.position = Vector3.MoveTowards(transform.position, topPos, riseSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, topPos) < 0.01f)
                {
                    state = State.Staying;
                    stayTimer = stayDuration;
                }
                break;

            case State.Staying:
                stayTimer -= Time.deltaTime;
                if (stayTimer <= 0f)
                    state = State.Returning;
                break;

            case State.Returning:
                transform.position = Vector3.MoveTowards(transform.position, startPos, returnSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, startPos) < 0.01f)
                    Destroy(gameObject);
                break;
        }
    }

}