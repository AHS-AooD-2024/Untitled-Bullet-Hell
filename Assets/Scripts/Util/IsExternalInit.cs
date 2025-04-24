#if !NET5_0_OR_GREATER
// Does nothing; allows using 'init' properties.
namespace System.Runtime.CompilerServices {
    internal static class IsExternalInit {}
}
#endif