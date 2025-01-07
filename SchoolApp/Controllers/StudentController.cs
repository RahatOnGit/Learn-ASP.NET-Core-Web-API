using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using SchoolApp.Data;
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
        private readonly CollegeDbContext _dbContext;

        public StudentController(ILogger<StudentController> logger, CollegeDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        [HttpGet]
        [Route("All")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult GetStudents()
        {
            _logger.LogTrace("All Students called");
            _logger.LogDebug("All Students called");

            _logger.LogInformation("All Students called");
            _logger.LogWarning("All Students called");
            _logger.LogError("All Students called");
            _logger.LogCritical("All Students called");



                
            var students_dto = _dbContext.Students.Select(s=> new StudentDTO()
            {
                Id = s.Id,
                StudentName = s.StudentName,
                Email = s.Email,
                Address = s.Address,
                DOB = s.DOB

            }).ToList();
            

            


            return Ok(students_dto);

        }

        [HttpGet("{id:int}", Name ="GetById")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]



        public IActionResult GetStudentById(int id)
        {
            if (id<=0)
            {
                _logger.LogWarning("Warning");
                return BadRequest();
            }
           
            var data =  _dbContext.Students.Where(c => c.Id == id).FirstOrDefault(); 
            
            if (data==null)
            {
                _logger.LogError("Some error");
                return NotFound($"The student Id= {id} doesn't exists.");
            }

            var s = new StudentDTO()
            {
                Id = data.Id,
                StudentName = data.StudentName,
                Email = data.Email,
                Address = data.Address,
                DOB = data.DOB

            };

            return Ok(s);
        }

        [HttpGet]
        [Route("{name:alpha}", Name ="GetStudentByName")]
        public IActionResult GetStudentByName(string name)
        {
            return Ok(_dbContext.Students.Where(c => c.StudentName == name).FirstOrDefault());

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


        public IActionResult CreateStudent([FromBody]StudentDTO model)
        {
            
            if (model==null)
                return BadRequest();

            //if (model.AdmissionDate < DateTime.Now)
            //{
            //    ModelState.AddModelError("Admission Date Error", "Admission date must be " +
            //        "greater than or equal to current time");

            //    return BadRequest(ModelState);
            //}

          

            Student student = new Student()
            {
                
                StudentName=model.StudentName,
                Email=model.Email,
                Address = model.Address,
                DOB = model.DOB
            };

            _dbContext.Students.Add(student);
            _dbContext.SaveChanges(); // Committing to the DataBase

            model.Id = student.Id;

            return CreatedAtRoute("GetById", new {id = model.Id} ,model);
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateStudent([FromBody] StudentDTO model)
        {
            if(model.Id<=0 || model==null)
                return BadRequest();

            var student = _dbContext.Students.Where(s=>s.Id==model.Id).FirstOrDefault();
            
            if (student==null)
                return NotFound();

            student.Email = model.Email;
            student.Address = model.Address;
            student.StudentName = model.StudentName;

            _dbContext.SaveChanges();

            return NoContent();

        }


        [HttpPatch]
        [Route("{id:int}/updateStudentPartial")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> model)
        {
            if (id <= 0 || model == null)
                return BadRequest();

            var student = _dbContext.Students.Where(s => s.Id == id).FirstOrDefault();

            if (student == null)
                return NotFound();

            var studentDTO = new StudentDTO
            {
                Id = student.Id,
                Email = student.Email,
                Address = student.Address,
                StudentName = student.StudentName,
                DOB = student.DOB
            };

            model.ApplyTo(studentDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            student.Email = studentDTO.Email;
            student.Address = studentDTO.Address;
            student.StudentName = studentDTO.StudentName;
            student.DOB = studentDTO.DOB;

            _dbContext.SaveChanges();

            return NoContent();

        }


        [HttpDelete("delete/{id:int}")]
        // api/student/delete/1
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult DeleteStudent(int id)
        {
            if (id<=0)
                return BadRequest();

            var student = _dbContext.Students.Where(s => s.Id == id).FirstOrDefault();

            if (student == null)
                return NotFound($"The student with id = {id} isn't found");
            
            _dbContext.Students.Remove(student);
            _dbContext.SaveChanges();

            return Ok(true);


        }
    }
}
