public interface IDamageable
{
    public float curHP { get; }
    public float maxHP { get; }

    public float DR { get; }

    public float shd { get; }

    public bool TakeDamage(float trDmg, float prgDmg, float ttlDmg, float fltDmg);
}
