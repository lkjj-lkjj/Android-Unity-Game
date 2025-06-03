using UnityEngine;
using UnityEngine.UI;

public class CarUI : MonoBehaviour
{
    public CarBehaviour Car;
    public Text HealthText;
    public Text ScoreText;
    public Text LevelText;

    void Update()
    {
        if (Car != null)
        {
            HealthText.text = "HP: " + Car.CurrentHealth + "/" + Car.MaxHealth;
            ScoreText.text = "Score: " + Car.Score;
            LevelText.text = "Level: " + Car.Level;
        }
    }
}