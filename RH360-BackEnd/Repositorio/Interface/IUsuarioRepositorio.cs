using RH360_BackEnd.Model;

namespace RH360_BackEnd.Repositorio.Interface
{
    public interface IUsuarioRepositorio
    {
        Task<IEnumerable<UsuarioModel>> ObterTodos();
        Task<int> CriarUsuario(UsuarioModel usuario);
        Task AtualizarUsuario(int id, UsuarioModel usuario);
        Task ExcluirUsuario(int id);
    }
}
