using Microsoft.AspNetCore.Mvc;

namespace ManhwaDimension.Models.Response
{
    public class ManhwaDimensionResponse : IActionResult
    {
        public static string STATUS_SUCCESS = "200";
        public string status { get; set; }
        public string message { get; set; }
        public string value { get; set; }
        public IList<Object> data { get; set; }
        public object resources { get; set; }
        public string title { get; set; }
        public object? errors { get; set; }

        public ManhwaDimensionResponse(string status, string message, IList<Object> data)
        {
            this.status = status;
            this.message = message;
            this.data = data;
        }
        public ManhwaDimensionResponse(string status, string message, object data)
        {
            this.status = status;
            this.message = message;
            this.resources = data;
        }
        public ManhwaDimensionResponse(string status, string message)
        {
            this.status = status;
            this.message = message;
        }

        public ManhwaDimensionResponse()
        {
        }

        public static ManhwaDimensionResponse Success<T>(T? data, string? message = "SUCCESS") where T : class
        {
            return new ManhwaDimensionResponse()
            {
                status = "200",
                message = message,
                resources = data
            };
        }

        public static ManhwaDimensionResponse Failed(string status, string message, object data)
        {
            return new ManhwaDimensionResponse(status, message, new List<object> { data });
        }
        public static ManhwaDimensionResponse Success(string status, string message, object data)
        {
            return new ManhwaDimensionResponse(status, message, new List<object> { data });
        }
        public static ManhwaDimensionResponse SUCCESS(IList<object> data)
        {
            return new ManhwaDimensionResponse("200", "SUCCESS", data);
        }
        public static ManhwaDimensionResponse ISEXISTUSERNAME()
        {
            return new ManhwaDimensionResponse("205", "IS_EXIST_USERNAME");
        }
        public static ManhwaDimensionResponse ISEXISTIDSTAFF()
        {
            return new ManhwaDimensionResponse("206", "IS_EXIST_IDSTAFF");
        }
        public static ManhwaDimensionResponse ISEXISTPHONENUMBER()
        {
            return new ManhwaDimensionResponse("207", "IS_EXIST_PHONE_NUMBER");
        }
        public static ManhwaDimensionResponse SUCCESS(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new ManhwaDimensionResponse("200", "SUCCESS", returnData);
        }
        public static ManhwaDimensionResponse BAD_REQUEST(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new ManhwaDimensionResponse("400", "BAD_REQUEST", returnData);
        }

        public static ManhwaDimensionResponse BAD_REQUEST()
        {
            List<Object> returnData = new List<Object>();
            var obj = new { Code = 400, Message = "BAD_REQUEST" };
            returnData.Add(obj);
            return new ManhwaDimensionResponse("400", "BAD_REQUEST", returnData);
        }
        public static ManhwaDimensionResponse FAIL_BOOKING_TIME()
        {
            return new ManhwaDimensionResponse("207", "SUCCESS");
        }
        public static ManhwaDimensionResponse SUCCESS()
        {
            return new ManhwaDimensionResponse("200", "SUCCESS");
        }
        //trả về SUCCESSNOTBIDDING trong hoàn tiền đặt cọc
        //public static ManhwaDimensionResponse SUCCESSNOTBIDDING(Object data)
        //{
        //    List<Object> returnData = new List<Object>();
        //    returnData.Add(data);
        //    return new ManhwaDimensionResponse("205", "SUCCESSNOTBIDDING", returnData);
        //}
        public static ManhwaDimensionResponse SUCCESSNOTBIDDING(IList<Object> data)
        {
            return new ManhwaDimensionResponse("205", "SUCCESSNOTBIDDING", data);
        }
        //trả về SUCCESSHAVEBIDDING trong hoàn tiền đặt cọc
        //public static ManhwaDimensionResponse SUCCESSHAVEBIDDING(Object data)
        //{
        //    List<Object> returnData = new List<Object>();
        //    returnData.Add(data);
        //    return new ManhwaDimensionResponse("206", "SUCCESSHAVEBIDDING", returnData);
        //}
        public static ManhwaDimensionResponse SUCCESSHAVEBIDDING(IList<Object> data)
        {
            return new ManhwaDimensionResponse("206", "SUCCESSHAVEBIDDING", data);
        }

