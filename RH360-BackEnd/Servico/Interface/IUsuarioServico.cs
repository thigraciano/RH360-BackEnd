using RH360_BackEnd.Model;

namespace RH360_BackEnd.Servico.Interface
{
    public interface IUsuarioServico
    {
        Task<IEnumerable<UsuarioModel>> ObterTodos();
        void CriarUsuario(UsuarioModel usuario);
        void AtualizarUsuario(int id, UsuarioModel usuario);
        void ExcluirUsuario(int id);
    }
}
