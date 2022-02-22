using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioTiposCuentas
    {
        Task Crear(TipoCuenta tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioId);
        Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);
    }

    public class RepositorioTiposCuentas : IRepositorioTiposCuentas
    {
        public readonly string connectionSring;
        public RepositorioTiposCuentas(IConfiguration configuration) 
        {
            connectionSring = configuration.GetConnectionString("DefaultConnection");

        }

        public async Task Crear(TipoCuenta tipoCuenta) 
        {
            using var connection = new SqlConnection(connectionSring);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO TiposCuentas (Nombre,UsuarioId, Orden) Values(@Nombre, @UsuarioId, 0);
                            SELECT SCOPE_IDENTITY();", tipoCuenta);
            tipoCuenta.Id = id;
        }

        public async Task<bool> Existe(string nombre, int usuarioId)
        {
            using var connection = new SqlConnection(connectionSring);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM TiposCuentas WHERE Nombre = @Nombre AND UsuarioId = @UsuarioId",
                new { nombre, usuarioId });

            return existe == 1;
        }

        public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId) 
        {
            using var connection = new SqlConnection(connectionSring);
            return await connection.QueryAsync<TipoCuenta>(@"SELECT Nombre,UsuarioId,Orden FROM TiposCuentas WHERE UsuarioId = @UsuarioId", new { usuarioId});

        }
    }
}
