namespace server_api
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AirUDBCOE : DbContext
    {
        
        public AirUDBCOE()
            : base("name=AirUDBCOE")
        {
        }
        
        public AirUDBCOE(string connectionString)
            : base(connectionString.Equals("")?"name=AirUDBCOE":connectionString)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<DataPoint> DataPoints { get; set; }
        public virtual DbSet<StationGroup> DeviceGroups { get; set; }
        public virtual DbSet<Station> Stations { get; set; }
        public virtual DbSet<Parameter> Parameters { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
