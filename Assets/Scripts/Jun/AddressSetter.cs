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
        // string s = File.ReadAllText(Application.streamingAssetsPath + "/address.json");
        string s = ("[\n  {\n    \"province\": \"서울특별시\",\n    \"cities\": [\n      \"종로구\",\n      \"중구\",\n      \"용산구\",\n      \"성동구\",\n      \"광진구\",\n      \"동대문구\",\n      \"중랑구\",\n      \"성북구\",\n      \"강북구\",\n      \"도봉구\",\n      \"노원구\",\n      \"은평구\",\n      \"서대문구\",\n      \"마포구\",\n      \"양천구\",\n      \"강서구\",\n      \"구로구\",\n      \"금천구\",\n      \"영등포구\",\n      \"동작구\",\n      \"관악구\",\n      \"서초구\",\n      \"강남구\",\n      \"송파구\",\n      \"강동구\"\n    ]\n  },\n  {\n    \"province\": \"부산광역시\",\n    \"cities\": [\n      \"중구\",\n      \"서구\",\n      \"동구\",\n      \"영도구\",\n      \"부산진구\",\n      \"동래구\",\n      \"남구\",\n      \"북구\",\n      \"강서구\",\n      \"해운대구\",\n      \"사하구\",\n      \"금정구\",\n      \"연제구\",\n      \"수영구\",\n      \"사상구\",\n      \"기장군\"\n    ]\n  },\n  {\n    \"province\": \"인천광역시\",\n    \"cities\": [\n      \"중구\",\n      \"동구\",\n      \"남구\",\n      \"연수구\",\n      \"남동구\",\n      \"부평구\",\n      \"계양구\",\n      \"서구\",\n      \"강화군\",\n      \"옹진군\"\n    ]\n  },\n  {\n    \"province\": \"대구광역시\",\n    \"cities\": [\n      \"중구\",\n      \"동구\",\n      \"서구\",\n      \"남구\",\n      \"북구\",\n      \"수성구\",\n      \"달서구\",\n      \"달성군\"\n    ]\n  },\n  {\n    \"province\": \"광주광역시\",\n    \"cities\": [\"동구\", \"서구\", \"남구\", \"북구\", \"광산구\"]\n  },\n  {\n    \"province\": \"대전광역시\",\n    \"cities\": [\"동구\", \"중구\", \"서구\", \"유성구\", \"대덕구\"]\n  },\n  {\n    \"province\": \"울산광역시\",\n    \"cities\": [\"중구\", \"남구\", \"동구\", \"북구\", \"울주군\"]\n  },\n  { \"province\": \"세종특별자치시\", \"cities\": [] },\n  {\n    \"province\": \"경기도\",\n    \"cities\": [\n      \"가평군\",\n      \"고양시\",\n      \"과천시\",\n      \"광명시\",\n      \"광주시\",\n      \"구리시\",\n      \"군포시\",\n      \"김포시\",\n      \"남양주시\",\n      \"동두천시\",\n      \"부천시\",\n      \"성남시\",\n      \"수원시\",\n      \"시흥시\",\n      \"안산시\",\n      \"안성시\",\n      \"안양시\",\n      \"양주시\",\n      \"양평군\",\n      \"여주시\",\n      \"연천군\",\n      \"오산시\",\n      \"용인시\",\n      \"의왕시\",\n      \"의정부시\",\n      \"이천시\",\n      \"파주시\",\n      \"평택시\",\n      \"포천시\",\n      \"하남시\",\n      \"화성시\"\n    ]\n  },\n  {\n    \"province\": \"강원도\",\n    \"cities\": [\n      \"원주시\",\n      \"춘천시\",\n      \"강릉시\",\n      \"동해시\",\n      \"속초시\",\n      \"삼척시\",\n      \"홍천군\",\n      \"태백시\",\n      \"철원군\",\n      \"횡성군\",\n      \"평창군\",\n      \"영월군\",\n      \"정선군\",\n      \"인제군\",\n      \"고성군\",\n      \"양양군\",\n      \"화천군\",\n      \"양구군\"\n    ]\n  },\n  {\n    \"province\": \"충청북도\",\n    \"cities\": [\n      \"청주시\",\n      \"충주시\",\n      \"제천시\",\n      \"보은군\",\n      \"옥천군\",\n      \"영동군\",\n      \"증평군\",\n      \"진천군\",\n      \"괴산군\",\n      \"음성군\",\n      \"단양군\"\n    ]\n  },\n  {\n    \"province\": \"충청남도\",\n    \"cities\": [\n      \"천안시\",\n      \"공주시\",\n      \"보령시\",\n      \"아산시\",\n      \"서산시\",\n      \"논산시\",\n      \"계룡시\",\n      \"당진시\",\n      \"금산군\",\n      \"부여군\",\n      \"서천군\",\n      \"청양군\",\n      \"홍성군\",\n      \"예산군\",\n      \"태안군\"\n    ]\n  },\n  {\n    \"province\": \"경상북도\",\n    \"cities\": [\n      \"포항시\",\n      \"경주시\",\n      \"김천시\",\n      \"안동시\",\n      \"구미시\",\n      \"영주시\",\n      \"영천시\",\n      \"상주시\",\n      \"문경시\",\n      \"경산시\",\n      \"군위군\",\n      \"의성군\",\n      \"청송군\",\n      \"영양군\",\n      \"영덕군\",\n      \"청도군\",\n      \"고령군\",\n      \"성주군\",\n      \"칠곡군\",\n      \"예천군\",\n      \"봉화군\",\n      \"울진군\",\n      \"울릉군\"\n    ]\n  },\n  {\n    \"province\": \"경상남도\",\n    \"cities\": [\n      \"창원시\",\n      \"김해시\",\n      \"진주시\",\n      \"양산시\",\n      \"거제시\",\n      \"통영시\",\n      \"사천시\",\n      \"밀양시\",\n      \"함안군\",\n      \"거창군\",\n      \"창녕군\",\n      \"고성군\",\n      \"하동군\",\n      \"합천군\",\n      \"남해군\",\n      \"함양군\",\n      \"산청군\",\n      \"의령군\"\n    ]\n  },\n  {\n    \"province\": \"전라북도\",\n    \"cities\": [\n      \"전주시\",\n      \"익산시\",\n      \"군산시\",\n      \"정읍시\",\n      \"완주군\",\n      \"김제시\",\n      \"남원시\",\n      \"고창군\",\n      \"부안군\",\n      \"임실군\",\n      \"순창군\",\n      \"진안군\",\n      \"장수군\",\n      \"무주군\"\n    ]\n  },\n  {\n    \"province\": \"전라남도\",\n    \"cities\": [\n      \"여수시\",\n      \"순천시\",\n      \"목포시\",\n      \"광양시\",\n      \"나주시\",\n      \"무안군\",\n      \"해남군\",\n      \"고흥군\",\n      \"화순군\",\n      \"영암군\",\n      \"영광군\",\n      \"완도군\",\n      \"담양군\",\n      \"장성군\",\n      \"보성군\",\n      \"신안군\",\n      \"장흥군\",\n      \"강진군\",\n      \"함평군\",\n      \"진도군\",\n      \"곡성군\",\n      \"구례군\"\n    ]\n  },\n  { \"province\": \"제주특별자치시\", \"cities\": [\"제주시\", \"서귀포시\"] }\n]\n");
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
