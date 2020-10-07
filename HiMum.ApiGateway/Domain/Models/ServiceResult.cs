using System.Collections.Generic;
using System.Linq;

namespace HiMum.ApiGateway.Domain.Models
{
    public class ServiceResult
    {
        public object Result { get; set; }

        public IList<ErrorDetail> Errors { get; set; }

        public bool IsSucces => !Errors?.Any() ?? true;
    }
}
