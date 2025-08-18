using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFX : MonoBehaviour
{
    private Character character;
    private SpriteRenderer sr;
    private Material defaultMaterial;
    [SerializeField] private float hitFxDuration;
    [SerializeField] private Material hitMaterial;


    private void Awake()
    {
        character = GetComponent<Character>();
        sr = GetComponentInChildren<SpriteRenderer>();
        defaultMaterial = sr.material;
    }
    private void Start()
    {
        character.stats.OnCharacterHit += PlayHitFx;
    }
    private void PlayHitFx()
    {
        StartCoroutine(CoHitFx(hitFxDuration));
    }    

    private IEnumerator CoHitFx(float fxDuration)
    {
        sr.material = hitMaterial;
        yield return new WaitForSeconds(fxDuration);
        sr.material = defaultMaterial;
    }    

}
