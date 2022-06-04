namespace PlantDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class plantwater : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Water_Date", "plant_id");
            AddForeignKey("dbo.Water_Date", "plant_id", "dbo.Plants", "plant_id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Water_Date", "plant_id", "dbo.Plants");
            DropIndex("dbo.Water_Date", new[] { "plant_id" });
        }
    }
}
