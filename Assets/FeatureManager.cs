using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FeatureManager : MonoBehaviour
{

    public List<Feature> features;
    public int currentFeature;


    void OnEnable()
    {
        LoadFeatures();
    }

    void OnDisable()
    {
        SaveFeatures();
    }

    void LoadFeatures()
    {
        features = new List<Feature>();
        features.Add(new Feature("Face", transform.Find("Face").GetComponent<SpriteRenderer>()));
        features.Add(new Feature("Hair", transform.Find("Face/Hair").GetComponent<SpriteRenderer>()));
        features.Add(new Feature("Eyes", transform.Find("Face/Eyes").GetComponent<SpriteRenderer>()));
        features.Add(new Feature("Mouth", transform.Find("Face/Mouth").GetComponent<SpriteRenderer>()));
        features.Add(new Feature("Tops", transform.Find("Top").GetComponent<SpriteRenderer>()));
        features.Add(new Feature("Bottoms", transform.Find("Bottom").GetComponent<SpriteRenderer>()));

        for(int i = 0; features.Count > i; i++)
        {
            string key = "FEATURE_" + i;
            if (!PlayerPrefs.HasKey(key))
                PlayerPrefs.SetInt(key, features[i].currentIndex);
            features[i].currentIndex = PlayerPrefs.GetInt(key);
            features[i].UpdateFeature();
        }
    }

    void SaveFeatures()
    {
        for(int i = 0; features.Count > i; i++)
        {
            string key = "FEATURE_" + i;
            PlayerPrefs.SetInt(key, features[i].currentIndex);
        }
        PlayerPrefs.Save();
    }

    public void SetCurrent(int index)
    {
        if (features == null)
            return;
        currentFeature = index;
    }

    public void NextChoice()
    {
        if (features == null)
            return;
        features[currentFeature].currentIndex++;
        features[currentFeature].UpdateFeature();
    }

    public void PrevChoice()
    {
        if (features == null)
            return;
        features[currentFeature].currentIndex--;
        features[currentFeature].UpdateFeature();
    }

}

[System.Serializable]

public class Feature
{
    public string ID;
    public int currentIndex;
    public Sprite[] choices;
    public SpriteRenderer renderer;

    public Feature(string id, SpriteRenderer rend)
    {
        ID = id;
        renderer = rend;
        UpdateFeature();
    }

    public void UpdateFeature()
    {
        choices = Resources.LoadAll<Sprite>("Textures/" + ID);
        if (choices == null || renderer == null)
            return;
        if (currentIndex < 0)
            currentIndex = choices.Length - 1;
        if (currentIndex >= choices.Length)
            currentIndex = 0;
        renderer.sprite = choices[currentIndex];
    }
}
