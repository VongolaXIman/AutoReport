using Model;
using System;

namespace Service
{
    public class BaseService
    {
        protected ResponseModel Response = new ResponseModel
        {
            Status = "",
            Msg = "",
            Success = false,
            Data = Array.Empty<object>()
        };

        public ResponseModel Response_200 = new ResponseModel
        {
            Status = "200",
            Msg = "執行成功",
            Success = true,
            Data = Array.Empty<object>()
        };

        public ResponseModel Response_401 = new ResponseModel
        {
            Status = "401",
            Msg = "執行錯誤",
            Success = false,
            Data = Array.Empty<object>()
        };

        public ResponseModel Response_500 = new ResponseModel
        {
            Status = "500",
            Msg = "API無回應、伺服器無回應，不回傳任何Content",
            Success = false,
            Data = Array.Empty<object>()
        };

        public ResponseModel Response_601 = new ResponseModel
        {
            Status = "601",
            Msg = "API傳入格式錯誤",
            Success = false,
            Data = Array.Empty<object>()
        };

        public ResponseModel Response_605 = new ResponseModel
        {
            Status = "605",
            Msg = "執行失敗",
            Success = false,
            Data = Array.Empty<object>()
        };

        public ResponseModel Response_607 = new ResponseModel
        {
            Status = "607",
            Msg = "檔案新增/取得失敗 (外部API串接)",
            Success = false,
            Data = Array.Empty<object>()
        };

        public ResponseModel Response_609 = new ResponseModel
        {
            Status = "609",
            Msg = "資料已存在，無法重複寫入。",
            Success = false,
            Data = Array.Empty<object>()
        };

        public ResponseModel Response_613 = new ResponseModel
        {
            Status = "613",
            Msg = "執行失敗",
            Success = false,
            Data = Array.Empty<object>()
        };

        public ResponseModel Response_701 = new ResponseModel
        {
            Status = "701",
            Msg = "帳號重複",
            Success = false,
            Data = Array.Empty<object>()
        };

        public ResponseModel Response_704 = new ResponseModel
        {
            Status = "704",
            Msg = "執行錯誤",
            Success = false,
            Data = Array.Empty<object>()
        };

        public ResponseModel Response_705 = new ResponseModel
        {
            Status = "705",
            Msg = "查無此來源",
            Success = false,
            Data = Array.Empty<object>()
        };

        public ResponseModel Response_706 = new ResponseModel
        {
            Status = "706",
            Msg = "資料正確性有誤，請重試",
            Success = false,
            Data = Array.Empty<object>()
        };

        public ResponseModel Response_711 = new ResponseModel
        {
            Status = "711",
            Msg = "該帳號尚未完成驗證",
            Success = false,
            Data = Array.Empty<object>()
        };

        public ResponseModel Response_712 = new ResponseModel
        {
            Status = "712",
            Msg = "重設密碼",
            Success = false,
            Data = Array.Empty<object>()
        };

        public ResponseModel Response_713 = new ResponseModel
        {
            Status = "713",
            Msg = "帳號鎖定",
            Success = false,
            Data = Array.Empty<object>()
        };

        public ResponseModel Response_703 = new ResponseModel
        {
            Status = "703",
            Msg = "寫入資料邏輯錯誤",
            Success = false,
            Data = Array.Empty<object>()
        };


        public static string RequestToString<T>(T request) where T : class
        {
            return request == null ? "" :
                           System.Text.Json.JsonSerializer.Serialize(value: request,
                           options: new System.Text.Json.JsonSerializerOptions
                           {
                               Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                           });
        }

        public static string RequestToStringIgnoreNullValues<T>(T request) where T : class
        {
            return request == null ? "" :
                           System.Text.Json.JsonSerializer.Serialize(value: request,
                           options: new System.Text.Json.JsonSerializerOptions
                           {
                               Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                               IgnoreNullValues = true
                           });
        }
    }
}
