using stackblob.Domain.Enums;
using stackblob.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Infrastructure.Persistence;

public static class StackblobDbContextSeed
{
    public static async Task SeedSampleData(StackblobDbContext context)
    {
        await context.SaveChangesAsync(default);

    }
}
