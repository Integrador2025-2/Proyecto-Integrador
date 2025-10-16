using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class CadenaDeValorRepository : ICadenaDeValorRepository
{
    private readonly ApplicationDbContext _context;
    public CadenaDeValorRepository(ApplicationDbContext context) => _context = context;

    public async Task<List<CadenaDeValor>> GetAllAsync() => await _context.CadenasDeValor.AsNoTracking().ToListAsync();

    public async Task<CadenaDeValor?> GetByIdAsync(int id) => await _context.CadenasDeValor.FindAsync(id);

    public async Task<CadenaDeValor> CreateAsync(CadenaDeValor entity)
    {
        _context.CadenasDeValor.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<CadenaDeValor> UpdateAsync(CadenaDeValor entity)
    {
        _context.CadenasDeValor.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _context.CadenasDeValor.FindAsync(id);
        if (e == null) return false;
        _context.CadenasDeValor.Remove(e);
        await _context.SaveChangesAsync();
        return true;
    }
}
