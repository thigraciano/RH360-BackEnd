using Microsoft.AspNetCore.Mvc;
using RH360_BackEnd.Model;
using RH360_BackEnd.Servico.Interface;

namespace RH360_BackEnd.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioServico _usuarioServico;

        public UsuariosController(IUsuarioServico usuariooServico)
        {
            _usuarioServico = usuariooServico;
        }

        [HttpGet]
        [Route("ObterUsuarios")]
        public async Task<ActionResult<IEnumerable<UsuarioModel>>> ObterUsuarios()
        {
            var usuarios = await _usuarioServico.ObterTodos();
            return Ok(usuarios);
        }


        [HttpPost]
        [Route("CriarUsuario")]
        public IActionResult CriarUsuario([FromBody] UsuarioModel usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retorna as mensagens de erro de validação
            }

            _usuarioServico.CriarUsuario(usuario);
            return Ok(new { mensagem = "Usuario criado com sucesso!" });
        }

        [HttpPut]
        [Route("AtualizarUsuario/{id}")]
        public IActionResult AtualizarUsuario(int id, [FromBody] UsuarioModel usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retorna as mensagens de erro de validação
            }
            _usuarioServico.AtualizarUsuario(id, usuario);
            return Ok(new { mensagem = "Usuario atualizado com sucesso!" });
        }

        [HttpDelete]
        [Route("ExcluirUsuario/{id}")]
        public IActionResult ExcluirUsuario(int id)
        {
            _usuarioServico.ExcluirUsuario(id);
            return Ok(new { mensagem = "Usuario excluído com sucesso!" });
        }
    }
}
