using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen_Capture.MVVM.Model
{
    public class HotKeyDBContextFactory : IDesignTimeDbContextFactory<HotKeyDBContext>
    {
        public HotKeyDBContext CreateDbContext(string[] args=null)
        {
            var db = new DbContextOptionsBuilder<HotKeyDBContext>();
            db.UseSqlite("Data Source =HotKeyDB.db");

            return new HotKeyDBContext(db.Options);
        }
    }
}
