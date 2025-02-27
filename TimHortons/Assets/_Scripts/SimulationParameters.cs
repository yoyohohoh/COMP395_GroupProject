using UnityEngine;

[CreateAssetMenu(fileName = "SimulationParameters", menuName = "Scriptable Objects/SimulationParameters")]
public class SimulationParameters : ScriptableObject
{
    // General simulation settings
    public string SimulationName = "Tim Hortons";
    public float TimeScale = 10.0f; // 1 hour in simulation time = 10 seconds in real time
    public float dt = 0.02f;  // Unity¡¦s default physics update interval (FixedUpdate) is 0.02s
    public float StartTime = 0.0f;
    public float EndTime = 60.0f;  // 60 seconds = 6 hours (08:00 - 14:00)

    // M/M/1 Queue Parameters
    [Header("M/M/1 Queue Parameters")]
    public float lambda = 32f;  // Arrival rate
    public float mu = 34f;      // Service rate
    public float wt = 28f;      // Waiting inLine Time
}
