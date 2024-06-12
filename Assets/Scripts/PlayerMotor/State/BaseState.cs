using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    protected PlayerMotor motor;

    private void Awake()
    {
        motor = GetComponent<PlayerMotor>();
    }
    public virtual void Construct() { }
    public virtual void Destruct() { }
    public virtual void Transition() { }

    public virtual Vector3 ProcessMotion()
    {
        Debug.Log("Process motion is not implemented in " + this.ToString());
        return Vector3.zero;
    }
}
