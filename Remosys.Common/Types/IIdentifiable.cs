using System;

namespace Remosys.Common.Types
{
    public interface IIdentifiable
    {
        Guid Id { get; }
    }
}