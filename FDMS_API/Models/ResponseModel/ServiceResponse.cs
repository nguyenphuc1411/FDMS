﻿namespace FDMS_API.Models.ResponseModel
{
    public class ServiceResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public int StatusCode { get; set; }
    }
}
