using UnityEngine;
using System.Collections;

public class FishCatchable : MonoBehaviour
{
    public FishType fishType;
    public int scoreValue;

    float catchChance;

    void Start()
    {
        switch (fishType)
        {
            case FishType.Fish:
                catchChance = 0.99f;
                scoreValue = 1;
                break;

            case FishType.SevenColorFish:
                catchChance = 0.75f;
                scoreValue = 3;
                break;

            case FishType.Crab:
                catchChance = 0.60f;
                scoreValue = 10;
                break;

            case FishType.Octopus:
                catchChance = 0.50f;
                scoreValue = 20;
                break;

            case FishType.SpecialFish:
                catchChance = 0.30f;
                scoreValue = 40;
                break;

            case FishType.Shark:
                catchChance = 0.06f;
                scoreValue = 70;
                break;

            case FishType.Orca:
                catchChance = 0.005f;
                scoreValue = 100;
                break;
        }
    }

    public void TryCatch()
    {
        float rand = Random.value;

        if (rand <= catchChance)
        {
            ScoreManager.Instance.AddScore(scoreValue);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        Vector3 startPos = transform.position;
        float duration = 0.15f;
        float strength = 0.12f;

        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            float xOffset = Random.Range(-strength, strength);
            transform.position = startPos + new Vector3(xOffset, 0, 0);
            yield return null;
        }

        transform.position = startPos;
    }
}