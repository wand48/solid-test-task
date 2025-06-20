using Solid.TestTask.Models;

namespace Solid.TestTask.Features.Crb
{
    public interface ICbrService
    {
        public ValCurs GetQuotations(DateOnly date);
    }
}
