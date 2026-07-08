using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoostsUI : Singleton<BoostsUI>
{
    [SerializeField] private Button boost1Button;
    [SerializeField] private TextMeshProUGUI boost1Name;
    [SerializeField] private TextMeshProUGUI boost1Description;
    private Boost boost1;


    [SerializeField] private Button boost2Button;
    [SerializeField] private TextMeshProUGUI boost2Name;
    [SerializeField] private TextMeshProUGUI boost2Description;
    private Boost boost2;

    [SerializeField] private Button boost3Button;
    [SerializeField] private TextMeshProUGUI boost3Name;
    [SerializeField] private TextMeshProUGUI boost3Description;
    private Boost boost3;

    void Start()
    {
        boost1Button.onClick.AddListener(() => {
            boost1.Select();
            Close();
        });
        boost2Button.onClick.AddListener(() => {
            boost1.Select();
            Close();
        });
        boost3Button.onClick.AddListener(() => {
            boost1.Select();
            Close();
        });
    }

    public void SetBoosts(Boost[] boosts)
    {
        boost1 = boosts[0];
        boost1Name.text = boost1.Name;
        boost1Description.text = boost1.Description;

        boost2 = boosts[1];
        boost2Name.text = boost2.Name;
        boost2Description.text = boost2.Description;

        boost3 = boosts[2];
        boost3Name.text = boost3.Name;
        boost3Description.text = boost3.Description;
    }

    void Close()
    {
        UIManager.Instance.PreviousState();
    }
}
