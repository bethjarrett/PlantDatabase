namespace PlantDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Water_date : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Water_Date",
                c => new
                    {
                        water_id = c.Int(nullable: false, identity: true),
                        water_date = c.DateTime(nullable: false),
                        plant_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.water_id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Water_Date");
        }
    }
}
