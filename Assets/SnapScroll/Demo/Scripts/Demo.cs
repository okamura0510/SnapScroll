using UnityEngine;
using UnityEngine.UI;

namespace SnapScroll
{
    public class Demo : MonoBehaviour
    {
        [SerializeField] SnapScrollView scrollView;
        [SerializeField] Image[] indicators;

        void Start()
        {
            Application.targetFrameRate = 60;
            
            scrollView.OnPageChanged += OnIndicatorUpdate;
            scrollView.RefreshPage();
        }
        
        void OnIndicatorUpdate()
        {
            for(var i = 0; i < indicators.Length; i++)
            {
                var a = (i == scrollView.Page) ? 1 : 0.5f;
                indicators[i].color = new Color(1, 1, 1, a);
            }
        }
        
        public void OnAppClick(GameObject app)
        {
            var url = "";
            switch(app.name)
            {
                case "Charikita":    url = "https://itunes.apple.com/jp/app/charide-laita.kamera/id566313348?mt=8";   break;
                case "M4u":          url = "https://assetstore.unity.com/packages/tools/gui/mvvm-4-ugui-44793";       break;
                case "SocialWorker": url = "https://github.com/okamura0510/SocialWorker";                             break;
                case "LJR":          url = "https://assetstore.unity.com/packages/tools/network/litjson-ruler-48492"; break;
                case "TempuraHP":    url = "http://okamura0510.jp/";                                                  break;
                case "TempuraBlog":  url = "https://okamura0510.hatenablog.jp/";                                      break;
            }
            Application.OpenURL(url);
        }
    }
}