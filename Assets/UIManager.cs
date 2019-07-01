using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public RectTransform btnPrefab;
    private FeatureManager featureManager;
    private Text descriptionText;
    private List<Button> buttons;

    // Start is called before the first frame update
    void Start()
    {
        featureManager = FindObjectOfType<FeatureManager>();
        descriptionText = transform.Find("Navigation/Selected").GetComponent<Text>();
        transform.Find("Navigation/Previous").GetComponent<Button>().onClick.AddListener(() => featureManager.PrevChoice());
        transform.Find("Navigation/Next").GetComponent<Button>().onClick.AddListener(() => featureManager.NextChoice());
        InitializeFeatureButtons();
    }

    void InitializeFeatureButtons()
    {
        buttons = new List<Button>();

        float height = btnPrefab.rect.height;
        float width = btnPrefab.rect.width;

        for (int i = 0; featureManager.features.Count > i; i++)
        {
            RectTransform temp = Instantiate<RectTransform>(btnPrefab);
            temp.name = i.ToString();
            temp.SetParent(transform.Find("Features").GetComponent<RectTransform>());
            temp.localScale = new Vector3(1, 1, 1);
            temp.localPosition = new Vector3(0, 0, 0);
            temp.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, i > 3 ? 1 * width : 0 , width);
            temp.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, i > 3 ? (i-4) * height : i * height, height);

            Button b = temp.GetComponent<Button>();
            b.onClick.AddListener(() => featureManager.SetCurrent(int.Parse(temp.name)));
            buttons.Add(b);
        }
    }
    void UpdateFeatureButtons()
    {
        for(int i = 0; featureManager.features.Count > i; i++)
        {
            buttons[i].transform.Find("FeatureImg").GetComponent<Image>().sprite = featureManager.features[i].renderer.sprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFeatureButtons();
        EventSystem.current.SetSelectedGameObject(buttons[featureManager.currentFeature].gameObject);
        descriptionText.text = featureManager.features[featureManager.currentFeature].ID + " #" + (featureManager.features[featureManager.currentFeature].currentIndex + 1).ToString();
    }
}
