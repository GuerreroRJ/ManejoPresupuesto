using Dapper;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        private readonly IServiciosUsuarios serviciosUsuarios;

        public TiposCuentasController(IRepositorioTiposCuentas  repositorioTiposCuentas, IServiciosUsuarios serviciosUsuarios) 
        {
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            this.serviciosUsuarios = serviciosUsuarios;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var tiposCuentas = await repositorioTiposCuentas.Obtener(usuarioId);
            return View(tiposCuentas);
        }

        public IActionResult Crear() {
            //var modelo = new TipoCuenta() { Nombre = "Rolando" };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta) {
            //validación si la vista es valida
            if (!ModelState.IsValid) {
                return View(tipoCuenta);
            }

            tipoCuenta.UsuarioId = serviciosUsuarios.ObtenerUsuarioId();

            var yaExisteTipoCuenta = await repositorioTiposCuentas.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);

            if (yaExisteTipoCuenta ) 
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre),$"El nombre {tipoCuenta.Nombre} ya existe");
                return View(tipoCuenta);
            }


            await repositorioTiposCuentas.Crear(tipoCuenta);

            return RedirectToAction("Index"); 
        }

        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoCuenta(String nombre)
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var yaExisteTipoCuenta = await repositorioTiposCuentas.Existe(nombre,usuarioId);

            if (yaExisteTipoCuenta) 
            {
                return Json($"El nombre {nombre} ya existe");
            }

            return Json(true);
        }
    }
}
