using System;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Models
{
    public class Contract : IIdentifiable
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime StartContract { get; set; }
        public DateTime EndContract { get; set; }

        public virtual User User { get; set; }
        public virtual Plan Plan { get; set; }


    }
}