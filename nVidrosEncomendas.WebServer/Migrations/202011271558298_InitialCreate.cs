namespace NVidrosEncomendas.WebServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Anexos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NomeFicheiro = c.String(),
                        Anulado = c.Boolean(nullable: false),
                        Savs_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Savs", t => t.Savs_Id)
                .Index(t => t.Savs_Id);
            
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        IdCliente = c.Int(nullable: false, identity: true),
                        NumCliente = c.Int(nullable: false),
                        NomeCliente = c.String(),
                        Anulado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdCliente);
            
            CreateTable(
                "dbo.DepartamentoSavs",
                c => new
                    {
                        IdDepartamentoSav = c.Int(nullable: false, identity: true),
                        NumDepartamentoSav = c.Int(nullable: false),
                        NomeDepartamentoSav = c.String(),
                        Anulado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdDepartamentoSav);
            
            CreateTable(
                "dbo.Operadores",
                c => new
                    {
                        UtilizadorId = c.Int(nullable: false, identity: true),
                        AspIdentityId = c.String(),
                        NomeCompleto = c.String(nullable: false, maxLength: 100),
                        NomeLogin = c.String(nullable: false, maxLength: 15),
                        Email = c.String(nullable: false, maxLength: 200),
                        ImagemAvatar = c.String(maxLength: 50),
                        Password = c.String(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        Admin = c.Boolean(nullable: false),
                        Anulado = c.Boolean(nullable: false),
                        Sav = c.Boolean(nullable: false),
                        AdminSav = c.Boolean(nullable: false),
                        Comercial = c.Boolean(nullable: false),
                        CriadoData = c.DateTime(nullable: false),
                        EditadoData = c.DateTime(nullable: false),
                        DashboardDesde = c.DateTime(nullable: false),
                        EditadoPorId = c.Int(),
                        EditadoPor_UtilizadorId = c.Int(),
                    })
                .PrimaryKey(t => t.UtilizadorId)
                .ForeignKey("dbo.Operadores", t => t.EditadoPorId)
                .ForeignKey("dbo.Operadores", t => t.EditadoPor_UtilizadorId)
                .Index(t => t.EditadoPorId)
                .Index(t => t.EditadoPor_UtilizadorId);
            
            CreateTable(
                "dbo.Encomendas",
                c => new
                    {
                        IdEncomenda = c.Int(nullable: false, identity: true),
                        NumDoc = c.Int(nullable: false),
                        NumSerieEncomenda = c.String(),
                        NomeObra = c.String(),
                        Producao = c.String(),
                        SemanaEntrega = c.Int(nullable: false),
                        DataPedido = c.DateTime(nullable: false),
                        DataProducao = c.DateTime(),
                        DataEntrega = c.DateTime(),
                        GuiaRemessa = c.String(),
                        Notas = c.String(),
                        Anulada = c.Boolean(nullable: false),
                        NumVidros = c.Int(nullable: false),
                        Estado = c.Int(nullable: false),
                        Cliente_IdCliente = c.Int(),
                        SerieDoc_NumSerie = c.String(maxLength: 128),
                        TipoEncomenda_IdTipoEncomenda = c.Int(),
                    })
                .PrimaryKey(t => t.IdEncomenda)
                .ForeignKey("dbo.Clientes", t => t.Cliente_IdCliente)
                .ForeignKey("dbo.Series", t => t.SerieDoc_NumSerie)
                .ForeignKey("dbo.TipoEncomendas", t => t.TipoEncomenda_IdTipoEncomenda)
                .Index(t => t.Cliente_IdCliente)
                .Index(t => t.SerieDoc_NumSerie)
                .Index(t => t.TipoEncomenda_IdTipoEncomenda);
            
            CreateTable(
                "dbo.Series",
                c => new
                    {
                        NumSerie = c.String(nullable: false, maxLength: 128),
                        NomeSerie = c.String(),
                        Inativa = c.Boolean(nullable: false),
                        SerieDefeito = c.Boolean(nullable: false),
                        UltimoDoc = c.Int(nullable: false),
                        UltimoDocSav = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NumSerie);
            
            CreateTable(
                "dbo.TipoEncomendas",
                c => new
                    {
                        IdTipoEncomenda = c.Int(nullable: false, identity: true),
                        NumTipoEncomenda = c.Int(nullable: false),
                        NomeTipoEncomenda = c.String(),
                        Setor = c.String(),
                        Anulado = c.Boolean(nullable: false),
                        SetorEncomenda_IdSetorEncomenda = c.Int(),
                    })
                .PrimaryKey(t => t.IdTipoEncomenda)
                .ForeignKey("dbo.SetorEncomendas", t => t.SetorEncomenda_IdSetorEncomenda)
                .Index(t => t.SetorEncomenda_IdSetorEncomenda);
            
            CreateTable(
                "dbo.SetorEncomendas",
                c => new
                    {
                        IdSetorEncomenda = c.Int(nullable: false, identity: true),
                        NumSetor = c.Int(nullable: false),
                        Nome = c.String(),
                        Anulado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdSetorEncomenda);
            
            CreateTable(
                "dbo.EstadoRecolhas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NomeEstado = c.String(),
                        Cor = c.String(),
                        EstadoFechaRecolha = c.Boolean(nullable: false),
                        Anulado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EstadoSavs",
                c => new
                    {
                        IdEstadoSav = c.Int(nullable: false, identity: true),
                        NomeEstadoSav = c.String(),
                        SubEstado = c.Int(nullable: false),
                        MarcaEncerrado = c.Boolean(nullable: false),
                        PreSeleccionadoNaPesquisa = c.Boolean(nullable: false),
                        Anulado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdEstadoSav);
            
            CreateTable(
                "dbo.ProdutoSavs",
                c => new
                    {
                        IdProdutoSav = c.Int(nullable: false, identity: true),
                        NumProdutoSav = c.Int(nullable: false),
                        NomeProdutoSav = c.String(),
                        Anulado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdProdutoSav);
            
            CreateTable(
                "dbo.Recolhas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DataPedidoRecolha = c.DateTime(nullable: false),
                        DataRecolha = c.DateTime(nullable: false),
                        DataChegadaPrevista = c.DateTime(nullable: false),
                        RecolhaCompleta = c.Boolean(nullable: false),
                        EstadoProduto = c.String(),
                        EstadoRecolha_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EstadoRecolhas", t => t.EstadoRecolha_Id)
                .Index(t => t.EstadoRecolha_Id);
            
            CreateTable(
                "dbo.Relatorios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NomeUtilizador = c.String(),
                        HtmlQuery = c.String(),
                        Controller = c.String(),
                        Method = c.String(),
                        NomeFicheiro = c.String(),
                        TipoFicheiro = c.String(),
                        UniqueId = c.String(),
                        DataGerado = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Savs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DataSav = c.DateTime(nullable: false),
                        TipoAvariaExtra = c.String(),
                        DescricaoSav = c.String(),
                        Causa = c.String(),
                        AcaoImplementar = c.String(),
                        NumDoc = c.Int(nullable: false),
                        DataEstado = c.DateTime(nullable: false),
                        NotasAdicionais = c.String(),
                        MarcarResolvida = c.Boolean(nullable: false),
                        DataResolvida = c.DateTime(nullable: false),
                        NotasResolvida = c.String(),
                        MarcarRespostaAoCliente = c.Boolean(nullable: false),
                        DataRespostaAoCliente = c.DateTime(),
                        DataExpedicao = c.DateTime(),
                        RespostaAoCliente = c.String(),
                        DireitoNaoConformidade = c.Boolean(nullable: false),
                        CriadoData = c.DateTime(nullable: false),
                        EditadoData = c.DateTime(nullable: false),
                        Anulada = c.Boolean(nullable: false),
                        Ref = c.String(),
                        SemanaEntrega = c.Int(nullable: false),
                        Custos = c.Double(),
                        CustosTransporte = c.Double(),
                        CustosDescricao = c.String(),
                        TemRecolha = c.Boolean(nullable: false),
                        Cliente_IdCliente = c.Int(),
                        CriadoPor_UtilizadorId = c.Int(),
                        Departamento_IdDepartamentoSav = c.Int(),
                        EditadoPor_UtilizadorId = c.Int(),
                        Estado_IdEstadoSav = c.Int(),
                        Produto_IdProdutoSav = c.Int(),
                        Recolha_Id = c.Int(),
                        SerieDoc_NumSerie = c.String(maxLength: 128),
                        Setor_IdSetor = c.Int(),
                        TipoAvaria_IdTipoAvaria = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clientes", t => t.Cliente_IdCliente)
                .ForeignKey("dbo.Operadores", t => t.CriadoPor_UtilizadorId)
                .ForeignKey("dbo.DepartamentoSavs", t => t.Departamento_IdDepartamentoSav)
                .ForeignKey("dbo.Operadores", t => t.EditadoPor_UtilizadorId)
                .ForeignKey("dbo.EstadoSavs", t => t.Estado_IdEstadoSav)
                .ForeignKey("dbo.ProdutoSavs", t => t.Produto_IdProdutoSav)
                .ForeignKey("dbo.Recolhas", t => t.Recolha_Id)
                .ForeignKey("dbo.Series", t => t.SerieDoc_NumSerie)
                .ForeignKey("dbo.Setors", t => t.Setor_IdSetor)
                .ForeignKey("dbo.TipoAvarias", t => t.TipoAvaria_IdTipoAvaria)
                .Index(t => t.Cliente_IdCliente)
                .Index(t => t.CriadoPor_UtilizadorId)
                .Index(t => t.Departamento_IdDepartamentoSav)
                .Index(t => t.EditadoPor_UtilizadorId)
                .Index(t => t.Estado_IdEstadoSav)
                .Index(t => t.Produto_IdProdutoSav)
                .Index(t => t.Recolha_Id)
                .Index(t => t.SerieDoc_NumSerie)
                .Index(t => t.Setor_IdSetor)
                .Index(t => t.TipoAvaria_IdTipoAvaria);
            
            CreateTable(
                "dbo.Setors",
                c => new
                    {
                        IdSetor = c.Int(nullable: false, identity: true),
                        NumSetor = c.Int(nullable: false),
                        Nome = c.String(),
                        Anulado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdSetor);
            
            CreateTable(
                "dbo.TipoAvarias",
                c => new
                    {
                        IdTipoAvaria = c.Int(nullable: false, identity: true),
                        NumTipoAvaria = c.Int(nullable: false),
                        NomeTipoAvaria = c.String(),
                        Anulado = c.Boolean(nullable: false),
                        InfoExtra = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdTipoAvaria);
            
            CreateTable(
                "dbo.OperadoresDepartamentoSavs",
                c => new
                    {
                        Operadores_UtilizadorId = c.Int(nullable: false),
                        DepartamentoSav_IdDepartamentoSav = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Operadores_UtilizadorId, t.DepartamentoSav_IdDepartamentoSav })
                .ForeignKey("dbo.Operadores", t => t.Operadores_UtilizadorId, cascadeDelete: true)
                .ForeignKey("dbo.DepartamentoSavs", t => t.DepartamentoSav_IdDepartamentoSav, cascadeDelete: true)
                .Index(t => t.Operadores_UtilizadorId)
                .Index(t => t.DepartamentoSav_IdDepartamentoSav);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Savs", "TipoAvaria_IdTipoAvaria", "dbo.TipoAvarias");
            DropForeignKey("dbo.Savs", "Setor_IdSetor", "dbo.Setors");
            DropForeignKey("dbo.Savs", "SerieDoc_NumSerie", "dbo.Series");
            DropForeignKey("dbo.Savs", "Recolha_Id", "dbo.Recolhas");
            DropForeignKey("dbo.Savs", "Produto_IdProdutoSav", "dbo.ProdutoSavs");
            DropForeignKey("dbo.Savs", "Estado_IdEstadoSav", "dbo.EstadoSavs");
            DropForeignKey("dbo.Savs", "EditadoPor_UtilizadorId", "dbo.Operadores");
            DropForeignKey("dbo.Savs", "Departamento_IdDepartamentoSav", "dbo.DepartamentoSavs");
            DropForeignKey("dbo.Savs", "CriadoPor_UtilizadorId", "dbo.Operadores");
            DropForeignKey("dbo.Savs", "Cliente_IdCliente", "dbo.Clientes");
            DropForeignKey("dbo.Anexos", "Savs_Id", "dbo.Savs");
            DropForeignKey("dbo.Recolhas", "EstadoRecolha_Id", "dbo.EstadoRecolhas");
            DropForeignKey("dbo.Encomendas", "TipoEncomenda_IdTipoEncomenda", "dbo.TipoEncomendas");
            DropForeignKey("dbo.TipoEncomendas", "SetorEncomenda_IdSetorEncomenda", "dbo.SetorEncomendas");
            DropForeignKey("dbo.Encomendas", "SerieDoc_NumSerie", "dbo.Series");
            DropForeignKey("dbo.Encomendas", "Cliente_IdCliente", "dbo.Clientes");
            DropForeignKey("dbo.Operadores", "EditadoPor_UtilizadorId", "dbo.Operadores");
            DropForeignKey("dbo.OperadoresDepartamentoSavs", "DepartamentoSav_IdDepartamentoSav", "dbo.DepartamentoSavs");
            DropForeignKey("dbo.OperadoresDepartamentoSavs", "Operadores_UtilizadorId", "dbo.Operadores");
            DropForeignKey("dbo.Operadores", "EditadoPorId", "dbo.Operadores");
            DropIndex("dbo.OperadoresDepartamentoSavs", new[] { "DepartamentoSav_IdDepartamentoSav" });
            DropIndex("dbo.OperadoresDepartamentoSavs", new[] { "Operadores_UtilizadorId" });
            DropIndex("dbo.Savs", new[] { "TipoAvaria_IdTipoAvaria" });
            DropIndex("dbo.Savs", new[] { "Setor_IdSetor" });
            DropIndex("dbo.Savs", new[] { "SerieDoc_NumSerie" });
            DropIndex("dbo.Savs", new[] { "Recolha_Id" });
            DropIndex("dbo.Savs", new[] { "Produto_IdProdutoSav" });
            DropIndex("dbo.Savs", new[] { "Estado_IdEstadoSav" });
            DropIndex("dbo.Savs", new[] { "EditadoPor_UtilizadorId" });
            DropIndex("dbo.Savs", new[] { "Departamento_IdDepartamentoSav" });
            DropIndex("dbo.Savs", new[] { "CriadoPor_UtilizadorId" });
            DropIndex("dbo.Savs", new[] { "Cliente_IdCliente" });
            DropIndex("dbo.Recolhas", new[] { "EstadoRecolha_Id" });
            DropIndex("dbo.TipoEncomendas", new[] { "SetorEncomenda_IdSetorEncomenda" });
            DropIndex("dbo.Encomendas", new[] { "TipoEncomenda_IdTipoEncomenda" });
            DropIndex("dbo.Encomendas", new[] { "SerieDoc_NumSerie" });
            DropIndex("dbo.Encomendas", new[] { "Cliente_IdCliente" });
            DropIndex("dbo.Operadores", new[] { "EditadoPor_UtilizadorId" });
            DropIndex("dbo.Operadores", new[] { "EditadoPorId" });
            DropIndex("dbo.Anexos", new[] { "Savs_Id" });
            DropTable("dbo.OperadoresDepartamentoSavs");
            DropTable("dbo.TipoAvarias");
            DropTable("dbo.Setors");
            DropTable("dbo.Savs");
            DropTable("dbo.Relatorios");
            DropTable("dbo.Recolhas");
            DropTable("dbo.ProdutoSavs");
            DropTable("dbo.EstadoSavs");
            DropTable("dbo.EstadoRecolhas");
            DropTable("dbo.SetorEncomendas");
            DropTable("dbo.TipoEncomendas");
            DropTable("dbo.Series");
            DropTable("dbo.Encomendas");
            DropTable("dbo.Operadores");
            DropTable("dbo.DepartamentoSavs");
            DropTable("dbo.Clientes");
            DropTable("dbo.Anexos");
        }
    }
}
