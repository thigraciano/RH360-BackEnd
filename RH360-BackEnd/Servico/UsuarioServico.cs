using Microsoft.AspNetCore.Connections;
using RH360_BackEnd.Model;
using RH360_BackEnd.Repositorio.Interface;
using RH360_BackEnd.Servico.Interface;

namespace RH360_BackEnd.Servico
{
    public class UsuarioServico : IUsuarioServico
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioServico(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public Task<IEnumerable<UsuarioModel>> ObterTodos()
        {
            return _usuarioRepositorio.ObterTodos();
        }

        public void CriarUsuario(UsuarioModel usuario)
        {
            _usuarioRepositorio.CriarUsuario(usuario);
        }

        public void AtualizarUsuario(int id, UsuarioModel usuario)
        {
            _usuarioRepositorio.AtualizarUsuario(id, usuario);
        }

        public void ExcluirUsuario(int id)
        {
            _usuarioRepositorio.ExcluirUsuario(id);
        }
    }
}
