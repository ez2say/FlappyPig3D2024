using UnityEngine;

public class SparkEmitter : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public ParticleSystem sparkParticleSystem;
    public float sparkInterval = 0.1f;
    private float nextSparkTime = 0f;

    void Update()
    {
        if (Time.time > nextSparkTime)
        {
            nextSparkTime = Time.time + sparkInterval;
            EmitSparks();
        }
    }

    void EmitSparks()
    {
        for (int i = 0; i < lineRenderer.positionCount - 1; i++)
        {
            Vector3 startPos = lineRenderer.GetPosition(i);
            Vector3 endPos = lineRenderer.GetPosition(i + 1);
            Vector3 sparkPos = Vector3.Lerp(startPos, endPos, Random.Range(0f, 1f));

            sparkParticleSystem.transform.position = sparkPos;
            sparkParticleSystem.Emit(1);
        }
    }
}