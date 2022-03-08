using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ImmersiveQuiz.Models;

namespace ImmersiveQuiz.Data
{
    public class ScoreContext : DbContext
    {
        public ScoreContext (DbContextOptions<ScoreContext> options)
            : base(options)
        {
        }

        public DbSet<ImmersiveQuiz.Models.Score> Score { get; set; }
    }
}
