using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioTiposCuentas
    {
        void Crear(TipoCuenta tipoCuenta);
    }

    public class RepositorioTiposCuentas : IRepositorioTiposCuentas
    {
        public readonly string connectionSring;
        public RepositorioTiposCuentas(IConfiguration configuration) 
        {
            connectionSring = configuration.GetConnectionString("DefaultConnection");

        }

        public void Crear(TipoCuenta tipoCuenta) 
        {
            using var connection = new SqlConnection(connectionSring);
            var id = connection.QuerySingle<int>($@"INSERT INTO TiposCuentas (Nombre,UsuarioId, Orden) Values(@Nombre, @UsuarioId, 0);
                            SELECT SCOPE_IDENTITY();", tipoCuenta);
            tipoCuenta.Id = id;
        }
    }
}
