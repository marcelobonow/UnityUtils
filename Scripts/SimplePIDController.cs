using UnityEngine;

public class SimplePIDController
{
    public float kp = 0.1f;
    public float kd = 0.002f;
    public float ki = 3f;

    public float maxIntegral;
    public float minIntegral;

    public float maxLimit;
    public float minLimit;

    public float previousValue;
    public float previousError;

    public float integratorResult;
    public float differentiatorResult;


    public SimplePIDController(float kp, float kd, float ki, float previousError, float maxLimit, float minLimit, float maxIntegral, float minIntegral, float previousValue)
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

    public float UpdateController(float setPoint, float currentValue, float deltaTime)
    {
        var error = setPoint - currentValue;

        var proportionalOutput = kp * error;

        integratorResult = integratorResult + (0.5f * ki * deltaTime * (error + previousError));
        integratorResult = Mathf.Clamp(integratorResult, minIntegral, maxIntegral);

        differentiatorResult = -kd * (currentValue - previousValue) / deltaTime;

        previousError = error;
        previousValue = currentValue;


        var pidResult = proportionalOutput + integratorResult + differentiatorResult;
        var result = Mathf.Clamp(pidResult, minLimit, maxLimit);
        return result;
    }
}
