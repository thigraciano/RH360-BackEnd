using Moq;
using Xunit;
using RH360_BackEnd.Servico;
using RH360_BackEnd.Model;
using RH360_BackEnd.Repositorio.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;

namespace RH360_BackEnd.Tests
{
    public class ProdutoServicoTests
    {
        private readonly Mock<IUsuarioRepositorio> _usuarioRepositorioMock;
        private readonly UsuarioServico _usuarioServico;

        public ProdutoServicoTests()
        {
            // Inicializa o mock do reposit�rio
            _usuarioRepositorioMock = new Mock<IUsuarioRepositorio>();
            _usuarioServico = new UsuarioServico(_usuarioRepositorioMock.Object); // Inje��o de depend�ncia
        }

        [Fact]
        public async Task ObterTodos_DeveRetornarUsuarios()
        {
            // Arrange: Cria uma lista de usuarios fict�cios para ser retornada pelo mock
            var usuariosFicticios = new List<UsuarioModel>
            {
                new UsuarioModel { Id = 1, Nome = "Usuario 1", email = "usuario1@usuario.com", senha = "123456" },
                new UsuarioModel { Id = 2, Nome = "Usuario 2", email = "usuario1@usuario.com", senha = "123456" }
            };

            // Configura o mock para retornar os usuarios fict�cios quando o m�todo ObterTodos for chamado
            _usuarioRepositorioMock.Setup(r => r.ObterTodos()).ReturnsAsync(usuariosFicticios);

            // Act: Chama o m�todo ObterTodos do UsuarioServico
            var resultado = await _usuarioServico.ObterTodos();

            // Assert: Verifica se o resultado � igual ao esperado
            resultado.Should().BeEquivalentTo(usuariosFicticios); // FluentAssertions para verifica��o
        }

        [Fact]
        public void CriarUsuario_DeveChamarRepositorio()
        {
            // Arrange
         var usuarioFicticio = new UsuarioModel { Nome = "Usuario Teste", email = "usuario1@usuario.com", senha = "123456"};

            // Act: Chama o m�todo CriarUsuario do UsuarioServico
            _usuarioServico.CriarUsuario(usuarioFicticio);

            // Assert: Verifica se o m�todo CriarUsuario do reposit�rio foi chamado
            _usuarioRepositorioMock.Verify(r => r.CriarUsuario(usuarioFicticio), Times.Once);
        }

        [Fact]
        public void AtualizarProduto_DeveChamarRepositorio()
        {
            // Arrange
            var usuarioFicticio = new UsuarioModel { Nome = "Produto Teste", email = "usuario1@usuario.com", senha = "123456" };
            int produtoId = 1;

            // Act: Chama o m�todo AtualizarUsuario do UsuarioServico
            _usuarioServico.AtualizarUsuario(produtoId, usuarioFicticio);

            // Assert: Verifica se o m�todo AtualizarUsuario do reposit�rio foi chamado com os par�metros corretos
            _usuarioRepositorioMock.Verify(r => r.AtualizarUsuario(produtoId, usuarioFicticio), Times.Once);
        }

        [Fact]
        public void ExcluirProduto_DeveChamarRepositorio()
        {
            // Arrange
            int produtoId = 1;

            // Act: Chama o m�todo ExcluirUsuario do UsuarioServico
            _usuarioServico.ExcluirUsuario(produtoId);

            // Assert: Verifica se o m�todo ExcluirUsuario do reposit�rio foi chamado
            _usuarioRepositorioMock.Verify(r => r.ExcluirUsuario(produtoId), Times.Once);
        }
    }
}
