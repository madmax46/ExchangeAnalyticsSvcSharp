using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAnalyticsService.Auth.Classes
{
    public class Response<T>
    {
        public int Id { get; set; }

        public bool Success { get; set; }

        public string Error { get; set; }

        public int ErrorCode { get; set; }

        public string Guid { get; set; }

        public T Value { get; set; }

        public Response()
        {
            this.Guid = ResponseGuid.ApplicationGuid;
        }

        public Response(T val)
        {
            this.Value = val;
            this.Success = true;
            this.Error = string.Empty;
            this.Guid = ResponseGuid.ApplicationGuid;
        }

        public Response(string error)
        {
            this.Error = error;
            this.Success = false;
            this.Value = default(T);
            this.Guid = ResponseGuid.ApplicationGuid;
        }

        public Response<T> Clone()
        {
            return (Response<T>)this.MemberwiseClone();
        }

        public Response<S> CloneBody<S>()
        {
            return new Response<S>()
            {
                Id = this.Id,
                Error = this.Error,
                ErrorCode = this.ErrorCode,
                Guid = this.Guid,
                Success = this.Success
            };
        }
    }

}
