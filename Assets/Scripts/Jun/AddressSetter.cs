using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using TMPro;


public class AddressSetter : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown provinceDropdown;
    [SerializeField]
    TMP_Dropdown cityDropdown;
    Collection<Address> address;
    public class Address
    {
        public string province { get; set; }
        public List<string> cities { get; set; }

    }
    static Collection<Address> LoadData(string data)
    {
        var retval = JsonConvert.DeserializeObject<Collection<Address>>(data);
        return retval;
    }
    private void Awake()
    {
        // JSON에서 주소지를 읽어온다.
        string s = File.ReadAllText(Application.dataPath + "/address.json");
        Debug.Log(s);
        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            Formatting = Newtonsoft.Json.Formatting.Indented
        };
        address = LoadData(s);
        //Debug.Log(address[0].province);

        // 읽어온 주소지를 드랍다운(Province, city)에 넣는다.
        // city의 경우 처음 보여지는 옵션인 서울만 넣어준다.
        foreach (Address item in address)
        {
            provinceDropdown.options.Add(new TMP_Dropdown.OptionData(item.province));
        }
        foreach (var city in address[0].cities)
        {
            cityDropdown.options.Add(new TMP_Dropdown.OptionData(city));
        }

        // Province 값이 바뀔때마다 City 값을 바꿀 수 있도록
        // 리스너를 추가한다.
        provinceDropdown.onValueChanged.AddListener(delegate
        {
            OnProvinceChanged();
        });
    }
    public void OnProvinceChanged()
    {
        cityDropdown.ClearOptions();
        foreach (var item in address)
        {
            //Debug.Log(string.Format("province : {0}, selected : {1}", item.province, change.options[change.value].text));
            if (item.province == provinceDropdown.options[provinceDropdown.value].text)
            {
                foreach (var city in item.cities)
                {
                    cityDropdown.options.Add(new TMP_Dropdown.OptionData(city));
                }
                cityDropdown.value = 0;
                break;
            }
        }
    }
}
