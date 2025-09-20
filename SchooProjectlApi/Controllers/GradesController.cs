using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchooProjectlApi.DTOs.AssignmentDtos;
using SchooProjectlApi.Entities;
using SchooProjectlApi.Services;
using System.Security.Claims;

namespace SchooProjectlApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly IGradeService _grades;

        public GradesController(IGradeService grades)
        {
            _grades = grades;
        }

        // Teacher: Assign or update grade for a student's assignment
        [HttpPost("assign")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> AssignGrade(AssignmentGradeDto dto)
        {
            var teacherId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var grade = await _grades.AssignGradeAsync(
                dto.AssignmentId,
                dto.StudentId,
                dto.Score,
                dto.Feedback,
                teacherId
            );

            if (grade == null)
                return BadRequest("Failed to assign grade. Ensure you own the assignment or it exists.");

            return Ok(new { Message = "Grade assigned successfully.", Grade = grade });
        }

        // Get all grades for a specific student (Admin/Teacher can view all, Student can view own)
        [HttpGet("student/{studentId}")]
        [Authorize(Roles = "Teacher,Admin,Student")]
        public async Task<IActionResult> GetStudentGrades(int studentId)
        {
            // If role is student, ensure they can only access their own grades
            if (User.IsInRole("Student"))
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                if (studentId != userId) return Forbid();
            }

            var grades = await _grades.GetGradesByStudentAsync(studentId);
            return Ok(grades);
        }
    }
}
