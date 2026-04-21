namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCompraModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DetalleOrdens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrdenId = c.Int(nullable: false),
                        ProductoId = c.Int(nullable: false),
                        Nombre = c.String(),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cantidad = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ordens", t => t.OrdenId, cascadeDelete: true)
                .Index(t => t.OrdenId);
            
            CreateTable(
                "dbo.Ordens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsuarioId = c.String(),
                        Fecha = c.DateTime(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Estado = c.String(),
                        Direccion = c.String(),
                        MetodoPago = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.CompraModels", "UsuarioId", c => c.String(nullable: false));
            AddColumn("dbo.CompraModels", "Cantidad", c => c.Int(nullable: false));
            AddColumn("dbo.CompraModels", "FechaCompra", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DetalleOrdens", "OrdenId", "dbo.Ordens");
            DropIndex("dbo.DetalleOrdens", new[] { "OrdenId" });
            DropColumn("dbo.CompraModels", "FechaCompra");
            DropColumn("dbo.CompraModels", "Cantidad");
            DropColumn("dbo.CompraModels", "UsuarioId");
            DropTable("dbo.Ordens");
            DropTable("dbo.DetalleOrdens");
        }
    }
}
