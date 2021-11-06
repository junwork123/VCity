using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DateSetter : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown dropYear;
    [SerializeField]
    TMP_Dropdown dropMonth;
    [SerializeField]
    TMP_Dropdown dropDay;
    // Start is called before the first frame update
    public int backdays = 0;
    void Awake()
    {
        //year
        dropYear.options = new List<TMP_Dropdown.OptionData>();
        for (int i = DateTime.Now.Year; i >= 1980; i--)
        {
            dropYear.options.Add(new TMP_Dropdown.OptionData(i.ToString()));
        }
        //month
        dropMonth.options = new List<TMP_Dropdown.OptionData>();
        for (int i = 1; i <= 12; i++)
        {
            dropMonth.options.Add(new TMP_Dropdown.OptionData(i.ToString()));
        }//day
        dropDay.options = new List<TMP_Dropdown.OptionData>();
        for (int i = 1; i <= 31; i++)
        {
            dropDay.options.Add(new TMP_Dropdown.OptionData(i.ToString()));
        }
        SetDateFromNow(backdays);
        dropMonth.onValueChanged.AddListener(delegate
        {
            OnMonthChanged();
        });
    }
    public void SetDateFromNow(int days)
    {
        DateTime date = DateTime.Now.AddDays(-days);
        for (int i = 0; i < dropYear.options.Count; i++)
        {

            if (dropYear.options[i].text == date.Year.ToString())
            {
                dropYear.value = i;
                break;
            }
        }
        for (int i = 0; i < dropMonth.options.Count; i++)
        {
            if (dropMonth.options[i].text == date.Month.ToString())
            {
                dropMonth.value = i;
                break;
            }
        }
        for (int i = 0; i < dropDay.options.Count; i++)
        {
            if (dropDay.options[i].text == date.Day.ToString())
            {
                dropDay.value = i;
                break;
            }
        }
    }
    public void OnMonthChanged()
    {
        int year = int.Parse(dropYear.value.ToString());
        int month = int.Parse(dropMonth.value.ToString());
        int END_OF_DAY = DateTime.DaysInMonth(year, month);
        dropDay.ClearOptions();
        for (int i = 1; i <= END_OF_DAY; i++)
        {
            dropDay.options.Add(new TMP_Dropdown.OptionData(i.ToString()));
        }

    }

}
