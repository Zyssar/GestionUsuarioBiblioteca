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
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return user;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            await _userService.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] User updatedUser)
        {
            var user = await _userService.GetByIdAsync(id); 
            if (user == null) return NotFound();

            updatedUser.Id = user.Id; 
            await _userService.UpdateAsync(id, updatedUser);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetByIdAsync(id); 
            if (user == null) return NotFound();

            await _userService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("validate/{id}")]
        public async Task<IActionResult> ValidateUserForLoan(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();

            if (user.HasPenalty || user.HasActiveLoan)
                return StatusCode(403, "Usuario no puede tomar prestado más libros.");  // Código de respuesta cambiado a 403

            return Ok("Usuario puede tomar prestado libros.");
        }

    }
}
