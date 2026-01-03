using Microsoft.AspNetCore.Mvc;

namespace ManhwaDimension.Models.Response
{
    public class GeneStoryResponse : IActionResult
    {
        public static string STATUS_SUCCESS = "200";
        public string status { get; set; }
        public string message { get; set; }
        public string value { get; set; }
        public IList<Object> data { get; set; }
        public object resources { get; set; }
        public string title { get; set; }
        public object? errors { get; set; }

        public GeneStoryResponse(string status, string message, IList<Object> data)
        {
            this.status = status;
            this.message = message;
            this.data = data;
        }
        public GeneStoryResponse(string status, string message, object data)
        {
            this.status = status;
            this.message = message;
            this.resources = data;
        }
        public GeneStoryResponse(string status, string message)
        {
            this.status = status;
            this.message = message;
        }

        public GeneStoryResponse()
        {
        }

        public static GeneStoryResponse Success<T>(T? data, string? message = "SUCCESS") where T : class
        {
            return new GeneStoryResponse()
            {
                status = "200",
                message = message,
                resources = data
            };
        }

        public static GeneStoryResponse Failed(string status, string message, object data)
        {
            return new GeneStoryResponse(status, message, new List<object> { data });
        }
        public static GeneStoryResponse Success(string status, string message, object data)
        {
            return new GeneStoryResponse(status, message, new List<object> { data });
        }
        public static GeneStoryResponse SUCCESS(IList<object> data)
        {
            return new GeneStoryResponse("200", "SUCCESS", data);
        }
        public static GeneStoryResponse ISEXISTUSERNAME()
        {
            return new GeneStoryResponse("205", "IS_EXIST_USERNAME");
        }
        public static GeneStoryResponse ISEXISTIDSTAFF()
        {
            return new GeneStoryResponse("206", "IS_EXIST_IDSTAFF");
        }
        public static GeneStoryResponse ISEXISTPHONENUMBER()
        {
            return new GeneStoryResponse("207", "IS_EXIST_PHONE_NUMBER");
        }
        public static GeneStoryResponse SUCCESS(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new GeneStoryResponse("200", "SUCCESS", returnData);
        }
        public static GeneStoryResponse BAD_REQUEST(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new GeneStoryResponse("400", "BAD_REQUEST", returnData);
        }

        public static GeneStoryResponse BAD_REQUEST()
        {
            List<Object> returnData = new List<Object>();
            var obj = new { Code = 400, Message = "BAD_REQUEST" };
            returnData.Add(obj);
            return new GeneStoryResponse("400", "BAD_REQUEST", returnData);
        }
        public static GeneStoryResponse FAIL_BOOKING_TIME()
        {
            return new GeneStoryResponse("207", "SUCCESS");
        }
        public static GeneStoryResponse SUCCESS()
        {
            return new GeneStoryResponse("200", "SUCCESS");
        }
        //trả về SUCCESSNOTBIDDING trong hoàn tiền đặt cọc
        //public static GeneStoryResponse SUCCESSNOTBIDDING(Object data)
        //{
        //    List<Object> returnData = new List<Object>();
        //    returnData.Add(data);
        //    return new GeneStoryResponse("205", "SUCCESSNOTBIDDING", returnData);
        //}
        public static GeneStoryResponse SUCCESSNOTBIDDING(IList<Object> data)
        {
            return new GeneStoryResponse("205", "SUCCESSNOTBIDDING", data);
        }
        //trả về SUCCESSHAVEBIDDING trong hoàn tiền đặt cọc
        //public static GeneStoryResponse SUCCESSHAVEBIDDING(Object data)
        //{
        //    List<Object> returnData = new List<Object>();
        //    returnData.Add(data);
        //    return new GeneStoryResponse("206", "SUCCESSHAVEBIDDING", returnData);
        //}
        public static GeneStoryResponse SUCCESSHAVEBIDDING(IList<Object> data)
        {
            return new GeneStoryResponse("206", "SUCCESSHAVEBIDDING", data);
        }

        public static GeneStoryResponse CREATED(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new GeneStoryResponse("201", "CREATED", returnData);
        }
        public static GeneStoryResponse CREATED(List<Object> data)
        {
            List<Object> returnData = new List<Object>();
            returnData = data;
            return new GeneStoryResponse("201", "CREATED", returnData);
        }
        public static GeneStoryResponse Faild()
        {
            return new GeneStoryResponse("099", "FAILD");
        }
        public static GeneStoryResponse UNAUTHORIZED()
        {
            return new GeneStoryResponse("401", "UNAUTHORIZED");
        }
        public static GeneStoryResponse BiddingFaildEnded(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new GeneStoryResponse("096", "BIDDINGFAILD", returnData);
        }

        public static GeneStoryResponse VerifyCodeExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new GeneStoryResponse("201", "Mã code không đúng", data);
        }

        public static GeneStoryResponse EmailExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new GeneStoryResponse("202", "EMAILEXIST", data);
        }
        public static GeneStoryResponse EmailNotValid(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new GeneStoryResponse("204", "EMAILNOTVALID");
        }
        public static GeneStoryResponse UsernameExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new GeneStoryResponse("203", "USENAMEEXIST");
        }
        public static GeneStoryResponse IdCardNumberExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new GeneStoryResponse("205", "IDCARNUMBEREXIST");
        }
        public static GeneStoryResponse BiddingRequestExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new GeneStoryResponse("203", "BIDDINGREQUESTEXIST", returnData);
        }
        public static GeneStoryResponse PhoneExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new GeneStoryResponse("204", "PHONEEXIST");
        }
        public static GeneStoryResponse CompanyIdExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new GeneStoryResponse("206", "COMPANYIDEXIST");
        }
        public static GeneStoryResponse ItemExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new GeneStoryResponse("203", "ITEMEXIST", returnData);
        }

        public static GeneStoryResponse Error(string status, string title, object? errors)
        {
            return new GeneStoryResponse()
            {
                status = status,
                title = title,
                errors = errors
            };
        }

        public static GeneStoryResponse NotFound(string title, object errors)
        {
            return Error("404", title, errors);
        }

        public static GeneStoryResponse NotFoundBiddingMax()
        {
            return new GeneStoryResponse("999", "FAILD");
        }
        public static GeneStoryResponse PostNameExist()
        {
            return new GeneStoryResponse("203", "POSTNAMEEXIST");
        }
        public static GeneStoryResponse NotFoundBiddingSecond()
        {
            return new GeneStoryResponse("998", "FAILD");
        }
        public static GeneStoryResponse PasswordExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new GeneStoryResponse("202", "PASSWORDEXIST", returnData);
        }
        public static GeneStoryResponse PasswordIsNotFormat(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new GeneStoryResponse("205", "PASSWORDISNOTINCORRECTFORMAT", returnData);
        }

        public static GeneStoryResponse OTP_REQUIRED(IList<Object> data)
        {
            return new GeneStoryResponse("200", "OTP_REQUIRED", data);
        }
        public static GeneStoryResponse OTP_OVER_LIMIT(IList<Object> data)
        {
            return new GeneStoryResponse("099", "OTP_OVER_LIMIT", data);
        }
        public static GeneStoryResponse OTP_INVALID_DATA(IList<Object> data)
        {
            return new GeneStoryResponse("098", "INVALID_DATA", data);
        }
        public static GeneStoryResponse OTP_EXIST()
        {
            return new GeneStoryResponse("204", "OTP_EXIST");
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
