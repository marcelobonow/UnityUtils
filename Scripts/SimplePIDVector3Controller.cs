using UnityEngine;

public class SimplePIDVector3Controller
{
    public float kp = 0.1f;
    public float kd = 0.002f;
    public float ki = 3f;

    public Vector3 maxIntegral;
    public Vector3 minIntegral;

    public Vector3 maxLimit;
    public Vector3 minLimit;

    public Vector3 previousValue;
    public Vector3 previousError;

    public Vector3 integratorResult;
    public Vector3 differentiatorResult;


    public SimplePIDVector3Controller(float kp, float kd, float ki, Vector3 previousError, Vector3 maxLimit, Vector3 minLimit, Vector3 maxIntegral, Vector3 minIntegral, Vector3 previousValue)
    {
        this.kp = kp;
        this.kd = kd;
        this.ki = ki;
        this.previousError = previousError;
        this.maxIntegral = maxIntegral;
        this.minIntegral = minIntegral;
        this.previousValue = previousValue;
        this.maxLimit = maxLimit;
        this.minLimit = minLimit;
    }

    public Vector3 UpdateController(Vector3 setPoint, Vector3 currentValue, float deltaTime)
    {
        var error = setPoint - currentValue;

        var proportionalOutput = kp * error;

        integratorResult = integratorResult + (0.5f * ki * deltaTime * (error + previousError));
        integratorResult = ExtensionMethods.Clamp(integratorResult, minIntegral, maxIntegral);

        differentiatorResult = -kd * (currentValue - previousValue) / deltaTime;

        previousError = error;
        previousValue = currentValue;


        var pidResult = proportionalOutput + integratorResult + differentiatorResult;
        //Debug.Log($"P: {proportionalOutput}, I:{integratorResult}, D:{differentiatorResult}");
        return ExtensionMethods.Clamp(pidResult, minLimit, maxLimit);
    }
}
