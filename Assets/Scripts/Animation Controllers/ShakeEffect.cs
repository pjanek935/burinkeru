using UnityEngine;

public class ShakeEffect : MonoBehaviour
{
    [SerializeField] float frequency = 1f;
    [SerializeField] float pow = 2;
    [SerializeField] float d = 2;
    [SerializeField] float maxRange = 100f;

    public static ShakeEffect Instance
    {
        get { return instance; }
    }

    static ShakeEffect instance;
    float randomSeed = 0f;
    float magnitude = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Nie moze byc dwoch instancji klasy ShakeEffect na scenie!");
        }
        else
        {
            instance = this;
        }

        randomSeed = Random.Range(0f, 1f);
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    private void Update()
    {
        if (magnitude > 0)
        {
            transform.localPosition = new Vector3(
                Mathf.PerlinNoise (randomSeed, Time.time * frequency) * 2 - 1,
                Mathf.PerlinNoise(randomSeed + 1, Time.time * frequency) * 2 - 1,
                Mathf.PerlinNoise(randomSeed + 2, Time.time * frequency) * 2 - 1
                ) * Mathf.Pow (magnitude, pow);

            magnitude -= Time.deltaTime * d;
        }
    }

    public void Shake (float magnitude)
    {
        this.magnitude += magnitude;
        this.magnitude = Mathf.Clamp01(this.magnitude);
    }

    public void ShakeIfNotShaking(float magnitude)
    {
        if (this.magnitude < float.Epsilon)
        {
            this.magnitude += magnitude;
            this.magnitude = Mathf.Clamp01(this.magnitude);
        }
    }

    public void ShakeAndClampToGivenValue(float magnitude)
    {
        this.magnitude += magnitude;
        this.magnitude = Mathf.Clamp(this.magnitude, 0, magnitude);
    }

    public void Shake (Vector3 origin)
    {
        float d = Vector3.Distance(origin, transform.position);
        float magnitude = Mathf.Pow (Mathf.InverseLerp(0, maxRange, d), 2);
        Shake(magnitude);
    }
}
