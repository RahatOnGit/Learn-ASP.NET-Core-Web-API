namespace SchoolApp.Data.Repository
{
    public interface IStudentRepository : ISchoolRepository<Student>
    {
       Task<List<Student>> GetStudentsByFeeStatus(int feeStatus);
    }
}
