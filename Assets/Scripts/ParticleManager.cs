
using UnityEngine;
public enum Particle_Type {Type_Combination_Success,Type_Combination_Fail,Type_Sell_Unit,Type_Set_Unit,Type_Spawn_Monster }
public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [SerializeField] private ParticleSystem[] particles;
    public void Initialize()
    {
    }
    public void PlayClickParticle(Vector3 pos, Particle_Type type)
    {
        particles[(int)type].gameObject.transform.position = pos;
        particles[(int)type].Play();
    }
    public void ParticleStart(Particle_Type type)
    {
        PlayClickParticle(Camera.main.ScreenToWorldPoint(Input.mousePosition), type);
    }
}
