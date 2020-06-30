namespace System
{
    public sealed class ConsoleFormatInfo : IFormatProvider
    {
        private static ConsoleFormatInfo s_ansiSupportedInfo;
        private static ConsoleFormatInfo s_ansiNotSupportedInfo;
        private bool _isReadOnly;
        private bool _supportsAnsiCodes;

        private ConsoleFormatInfo()
        {
        }

        public static ConsoleFormatInfo CurrentInfo
        {
            get
            {
                // TODO: read this from the Console
                return AnsiSupportedInfo;
            }
        }

        public static ConsoleFormatInfo AnsiSupportedInfo => s_ansiSupportedInfo ??=
            new ConsoleFormatInfo { _isReadOnly = true, _supportsAnsiCodes = true };

        public static ConsoleFormatInfo AnsiNotSupportedInfo => s_ansiNotSupportedInfo ??=
            new ConsoleFormatInfo { _isReadOnly = true, _supportsAnsiCodes = false };

        public bool SupportsAnsiCodes
        {
            get => _supportsAnsiCodes;
            set
            {
                VerifyWritable();
                _supportsAnsiCodes = value;
            }
        }

        public static ConsoleFormatInfo GetInstance(IFormatProvider formatProvider)
        {
            return formatProvider == null ?
                CurrentInfo : // Fast path for a null provider
                GetProviderNonNull(formatProvider);

            static ConsoleFormatInfo GetProviderNonNull(IFormatProvider provider)
            {
                return
                    provider as ConsoleFormatInfo ?? // Fast path for an CFI
                    provider.GetFormat(typeof(ConsoleFormatInfo)) as ConsoleFormatInfo ??
                    CurrentInfo;
            }
        }

        public object GetFormat(Type formatType) =>
            formatType == typeof(ConsoleFormatInfo) ? this : null;

        private void VerifyWritable()
        {
            if (_isReadOnly)
            {
                throw new InvalidOperationException("ReadOnly");
            }
        }
    }
}
