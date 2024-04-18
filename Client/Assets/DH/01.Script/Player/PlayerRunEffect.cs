using PJH;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerRunEffect : MonoBehaviour
{
    [SerializeField] VisualEffect effect;
    private Player owner;

    private void Awake()
    {
        owner = GetComponent<Player>();
    }

    private void Update()
    {
        if (owner.IsSprinting)
        {
            if (!effect.isActiveAndEnabled)
                effect.Play();
        }
        else
        {
            effect.Stop();
        }
    }
}
