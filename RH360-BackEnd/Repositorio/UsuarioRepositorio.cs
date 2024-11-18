using Dapper;
using Npgsql;
using RH360_BackEnd.Model;
using RH360_BackEnd.Repositorio.Interface;
using Sata.Api.Estoque.Infraestrutura.Util;

namespace RH360_BackEnd.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly string _connectionString;

        public UsuarioRepositorio(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<UsuarioModel>> ObterTodos()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

 
                CriadorSQL objSql = new CriadorSQL("usuarios");

                objSql.AddSelectHeader(@"
                    SELECT id, 
                    nome, 
                    email,
                    senha
                    FROM usuarios 
                    WHERE 1=1");

                objSql.AddSelectFooter(" ORDER BY nome ");
                var sql = objSql.Select();

                return await connection.QueryAsync<UsuarioModel>(sql);
            }
        }


        public async Task<int> CriarUsuario(UsuarioModel usuario)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                CriadorSQL objSql = new CriadorSQL("usuarios");
                objSql.AddCampo(new Dictionary<string, object>
                {
                { "nome", usuario.Nome },
                { "email", usuario.email },
                { "senha", usuario.senha },
                });

                string sql = objSql.Insert();
                return await connection.ExecuteAsync(sql);

            }
        }

        public void AtualizarUsuario(int id, UsuarioModel usuario)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var query = "UPDATE usuarios SET nome = @nome, email = @email, senha = @senha WHERE id = @id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@nome", usuario.Nome);
                    command.Parameters.AddWithValue("@email", usuario.email);
                    command.Parameters.AddWithValue("@senha", usuario.senha);
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task ExcluirUsuario(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                CriadorSQL objSql = new CriadorSQL("usuarios");

                objSql.AddWhere(new Dictionary<string, object>
                {
                { "id", id }

                });

                var sql = objSql.Delete();

                await connection.ExecuteAsync(sql);
            }
        }
    }
}
