using Microsoft.EntityFrameworkCore;
using PersonApi.Models;

namespace PersonApi.Repositories
{
    public class PeopleRepository : IPeopleRepository
    {
        private readonly PeopleContext _personContext;
        public PeopleRepository(PeopleContext personContext)
        {
            _personContext = personContext;
        }

        public async Task<IEnumerable<Person>> GetPeopleAsync()
        {
            return await _personContext.People.ToListAsync();
        }

        public async Task<Person?> GetPersonAsync(int id)
        {
            return await _personContext.People.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddPersonAsync(Person person)
        {
            await _personContext.People.AddAsync(person);
            await _personContext.SaveChangesAsync();
        }

        public async Task UpdatePersonAsync(Person person)
        {
            _personContext.People.Update(person);
            await _personContext.SaveChangesAsync();
        }

        public async Task DeletePersonAsync(Person person)
        {
            _personContext.People.Remove(person);
            await _personContext.SaveChangesAsync();
        }
    }
}
