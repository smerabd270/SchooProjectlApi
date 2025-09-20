using SchooProjectlApi.Entities;

namespace SchooProjectlApi.Services
{
    public interface IGradeService
    {
        // Teacher assigns or updates a grade for a student’s assignment
        Task<Grade?> AssignGradeAsync(int assignmentId, int studentId, int score, string? feedback, int teacherId);

        // Student views all their grades
        Task<IEnumerable<Grade>> GetGradesByStudentAsync(int studentId);
    }
}
