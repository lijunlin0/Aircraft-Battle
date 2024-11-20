using UnityEngine;

public class ExplosiveEffect:MonoBehaviour
{
    private ParticleSystem mParticleSystem;
    public static ExplosiveEffect Create(Vector3 position)
    {
        GameObject gameObject=FightManager.GetCurrent().GetPoolManager().GetGameObject("Other/Explosion0"+RandomHelper.RandomInt(1,5));
        gameObject.transform.position=position;
        ExplosiveEffect explosiveEffect=gameObject.AddComponent<ExplosiveEffect>();
        explosiveEffect.Init();
        return explosiveEffect;
    }

    private void Init()
    {
        mParticleSystem=GetComponent<ParticleSystem>();
        mParticleSystem.Play();
    }
    public void Update()
    {
        if (!mParticleSystem.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}