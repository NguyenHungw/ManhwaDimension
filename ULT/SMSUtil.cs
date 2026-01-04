using Newtonsoft.Json.Linq;
using RestSharp;

namespace ManhwaDimension.Util
{
    //public class SMSUtil
    //{
    //    public static ManhwaDimensionResponse SendSMS(string PhoneNumber, string SMSContent)
    //    {
    //        return STSendSMS(PhoneNumber, SMSContent);
    //    }

    //    public static ManhwaDimensionResponse SendSMSThreading(string PhoneNumber, string SMSContent)
    //    {
    //        return STSendSMSThreading(PhoneNumber, SMSContent);
    //    }

    //    public static ManhwaDimensionResponse STSendSMS(string PhoneNumber, string SMSContent)
    //    {
    //        var novaticResponse = ManhwaDimensionResponse.BAD_REQUEST();

    //        //Validate phone number 
    //        PhoneNumber = StandardizePhoneNumber(PhoneNumber);

    //        var client = new RestClient(SystemConstant.SMS_SOUTH_TELECOM_URL);
    //        client.Timeout = -1;
    //        var request = new RestRequest(RestSharp.Method.POST);
    //        request.AddHeader("Authorization", "Basic " + SystemConstant.SMS_SOUTH_TELECOM_TOKEN);
    //        //request.AddHeader("Content-Type", "application/json");
    //        request.AddHeader("Accept", "application/json");
    //        var dataToSend = "{\"from\":\"" + SystemConstant.LV_SMS_BRAND_NAME + "\",\"to\":\"" + PhoneNumber + "\",\"text\":\"" + SMSContent + "\",\"unicode\":\"1\"}";

    //        request.AddHeader("Content-Type", "text/plain");
    //        request.AddParameter("text/plain", dataToSend, ParameterType.RequestBody);

    //        IRestResponse response = client.Execute(request);
    //        //Console.WriteLine(response.Content);

    //        dynamic jsonResponse = JObject.Parse(response.Content);
    //        try
    //        {
    //            var responseStatusCode = jsonResponse["status"].ToString();
    //            if (responseStatusCode == "1")
    //            {
    //                novaticResponse = ManhwaDimensionResponse.SUCCESS(response.Content);
    //            }
    //            else
    //            {
    //                novaticResponse = ManhwaDimensionResponse.BAD_REQUEST(jsonResponse);
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            novaticResponse = ManhwaDimensionResponse.BAD_REQUEST(e);
    //            //throw;
    //        }
    //        return novaticResponse;
    //    }
    //    public static ManhwaDimensionResponse STSendSMSThreading(string PhoneNumber, string SMSContent)
    //    {
    //        var novaticResponse = ManhwaDimensionResponse.SUCCESS();
    //        new Thread(() =>
    //        {
    //            //Validate phone number 
    //            PhoneNumber = StandardizePhoneNumber(PhoneNumber);

    //            var client = new RestClient(SystemConstant.SMS_SOUTH_TELECOM_URL);
    //            client.Timeout = -1;
    //            var request = new RestRequest(RestSharp.Method.POST);
    //            request.AddHeader("Authorization", "Basic " + SystemConstant.SMS_SOUTH_TELECOM_TOKEN);
    //            //request.AddHeader("Content-Type", "application/json");
    //            request.AddHeader("Accept", "application/json");
    //            var dataToSend = "{\"from\":\"" + SystemConstant.LV_SMS_BRAND_NAME + "\",\"to\":\"" + PhoneNumber + "\",\"text\":\"" + SMSContent + "\",\"unicode\":\"1\"}";

    //            request.AddHeader("Content-Type", "text/plain");
    //            request.AddParameter("text/plain", dataToSend, ParameterType.RequestBody);

    //            IRestResponse response = client.Execute(request);
    //            //Console.WriteLine(response.Content);

    //            dynamic jsonResponse = JObject.Parse(response.Content);
    //            try
    //            {
    //                var responseStatusCode = jsonResponse["status"].ToString();
    //                if (responseStatusCode == "1")
    //                {
    //                    novaticResponse = ManhwaDimensionResponse.SUCCESS(response.Content);
    //                }
    //                else
    //                {
    //                    novaticResponse = ManhwaDimensionResponse.BAD_REQUEST(jsonResponse);
    //                }
    //            }
    //            catch (Exception e)
    //            {
    //                novaticResponse = ManhwaDimensionResponse.BAD_REQUEST(e);
    //                //throw;
    //            }
    //        }).Start();


    //        return novaticResponse;
    //    }


    //    public static string StandardizePhoneNumber(string PhoneNumber)
    //    {
    //        //Validate phone number 
    //        if (PhoneNumber.Substring(0, 1) == "0")
    //        {
    //            PhoneNumber = SystemConstant.SMS_VIETNAME_CODE + PhoneNumber.Substring(1, PhoneNumber.Length - 1);
    //        }
    //        return PhoneNumber;
    //    }


    //    //SMS Templates
    //    public static string SMS_CONTENT_REGISTER_ACCOUNT_OTP = AppSettingConfig.Instance.Get<string>("SMSBrandingConfig:RegisterAccountOTPTemplate");
    //}
}
