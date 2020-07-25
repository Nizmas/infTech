using Microsoft.EntityFrameworkCore;

namespace InfTechWeb.DBSetting

{
    public sealed class ApplicationContext : DbContext //определяет контекст данных, используемый для взаимодействия с базой данных
    {
        public DbSet<FolderModel> Folders { get; set; } //создаём виртуальную копия БД
        public DbSet<FileModel> Files { get; set; }
        public DbSet<ExtensionsModel> Extensions { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //устанавливает параметры подключения
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=inftech;User Id=SA;Password=Nizayev1;");
        }
    }
}