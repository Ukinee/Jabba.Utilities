using System.Diagnostics.CodeAnalysis;

namespace Utilities.Exceptions
{
    public static class NotImplemented
    {
        [DoesNotReturn]
        public static void ByDesign() => throw new NotImplementedException("By design");
    }
}
