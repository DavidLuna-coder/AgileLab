using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using TFG.Application.Services.Experiences.Commands;

namespace TFG.Api.Controllers
{
    [Route("api/snapshots")]
    [ApiController]
    public class SnapshotsController : ControllerBase
    {
        private readonly ProjectSnapshotService _snapshotService;

        public SnapshotsController(ProjectSnapshotService snapshotService)
        {
            _snapshotService = snapshotService;
        }

        // Endpoint temporal para ejecutar el registro de snapshots manualmente
        [HttpPost("run")]
        [AllowAnonymous] // Puedes ajustar el rol según tu sistema
        public async Task<IActionResult> RunSnapshots(CancellationToken cancellationToken)
        {
            try
            {
                await _snapshotService.RegisterSnapshotsAsync(cancellationToken);
                return Ok(new { message = "Snapshots registrados correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
