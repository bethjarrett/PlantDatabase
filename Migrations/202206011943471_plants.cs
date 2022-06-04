namespace PlantDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class plants : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Plants",
                c => new
                    {
                        plant_id = c.Int(nullable: false, identity: true),
                        plant_name = c.String(),
                        plant_water = c.Int(nullable: false),
                        plant_humidity = c.Int(nullable: false),
                        plant_light = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.plant_id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Plants");
        }
    }
}
