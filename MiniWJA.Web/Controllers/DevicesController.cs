
using System.Linq;
using System.Web.Mvc;
using MiniWJA.Data;

namespace MiniWJA.Web.Controllers
{
    [Authorize]
    public class DevicesController : Controller
    {
        public ActionResult Index(string search)
        {
            using (var db = new AppDbContext())
            {
                var q = db.Devices.AsQueryable();
                if (!string.IsNullOrWhiteSpace(search))
                    q = q.Where(d => d.Hostname.Contains(search) || d.IpAddress.Contains(search) || d.Model.Contains(search));
                var devices = q.OrderBy(d => d.Hostname).ToList();
                return View(devices);
            }
        }

        public ActionResult Details(int id)
        {
            using (var db = new AppDbContext())
            {
                var device = db.Devices.FirstOrDefault(d => d.Id == id);
                if (device == null) return HttpNotFound();
                return View(device);
            }
        }
    }
}
