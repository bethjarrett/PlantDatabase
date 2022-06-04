namespace PlantDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class june2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Water_Date", "water_day", c => c.DateTime(nullable: false));
            DropColumn("dbo.Water_Date", "water_date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Water_Date", "water_date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Water_Date", "water_day");
        }
    }
}
