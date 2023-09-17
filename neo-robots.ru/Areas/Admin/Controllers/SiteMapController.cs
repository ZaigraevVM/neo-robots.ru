using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMI.MediatR;
using System.Data;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace SMI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class SiteMapController : Controller
    {
        private IMediator _mediator { get; set; }
        public SiteMapController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            await _mediator.Send(new SiteMapRequest());
            return View();
        }

        public async Task<IActionResult> Html()
        {
            await _mediator.Send(new AggregatorRequest());
            return View("Index");
        }
    }
}
