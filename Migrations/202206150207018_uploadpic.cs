namespace PlantDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uploadpic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Plants", "planthaspic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Plants", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Plants", "PicExtension");
            DropColumn("dbo.Plants", "planthaspic");
        }
    }
}
