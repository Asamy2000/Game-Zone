using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameZone.Services
{
    public interface IDevicesServices
    {
        IEnumerable<SelectListItem> GetDevices();
    }
}
