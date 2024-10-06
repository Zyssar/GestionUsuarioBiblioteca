using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GestionUsuarioBiblioteca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get() =>
            await _userService.GetAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id) // Cambiado a int
        {
            var user = await _userService.GetByIdAsync(id); // Cambiado a int
            if (user == null) return NotFound();
            return user;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            // No se debe establecer Id en el cuerpo del usuario
            await _userService.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user); // Ya no es necesario convertir a string
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] User updatedUser) // Cambiado a int
        {
            var user = await _userService.GetByIdAsync(id); // Cambiado a int
            if (user == null) return NotFound();

            updatedUser.Id = user.Id; // Asegurarse de mantener el mismo Id
            await _userService.UpdateAsync(id, updatedUser); // Cambiado a int
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) // Cambiado a int
        {
            var user = await _userService.GetByIdAsync(id); // Cambiado a int
            if (user == null) return NotFound();

            await _userService.DeleteAsync(id); // Cambiado a int
            return NoContent();
        }

        [HttpGet("validate/{id}")]
        public async Task<IActionResult> ValidateUserForLoan(int id) // Cambiado a int
        {
            var user = await _userService.GetByIdAsync(id); // Cambiado a int
            if (user == null) return NotFound();

            if (user.HasPenalty || user.HasActiveLoan)
                return BadRequest("Usuario no puede tomar prestado más libros.");

            return Ok("Usuario puede tomar prestado libros.");
        }
    }
}
