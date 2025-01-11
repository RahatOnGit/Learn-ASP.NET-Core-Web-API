using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolApp.Data;
using SchoolApp.Data.Repository;
using SchoolApp.Models;
using System.Linq;
using System.Net;

namespace SchoolApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class StudentController : ControllerBase
    {

        private readonly ILogger<StudentController> _logger;
        private readonly IMapper _mapper;

        private readonly IStudentRepository _studentRepository;                      

        public StudentController(ILogger<StudentController> logger, IStudentRepository studentRepository,
            IMapper mapper)
        {
            _logger = logger;
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("All")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetStudents()
        {
            _logger.LogTrace("All Students called");
            _logger.LogDebug("All Students called");

            _logger.LogInformation("All Students called");
            _logger.LogWarning("All Students called");
            _logger.LogError("All Students called");
            _logger.LogCritical("All Students called");



                
           var students = await _studentRepository.GetAll();

           var students_dto = _mapper.Map<List<StudentDTO>>(students);
            

            


            return Ok(students_dto);

        }

        [HttpGet("{id:int}", Name ="GetById")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]



        public async Task<IActionResult> GetStudentById(int id)
        {
            if (id<=0)
            {
                _logger.LogWarning("Warning");
                return BadRequest();
            }
           
            var data =  await _studentRepository.GetById(id);
            
            if (data==null)
            {
                _logger.LogError("Some error");
                return NotFound($"The student Id= {id} doesn't exists.");
            }

            var s = _mapper.Map<StudentDTO>(data);

            return Ok(s);
        }

        [HttpGet]
        [Route("{name:alpha}", Name ="GetStudentByName")]
        public async Task<IActionResult> GetStudentByName(string name)
        {
            return Ok(await _studentRepository.GetByName(name));

        }

        //[HttpDelete("{id:int}")]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(500)]
        //[ProducesResponseType (204)]

        //public ActionResult DeleteStudentById(int id)
        //{
        //    var data =  _dbContext.Students.Where(c => c.Id == id).FirstOrDefault();
        //    if (data == null) 
        //    {
        //        return NotFound();
        //    }
           
        //    _dbContext.Students.Remove(data);
        //    _dbContext.SaveChanges();

        //   return NoContent();
        //}

        [HttpPost("Create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]


        public async Task<IActionResult> CreateStudent([FromBody]StudentDTO model)
        {

            if (model == null)
                return BadRequest();

            //if (model.AdmissionDate < DateTime.Now)
            //{
            //    ModelState.AddModelError("Admission Date Error", "Admission date must be " +
            //        "greater than or equal to current time");

            //    return BadRequest(ModelState);
            //}



            var student = _mapper.Map<Student>(model);

            var id = await _studentRepository.Create(student); // Committing to the DataBase

            model.Id = id;

            return CreatedAtRoute("GetById", new {id = model.Id} ,model);
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStudent([FromBody] StudentDTO model)
        {
            if(model.Id<=0 || model==null)
                return BadRequest();

            var student = await _studentRepository.GetById(model.Id, true);
            
            if (student==null)
                return NotFound();

            var newStudent = _mapper.Map<Student>(model);

           

           await _studentRepository.Update(newStudent);

           return NoContent();
        }


        [HttpPatch]
        [Route("{id:int}/updateStudentPartial")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> model)
        {
            if (id <= 0 || model == null)
                return BadRequest();

            var student = await _studentRepository.GetById(id, true);

            if (student == null)
                return NotFound();

            var studentDTO = _mapper.Map<StudentDTO>(student);

            model.ApplyTo(studentDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            student = _mapper.Map<Student>(studentDTO);

            await _studentRepository.Update(student);

            return NoContent();

        }


        [HttpDelete("delete/{id:int}")]
        // api/student/delete/1
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> DeleteStudent(int id)
        {
            if (id<=0)
                return BadRequest();

            var student = await _studentRepository.GetById(id);

            if (student == null)
                return NotFound($"The student with id = {id} isn't found");
            
            await _studentRepository.Delete(student);

            return Ok(true);


        }
    }
}