        public static ManhwaDimensionResponse CREATED(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new ManhwaDimensionResponse("201", "CREATED", returnData);
        }
        public static ManhwaDimensionResponse CREATED(List<Object> data)
        {
            List<Object> returnData = new List<Object>();
            returnData = data;
            return new ManhwaDimensionResponse("201", "CREATED", returnData);
        }
        public static ManhwaDimensionResponse Faild()
        {
            return new ManhwaDimensionResponse("099", "FAILD");
        }
        public static ManhwaDimensionResponse UNAUTHORIZED()
        {
            return new ManhwaDimensionResponse("401", "UNAUTHORIZED");
        }
        public static ManhwaDimensionResponse BiddingFaildEnded(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new ManhwaDimensionResponse("096", "BIDDINGFAILD", returnData);
        }

        public static ManhwaDimensionResponse VerifyCodeExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new ManhwaDimensionResponse("201", "Mã code không đúng", data);
        }

        public static ManhwaDimensionResponse EmailExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new ManhwaDimensionResponse("202", "EMAILEXIST", data);
        }
        public static ManhwaDimensionResponse EmailNotValid(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new ManhwaDimensionResponse("204", "EMAILNOTVALID");
        }
        public static ManhwaDimensionResponse UsernameExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new ManhwaDimensionResponse("203", "USENAMEEXIST");
        }
        public static ManhwaDimensionResponse IdCardNumberExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new ManhwaDimensionResponse("205", "IDCARNUMBEREXIST");
        }
        public static ManhwaDimensionResponse BiddingRequestExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new ManhwaDimensionResponse("203", "BIDDINGREQUESTEXIST", returnData);
        }
        public static ManhwaDimensionResponse PhoneExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new ManhwaDimensionResponse("204", "PHONEEXIST");
        }
        public static ManhwaDimensionResponse CompanyIdExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new ManhwaDimensionResponse("206", "COMPANYIDEXIST");
        }
        public static ManhwaDimensionResponse ItemExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new ManhwaDimensionResponse("203", "ITEMEXIST", returnData);
        }

        public static ManhwaDimensionResponse Error(string status, string title, object? errors)
        {
            return new ManhwaDimensionResponse()
            {
                status = status,
                title = title,
                errors = errors
            };
        }

        public static ManhwaDimensionResponse NotFound(string title, object errors)
        {
            return Error("404", title, errors);
        }

        public static ManhwaDimensionResponse NotFoundBiddingMax()
        {
            return new ManhwaDimensionResponse("999", "FAILD");
        }
        public static ManhwaDimensionResponse PostNameExist()
        {
            return new ManhwaDimensionResponse("203", "POSTNAMEEXIST");
        }
        public static ManhwaDimensionResponse NotFoundBiddingSecond()
        {
            return new ManhwaDimensionResponse("998", "FAILD");
        }
        public static ManhwaDimensionResponse PasswordExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new ManhwaDimensionResponse("202", "PASSWORDEXIST", returnData);
        }
        public static ManhwaDimensionResponse PasswordIsNotFormat(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new ManhwaDimensionResponse("205", "PASSWORDISNOTINCORRECTFORMAT", returnData);
        }

        public static ManhwaDimensionResponse OTP_REQUIRED(IList<Object> data)
        {
            return new ManhwaDimensionResponse("200", "OTP_REQUIRED", data);
        }
        public static ManhwaDimensionResponse OTP_OVER_LIMIT(IList<Object> data)
        {
            return new ManhwaDimensionResponse("099", "OTP_OVER_LIMIT", data);
        }
        public static ManhwaDimensionResponse OTP_INVALID_DATA(IList<Object> data)
        {
            return new ManhwaDimensionResponse("098", "INVALID_DATA", data);
        }
        public static ManhwaDimensionResponse OTP_EXIST()
        {
            return new ManhwaDimensionResponse("204", "OTP_EXIST");
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
