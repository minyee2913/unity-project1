using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonDestroyData : MonoBehaviour
{
    public Stage stage;

    private int money;
    private int soseoghoe;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.HasKey("money")) this.money = PlayerPrefs.GetInt("money");
        else
        {
            this.money = 1000;
            PlayerPrefs.SetInt("money", this.money);
        }
        if (PlayerPrefs.HasKey("soseoghoe")) this.soseoghoe = PlayerPrefs.GetInt("soseoghoe");
        else
        {
            this.soseoghoe = 120;
            PlayerPrefs.SetInt("soseoghoe", this.soseoghoe);
        }
    }

    public int GetMoney()
    {
        return this.money;
    }

    public void SetMoney(int money)
    {
        this.money = money;
        PlayerPrefs.SetInt("money", this.money);
    }

    public void AddMoney(int money)
    {
        this.money += money;
        PlayerPrefs.SetInt("money", this.money);
    }

    public int GetSoseog()
    {
        return this.soseoghoe;
    }

    public void SetSoseog(int Soseog)
    {
        this.soseoghoe = Soseog;
        PlayerPrefs.SetInt("soseoghoe", this.soseoghoe);
    }

    public void AddSoseog(int Soseog)
    {
        this.soseoghoe += Soseog;
        PlayerPrefs.SetInt("soseoghoe", this.soseoghoe);
    }
}
