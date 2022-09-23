using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.meespostma.nl.Data;
using api.meespostma.nl.Models.Projects;
using AutoMapper;
using api.meespostma.nl.Static;

namespace api.meespostma.nl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]     
    public class ProjectsController : ControllerBase
    {
        private readonly ApiMeesPostmaContext _context;
        private readonly IMapper mapper;
        private readonly ILogger<ProjectsController> logger;

        public ProjectsController(ApiMeesPostmaContext context, IMapper mapper, ILogger<ProjectsController> logger)
        {
            _context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectReadOnlyDto>>> GetProjects()
        {
            try
            {
                var projectDtos = mapper.Map<IEnumerable<ProjectReadOnlyDto>>(await _context.Projects.ToListAsync());
                
                return Ok(projectDtos);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error Performing GET in {nameof(GetProjects)}");

                return StatusCode(500, Messages.Error500Message);
            }
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectReadOnlyDto>> GetProject(int id)
        {
            try
            {
                var project = await _context.Projects.FindAsync(id);

                if (project == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<ProjectReadOnlyDto>(project));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error Performing GET in {nameof(GetProject)}");

                return StatusCode(500, Messages.Error500Message);
            }
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, Project project)
        {
            if (id != project.Id)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProjectCreateDto>> PostProject(ProjectCreateDto projectDto)
        {
            try
            {
                var project = mapper.Map<Project>(projectDto);
                await _context.Projects.AddAsync(project);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error Performing POST in {nameof(PostProject)}", projectDto);
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
