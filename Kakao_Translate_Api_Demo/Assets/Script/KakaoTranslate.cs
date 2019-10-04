using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class KakaoTranslate : MonoBehaviour
{
    public InputField inputBox;
    public Text transTx;

    public void _Translate()
    {
        //StartCoroutine(Translate((임의 변수) =>
        // {
        //     Debug.log(임의 변수);
        // }, "바꾸고 싶은 말", "현재 언어", "바뀔 언어",카카오 디벨로퍼 - Rest Api Id));

        StartCoroutine(Translate((result) =>
      {
          transTx.text = result;
      }, inputBox.text, "kr", "en", "Kakao Rest Key"));
    }

    IEnumerator Translate(UnityAction<string> result, string val, string Init_Lan, string Target_Lan, string RestAppId)
    {
        //데이터 전달
        var formData = new WWWForm();
        formData.AddField("query", val);
        formData.AddField("src_lang", Init_Lan);
        formData.AddField("target_lang", Target_Lan);

        var www = UnityWebRequest.Post("https://kapi.kakao.com/v1/translation/translate", formData);
        //헤더 설정
        www.SetRequestHeader("Authorization", "KakaoAK " + RestAppId);
        yield return www.SendWebRequest();

        //네트워크 에러가 났는지 체크
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //json파싱을 해제함
            var json = JObject.Parse(www.downloadHandler.text);
            var translate = (string)json["translated_text"][0][0];
            print(translate);

            //성공적으로 처리된 데이터 Return
            result(translate);
        }
    }
}