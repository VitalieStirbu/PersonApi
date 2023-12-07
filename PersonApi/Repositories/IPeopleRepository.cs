using PersonApi.Models;

namespace PersonApi.Repositories
{
    public interface IPeopleRepository
    {
        Task<IEnumerable<Person>> GetPeopleAsync();
        Task<Person?> GetPersonAsync(int id);
        Task AddPersonAsync(Person person);
        Task UpdatePersonAsync(Person person);
        Task DeletePersonAsync(Person person);
    }
}
