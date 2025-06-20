using System.Text;

namespace Solid.TestTask.Registration
{
    public static class ProviderRegistration
    {
        public static void Register()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }
}
