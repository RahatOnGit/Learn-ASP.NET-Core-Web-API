using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
    [Authorize]
    public class StudentController : ControllerBase
    {

        private readonly ILogger<StudentController> _logger;
        private readonly IMapper _mapper;

        private readonly IStudentRepository _studentRepository;                      

        private APIResponse _response;
        public StudentController(ILogger<StudentController> logger, IStudentRepository studentRepository,
            IMapper mapper)
        {
            _logger = logger;
            _studentRepository = studentRepository;
            _mapper = mapper;
            _response = new();
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


            try
            {

                var students = await _studentRepository.GetAll();

                _response.Data = _mapper.Map<List<StudentDTO>>(students);

                _response.Status = true;

                _response.StatusCode = HttpStatusCode.OK;

                

                

                return Ok(_response);
            }

            catch (Exception ex)
            {
                _response.Errors.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Status = false;

                return new ObjectResult(_response)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
          

        }

        [HttpGet("{id:int}", Name ="GetById")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]



        public async Task<IActionResult> GetStudentById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Warning");
                    return BadRequest();
                }

                var data = await _studentRepository.GetByFilterValue(student => student.Id == id);

                if (data == null)
                {
                    _logger.LogError("Some error");
                    return NotFound($"The student Id= {id} doesn't exists.");
                }

                _response.Data = _mapper.Map<StudentDTO>(data);

                _response.Status = true;

                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);



            }




            catch (Exception ex)
            {
                _response.Errors.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Status = false;

                return new ObjectResult(_response)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        [HttpGet]
        [Route("{name:alpha}", Name = "GetStudentByName")]
        public async Task<IActionResult> GetStudentByName(string name)
        {

            try
            {
                var data = await _studentRepository.GetByFilterValue(student => student.StudentName.ToLower().Contains(name.ToLower()));

                _response.Data = _mapper.Map<StudentDTO>(data);

                _response.Status = true;

                _response.StatusCode = HttpStatusCode.OK;




                return Ok(_response);
            }

            catch (Exception ex)
            {
                _response.Errors.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Status = false;

                return new ObjectResult(_response)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

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
            try
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

                var data = await _studentRepository.Create(student); // Committing to the DataBase

                model.Id = data.Id;

                _response.Data = model;
                _response.Status = true;
                _response.StatusCode = HttpStatusCode.OK;

                return CreatedAtRoute("GetById", new { id = model.Id }, _response);

            }

            catch (Exception ex)
            {
                _response.Errors.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Status = false;

                return new ObjectResult(_response)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStudent([FromBody] StudentDTO model)
        {

            try
            {
                if (model.Id <= 0 || model == null)
                    return BadRequest();

                var student = await _studentRepository.GetByFilterValue(student => student.Id == model.Id, true);

                if (student == null)
                    return NotFound();

                var newStudent = _mapper.Map<Student>(model);



                await _studentRepository.Update(newStudent);

                return NoContent();
            }

            catch (Exception ex)
            {
                _response.Errors.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Status = false;

                return new ObjectResult(_response)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }


        [HttpPatch]
        [Route("{id:int}/updateStudentPartial")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> model)
        {
            try
            {
                if (id <= 0 || model == null)
                    return BadRequest();

                var student = await _studentRepository.GetByFilterValue(student => student.Id == id, true);

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

            catch (Exception ex)
            {
                _response.Errors.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Status = false;

                return new ObjectResult(_response)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }




        }


        [HttpDelete("delete/{id:int}")]
        // api/student/delete/1
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest();

                var student = await _studentRepository.GetByFilterValue(student => student.Id == id);

                if (student == null)
                    return NotFound($"The student with id = {id} isn't found");

                await _studentRepository.Delete(student);

                _response.Status = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Data = true;

                return Ok(_response);

            }

            catch (Exception ex)
            {
                _response.Errors.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Status = false;

                return new ObjectResult(_response)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }


        }

        [HttpGet("ByFees")]
     
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStudebtDataByFees()
        {
            var data = await _studentRepository.GetStudentsByFeeStatus(0);
            if (data == null)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
