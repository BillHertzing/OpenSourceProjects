using System;

namespace ATAP.WebGet
{
    public interface IResponse<out T>
    {
        T Value { get; }
    }
}
