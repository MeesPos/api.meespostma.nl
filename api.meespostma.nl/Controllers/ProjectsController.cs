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
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProjectsController(ApiMeesPostmaContext context, IMapper mapper, ILogger<ProjectsController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.mapper = mapper;
            this.logger = logger;
            this.webHostEnvironment = webHostEnvironment;
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
        public async Task<IActionResult> PutProject(int id, ProjectUpdateDto projectDto)
        {
            if (id != projectDto.Id)
            {
                return BadRequest();
            }

            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(projectDto.Logo) == false)
            {
                projectDto.Logo = CreateFile(projectDto.Logo, projectDto.OriginalLogoName);

                var picName = Path.GetFileName(project.Logo);
                var path = $"{webHostEnvironment.WebRootPath}\\projectlogos\\{picName}";

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            mapper.Map(projectDto, project);
            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    logger.LogError(ex, $"Error Performing GET in {nameof(PutProject)}");

                    return StatusCode(500, Messages.Error500Message);
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

                if (string.IsNullOrEmpty(projectDto.Logo) == false)
                {
                    project.Logo = CreateFile(projectDto.Logo, projectDto.OriginalLogoName);
                }

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

        private string CreateFile(string imageBase64, string imageName)
        {
            var url = HttpContext.Request.Host.Value;
            var ext = Path.GetExtension(imageName);
            var fileName = $"{Guid.NewGuid()}{ext}";

            var path = $"{webHostEnvironment.WebRootPath}\\projectlogos\\{fileName}";

            byte[] image = Convert.FromBase64String(imageBase64);

            var fileStream = System.IO.File.Create(path);
            fileStream.Write(image, 0, image.Length);
            fileStream.Close();

            return $"https://{url}/projectlogos/{fileName}";
        }

        private async Task<bool> ProjectExists(int id)
        {
            return await _context.Projects.AnyAsync(e => e.Id == id);
        }
    }
}
