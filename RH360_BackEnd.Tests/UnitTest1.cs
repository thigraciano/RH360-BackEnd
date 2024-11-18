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
            // Inicializa o mock do repositório
            _usuarioRepositorioMock = new Mock<IUsuarioRepositorio>();
            _usuarioServico = new UsuarioServico(_usuarioRepositorioMock.Object); // Injeção de dependência
        }

        [Fact]
        public async Task ObterTodos_DeveRetornarUsuarios()
        {
            // Arrange: Cria uma lista de usuarios fictícios para ser retornada pelo mock
            var usuariosFicticios = new List<UsuarioModel>
            {
                new UsuarioModel { Id = 1, Nome = "Usuario 1", email = "usuario1@usuario.com", senha = "123456" },
                new UsuarioModel { Id = 2, Nome = "Usuario 2", email = "usuario1@usuario.com", senha = "123456" }
            };

            // Configura o mock para retornar os usuarios fictícios quando o método ObterTodos for chamado
            _usuarioRepositorioMock.Setup(r => r.ObterTodos()).ReturnsAsync(usuariosFicticios);

            // Act: Chama o método ObterTodos do UsuarioServico
            var resultado = await _usuarioServico.ObterTodos();

            // Assert: Verifica se o resultado é igual ao esperado
            resultado.Should().BeEquivalentTo(usuariosFicticios); // FluentAssertions para verificação
        }

        [Fact]
        public void CriarUsuario_DeveChamarRepositorio()
        {
            // Arrange
         var usuarioFicticio = new UsuarioModel { Nome = "Usuario Teste", email = "usuario1@usuario.com", senha = "123456"};

            // Act: Chama o método CriarUsuario do UsuarioServico
            _usuarioServico.CriarUsuario(usuarioFicticio);

            // Assert: Verifica se o método CriarUsuario do repositório foi chamado
            _usuarioRepositorioMock.Verify(r => r.CriarUsuario(usuarioFicticio), Times.Once);
        }

        [Fact]
        public void AtualizarProduto_DeveChamarRepositorio()
        {
            // Arrange
            var usuarioFicticio = new UsuarioModel { Nome = "Produto Teste", email = "usuario1@usuario.com", senha = "123456" };
            int produtoId = 1;

            // Act: Chama o método AtualizarUsuario do UsuarioServico
            _usuarioServico.AtualizarUsuario(produtoId, usuarioFicticio);

            // Assert: Verifica se o método AtualizarUsuario do repositório foi chamado com os parâmetros corretos
            _usuarioRepositorioMock.Verify(r => r.AtualizarUsuario(produtoId, usuarioFicticio), Times.Once);
        }

        [Fact]
        public void ExcluirProduto_DeveChamarRepositorio()
        {
            // Arrange
            int produtoId = 1;

            // Act: Chama o método ExcluirUsuario do UsuarioServico
            _usuarioServico.ExcluirUsuario(produtoId);

            // Assert: Verifica se o método ExcluirUsuario do repositório foi chamado
            _usuarioRepositorioMock.Verify(r => r.ExcluirUsuario(produtoId), Times.Once);
        }
    }
}
