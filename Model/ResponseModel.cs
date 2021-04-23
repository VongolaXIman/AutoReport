namespace Model
{
    public class ResponseModel
    {
        /// <summary>
        /// 狀態碼
        /// </summary>        
        public string Status { get; set; }
        /// <summary>
        /// 狀態資訊
        /// </summary>       
        public string Msg { get; set; }
        /// <summary>
        /// 成功與否
        /// </summary>        
        public bool Success { get; set; }
        /// <summary>
        /// 回傳資料
        /// </summary>        
        public object Data { get; set; }
    }
}
