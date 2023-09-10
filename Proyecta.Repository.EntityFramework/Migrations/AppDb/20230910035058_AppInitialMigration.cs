using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Proyecta.Repository.EntityFramework.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class AppInitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RiskCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiskOwner",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskOwner", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiskTreatment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskTreatment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Risks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<int>(type: "integer", nullable: false),
                    DateFrom = table.Column<DateOnly>(type: "date", nullable: false),
                    DateTo = table.Column<DateOnly>(type: "date", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Phase = table.Column<int>(type: "integer", nullable: false),
                    Manageability = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TreatmentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Risks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Risks_RiskCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "RiskCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Risks_RiskOwner_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "RiskOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Risks_RiskTreatment_TreatmentId",
                        column: x => x.TreatmentId,
                        principalTable: "RiskTreatment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "RiskCategory",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("308b1695-1722-4773-8661-01336ecd1998"), "HSE y seguridad física" },
                    { new Guid("3deb0dab-f2fe-4df2-a48f-7af824c60592"), "Arqueología" },
                    { new Guid("4c4c9430-0bcc-4d60-b409-d8c6109cbc8d"), "Comisionamiento y arranque" },
                    { new Guid("4f069308-68af-4b5d-ab8c-ed1103dcba47"), "Ambiental" },
                    { new Guid("5034e84f-75f0-40e6-a912-184e68f210dd"), "Abastecimiento" },
                    { new Guid("8a0a6aed-631b-4009-bb97-23e072668d84"), "Ingeniería" },
                    { new Guid("939aed64-27b1-4d45-9803-a88ba4ccac7a"), "Perforación, completamiento y workover" },
                    { new Guid("c366fcbc-bd3f-4fdf-98c0-ad4c0720b378"), "Entorno" },
                    { new Guid("cc7ef7a0-2f09-4a54-8527-87d0de4425a9"), "Yacimientos" },
                    { new Guid("cd821c53-efd4-465c-939c-5acc03b1f77f"), "Gerenciamiento del proyecto" },
                    { new Guid("d1144533-b4f8-455b-bc82-54ef7bb820f2"), "Montaje y construcción" },
                    { new Guid("d2c1621f-2efa-4c6e-a614-fc20f67a8fda"), "Legislativo, normativo y/o tributario" },
                    { new Guid("dc9f763b-8ce6-49a9-9261-a3ac7287501d"), "Inmobiliario" },
                    { new Guid("e6211a79-a7b5-42cf-ad8d-810b7c44db33"), "Calidad y materiales" }
                });

            migrationBuilder.InsertData(
                table: "RiskOwner",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("042fa692-ba22-490d-8aab-2d9795bb5dd3"), "Orlando Pinto Lozano" },
                    { new Guid("0bbb0eee-570a-49e9-8725-bbdb1277a9d9"), "Carlos Alberto Sierra Pulido" },
                    { new Guid("0ce0850f-db55-4005-b779-532a45f03103"), "Henry Rivera Carrillo" },
                    { new Guid("118913f3-63bb-42b4-be97-03085d91bd3f"), "Francisco Javier Rey Navas" },
                    { new Guid("12ce57b3-7c9f-40ce-a379-c581d98e1dd2"), "Luis Fernando Salgado Ortiz" },
                    { new Guid("159fa26a-a787-4a66-b91d-f2fd01622fa3"), "Bernardo Diego Acevedo Rojas" },
                    { new Guid("166c9a11-bee9-4983-968d-f9508fa1ba3b"), "Fernando Carrera Calderon" },
                    { new Guid("1bc586e4-df03-4d96-9496-06820afdb285"), "Luis Enrique Soto Mayorga" },
                    { new Guid("1df4b57d-96a3-43ca-bcb7-8d5b1d221be3"), "Jose Antonio Padilla Pereira" },
                    { new Guid("29748b6c-3f6e-4fb8-a1d5-01264251610b"), "Claudia Parra Diaz" },
                    { new Guid("2d744abb-27db-4433-9110-30c0cfef9a98"), "Carmen Adriana Hennessey" },
                    { new Guid("42ccf3a4-c3b0-4bd7-84ec-df184bdcc8d9"), "Dagoberto Rivera Dussan" },
                    { new Guid("4915e049-902f-4a32-a3cf-224fc4953b9a"), "Ahamelth Yesid Hernandez Viana" },
                    { new Guid("50285870-e1e2-41c4-adeb-0916308818a7"), "Leonardo Alberto Guzman Canonigo" },
                    { new Guid("5ad7ef59-b58d-409c-845b-4e940cf227f1"), "Luisa Fernanda Ortiz Triana" },
                    { new Guid("5b17118c-c980-4606-b3ee-09c4ae143724"), "Diego Andres Orduz Tibaduiza" },
                    { new Guid("5ef74348-1c30-445e-bdf0-82c60837b448"), "Francisco Ascencio Alba" },
                    { new Guid("5f61fe86-dced-412b-b24c-2e07f52a22a1"), "Nahun Edgardo Perez de la Rosa" },
                    { new Guid("66865fab-e455-4521-baa1-5a54e03746aa"), "Juan Carlos Heredia Sanchez" },
                    { new Guid("68be25c5-fbef-40d2-947b-afe5b5b5108e"), "Eduardo Lozano Lozano" },
                    { new Guid("6bff6f50-feaf-4809-89fd-9f12216c1592"), "Mauricio Cufiño Peña" },
                    { new Guid("6e829b96-356b-4f13-89b9-bf072f541e46"), "Edison Ramirez Salinas" },
                    { new Guid("6ef9b6ec-063e-40ea-8e9a-79c9227c1bc0"), "Leonardo Rojas Rendón" },
                    { new Guid("71a4b04c-264d-4035-90ba-e7f7a6288f41"), "Mauricio Gutierrez Benavides" },
                    { new Guid("770f30a7-37b5-4a3a-9128-47c440aa0b73"), "Kevis Madera Mejia" },
                    { new Guid("77c788ca-fe12-4985-8d20-ab7fb5f2f703"), "Javier Enrique Tavera Palencia" },
                    { new Guid("7d07bd3e-bd71-4d0f-8b1c-e601b5f0172d"), "Paola Andrea Granados Barrios" },
                    { new Guid("81587d2c-1a8a-4a39-b07d-f09013d539bd"), "Luis Alberto Leal Castellanos" },
                    { new Guid("873952a4-c8c5-44b6-bd36-09ac018902eb"), "Francia Licet Parga Alvarez" },
                    { new Guid("8fb98f73-9bf7-41d8-ad1e-311a1c40b1c9"), "Hugo Moreno Ramon" },
                    { new Guid("90d26be3-b396-4510-900e-97fb1cf52ee1"), "Mauricio Antonio Goyeneche" },
                    { new Guid("90ec029b-77af-46a7-a38b-ea74ae735f7e"), "Alberto Camargo Sanchez" },
                    { new Guid("994a4f56-a7a9-41d2-8c8d-148c81a7a220"), "Ana Erika Niño Porras" },
                    { new Guid("9c41b995-490b-47f6-8d83-cda9f7af4c52"), "Edgar Javier Saucedo Davila" },
                    { new Guid("9decd84d-b130-4e4e-b097-ead56206e6e0"), "Roosemberg Cardenas Becerra" },
                    { new Guid("9e546a20-0121-4e32-906c-f8a0634495ca"), "Nestor Leonel Serrano Losada" },
                    { new Guid("9fefef3e-c3b1-47c4-bfb0-dcece836cf3c"), "Edgar Eduardo Durán Pinzón" },
                    { new Guid("a078a228-0379-41ee-867b-5809af0af67c"), "Olga Lucia Rodriguez Vargas" },
                    { new Guid("a5ba9221-3e32-4862-904e-60a1fdc2c4f1"), "Ruben Dario Velasquez Ruiz" },
                    { new Guid("b0d5b835-bca2-4522-990d-184b68f6bd23"), "Ramiro José Arenas Vargas" },
                    { new Guid("b758e67b-89af-4c13-a067-a1294134b35f"), "Cesar Augusto Aragón Castañeda" },
                    { new Guid("b9825b5b-1d70-4fd8-b5f5-d92916aca519"), "Edgar Hernán Antonio Castellanos" },
                    { new Guid("bc2d4150-f9b9-4811-9432-86424a383eb9"), "Carlos Daniel Martinez Dominguez" },
                    { new Guid("beb4d876-0a78-4a91-a8ce-0a4f11264e76"), "Liliana Paola Alvarez" },
                    { new Guid("c26cd87c-a139-4bb6-a96c-94ad9a4302e6"), "Milton Enrique Mejia Upegui" },
                    { new Guid("c7ec5b9e-a991-4ec2-9fe8-814728783c7a"), "Javier Hernandez Espinel" },
                    { new Guid("c910bde1-a668-4da2-a23f-28771734ca91"), "Oscar Eduardo Espinosa Lopez" },
                    { new Guid("e2a82b3c-3e03-4e4a-bcbf-9ec7772ae552"), "Fredy Alexander Cardenas Gonzalez" },
                    { new Guid("e737e669-e15d-4edd-b27b-a920644a8b77"), "Martin Quintero Montano" },
                    { new Guid("e7e40b0e-5607-430e-9840-902d0e78c9bc"), "Gabriel Antuan Sierra Alvarez" },
                    { new Guid("e8deabee-5757-464e-a371-8127980f5f54"), "Jose Reinaldo Villalba Peña" },
                    { new Guid("ec8210c2-7e06-43ec-bb41-fb17c25cedcf"), "Jesus Guillermo Zambrano Fernandez" },
                    { new Guid("f13e9e26-35a2-4b73-a9dd-3cb129896db3"), "Diego Alonso Reyes Urrea" },
                    { new Guid("f2cde89c-02bd-41cf-bb54-ba59f7f0dffc"), "Jenny Maritza Ospina Suarez" },
                    { new Guid("f2dfd59f-3262-4027-a276-9eb6ecde340e"), "Alvaro David Jimenez Castro" },
                    { new Guid("f8c26479-c3d7-4908-8fc0-ef50776a5e46"), "Francisco Antonio Muñoz Lozada" },
                    { new Guid("fea11a07-2665-4c87-a480-3b6d706af1b9"), "Francisco Tinjaca Bermudez" }
                });

            migrationBuilder.InsertData(
                table: "RiskTreatment",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), "Transferir" },
                    { new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), "Mitigar" },
                    { new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), "Aceptar" }
                });

            migrationBuilder.InsertData(
                table: "Risks",
                columns: new[] { "Id", "CategoryId", "Code", "CreatedAt", "DateFrom", "DateTo", "Manageability", "Name", "OwnerId", "Phase", "State", "TreatmentId", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("01d05468-617e-45f7-9544-7c4af1477790"), new Guid("cd821c53-efd4-465c-939c-5acc03b1f77f"), 24, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(2560), new DateOnly(2024, 10, 26), new DateOnly(2025, 9, 12), 3, "Demora autorización uso presupuesto", new Guid("f2dfd59f-3262-4027-a276-9eb6ecde340e"), 4, false, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(2560) },
                    { new Guid("045825fd-cbfe-477c-960a-75c773d79053"), new Guid("cd821c53-efd4-465c-939c-5acc03b1f77f"), 42, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(5190), new DateOnly(2026, 6, 19), new DateOnly(2026, 6, 22), 1, "Impactos de la Operación del DEMAG", new Guid("f2dfd59f-3262-4027-a276-9eb6ecde340e"), 3, false, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(5190) },
                    { new Guid("048b4c10-6d8f-48fb-b0b5-2d21df36f9fe"), new Guid("d1144533-b4f8-455b-bc82-54ef7bb820f2"), 6, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(4760), new DateOnly(1984, 10, 10), new DateOnly(1985, 1, 14), 1, "No disponibilidad de materiales", new Guid("118913f3-63bb-42b4-be97-03085d91bd3f"), 2, true, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(4760) },
                    { new Guid("058af583-95a9-478a-becf-d693672cc4a5"), new Guid("939aed64-27b1-4d45-9803-a88ba4ccac7a"), 33, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(1990), new DateOnly(1983, 7, 8), new DateOnly(1983, 7, 27), 2, "Adecuación sistema contraincendio", new Guid("770f30a7-37b5-4a3a-9128-47c440aa0b73"), 1, false, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(1990) },
                    { new Guid("09ba730a-0ba3-4982-9dd3-2f8787c04968"), new Guid("cc7ef7a0-2f09-4a54-8527-87d0de4425a9"), 55, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(3950), new DateOnly(2009, 3, 4), new DateOnly(2010, 3, 6), 3, "Cambios en las tarifas del Rig.", new Guid("1bc586e4-df03-4d96-9496-06820afdb285"), 2, false, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(3950) },
                    { new Guid("0c7e894b-f04d-438d-ad43-603c9225f28b"), new Guid("939aed64-27b1-4d45-9803-a88ba4ccac7a"), 17, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(1560), new DateOnly(2023, 6, 28), new DateOnly(2024, 5, 31), 2, "No disponibilidad de recursos", new Guid("e737e669-e15d-4edd-b27b-a920644a8b77"), 4, true, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(1560) },
                    { new Guid("0c9cf47d-639f-4102-a499-3b5a6b068e3f"), new Guid("d2c1621f-2efa-4c6e-a614-fc20f67a8fda"), 59, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(1280), new DateOnly(2015, 5, 17), new DateOnly(2016, 3, 2), 3, "Retrasos en la ejecución por paros", new Guid("9fefef3e-c3b1-47c4-bfb0-dcece836cf3c"), 1, true, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(1280) },
                    { new Guid("106549e8-2970-4055-a6af-2b09769dcda3"), new Guid("939aed64-27b1-4d45-9803-a88ba4ccac7a"), 50, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(8540), new DateOnly(2007, 11, 21), new DateOnly(2008, 7, 27), 1, "Dificultad en el acceso a las áreas", new Guid("9e546a20-0121-4e32-906c-f8a0634495ca"), 3, true, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(8540) },
                    { new Guid("13be1f58-9a76-452d-818d-ba2c5718796e"), new Guid("5034e84f-75f0-40e6-a912-184e68f210dd"), 16, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(5080), new DateOnly(1996, 10, 17), new DateOnly(1997, 3, 29), 1, "C12 Arqueología preventiva", new Guid("66865fab-e455-4521-baa1-5a54e03746aa"), 1, false, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(5080) },
                    { new Guid("1518aaaa-7c47-4a3e-95fb-636cd3af0ba7"), new Guid("cc7ef7a0-2f09-4a54-8527-87d0de4425a9"), 30, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(2920), new DateOnly(1984, 4, 29), new DateOnly(1984, 6, 15), 1, "Inicio de contratos", new Guid("118913f3-63bb-42b4-be97-03085d91bd3f"), 3, false, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(2920) },
                    { new Guid("17ba623c-f829-4ed6-9175-fdbacc887266"), new Guid("cd821c53-efd4-465c-939c-5acc03b1f77f"), 18, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(7130), new DateOnly(2033, 9, 9), new DateOnly(2033, 10, 26), 3, "Retrasos en entrega de Construcción", new Guid("1bc586e4-df03-4d96-9496-06820afdb285"), 4, true, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(7130) },
                    { new Guid("1dbee896-c4ed-44c4-bb17-1e088637d1cb"), new Guid("d1144533-b4f8-455b-bc82-54ef7bb820f2"), 26, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(5780), new DateOnly(2012, 7, 21), new DateOnly(2013, 8, 2), 1, "Paros, bloqueos y sabotajes", new Guid("b758e67b-89af-4c13-a067-a1294134b35f"), 4, true, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(5790) },
                    { new Guid("224b5103-1d99-46bb-9ca8-b5294b37226f"), new Guid("d1144533-b4f8-455b-bc82-54ef7bb820f2"), 26, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(3380), new DateOnly(2002, 3, 6), new DateOnly(2002, 11, 16), 2, "Cambio estimados F3 (WDP F2)", new Guid("9e546a20-0121-4e32-906c-f8a0634495ca"), 3, false, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(3380) },
                    { new Guid("2340b72a-4f5d-4f11-be2e-c3c9e47007aa"), new Guid("939aed64-27b1-4d45-9803-a88ba4ccac7a"), 49, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(9120), new DateOnly(2007, 10, 6), new DateOnly(2008, 10, 20), 3, "No disponibilidad de materiales", new Guid("8fb98f73-9bf7-41d8-ad1e-311a1c40b1c9"), 3, false, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(9120) },
                    { new Guid("2887ae74-2665-4d56-b2ff-49942113ba64"), new Guid("cc7ef7a0-2f09-4a54-8527-87d0de4425a9"), 69, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(4200), new DateOnly(1990, 12, 7), new DateOnly(1991, 10, 7), 3, "Daños a instalaciones operativas", new Guid("118913f3-63bb-42b4-be97-03085d91bd3f"), 1, true, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(4200) },
                    { new Guid("2a215a34-96eb-47df-876a-937fa68c5c3f"), new Guid("cd821c53-efd4-465c-939c-5acc03b1f77f"), 18, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(4780), new DateOnly(2029, 3, 20), new DateOnly(2029, 3, 23), 3, "Retrasos campaña de perforación", new Guid("5ef74348-1c30-445e-bdf0-82c60837b448"), 3, false, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(4780) },
                    { new Guid("2a3af155-488a-48f2-b134-e80e13b20fee"), new Guid("c366fcbc-bd3f-4fdf-98c0-ad4c0720b378"), 67, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(3060), new DateOnly(2003, 6, 24), new DateOnly(2004, 4, 18), 2, "PR1 Equipo que no se amolden a vias", new Guid("8fb98f73-9bf7-41d8-ad1e-311a1c40b1c9"), 2, false, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(3060) },
                    { new Guid("2b13240a-203e-4405-9024-1287b0948f84"), new Guid("d1144533-b4f8-455b-bc82-54ef7bb820f2"), 59, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(8400), new DateOnly(1985, 4, 3), new DateOnly(1986, 3, 21), 1, "Retrasos en entrega de Construcción de los clusters", new Guid("e737e669-e15d-4edd-b27b-a920644a8b77"), 2, true, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(8400) },
                    { new Guid("2f2dff56-fe40-4c21-a200-8a6a8a6a452c"), new Guid("e6211a79-a7b5-42cf-ad8d-810b7c44db33"), 42, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(3520), new DateOnly(2007, 8, 1), new DateOnly(2008, 7, 16), 2, "Disponibilidad de recursos humanos", new Guid("166c9a11-bee9-4983-968d-f9508fa1ba3b"), 4, true, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(3520) },
                    { new Guid("32590c3f-cabb-4787-9e03-9972739bf01d"), new Guid("3deb0dab-f2fe-4df2-a48f-7af824c60592"), 66, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(3870), new DateOnly(1996, 3, 30), new DateOnly(1996, 6, 20), 3, "Daños a instalaciones operativas", new Guid("c7ec5b9e-a991-4ec2-9fe8-814728783c7a"), 1, false, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(3870) },
                    { new Guid("33479861-c6e1-4908-a491-2e08d59be3bf"), new Guid("5034e84f-75f0-40e6-a912-184e68f210dd"), 11, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(9980), new DateOnly(1989, 4, 5), new DateOnly(1989, 12, 14), 1, "Hallazgos Arqueológicos", new Guid("6ef9b6ec-063e-40ea-8e9a-79c9227c1bc0"), 4, true, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(9980) },
                    { new Guid("3372fa5f-9479-43a3-ae6c-98a67f3d8a0a"), new Guid("d1144533-b4f8-455b-bc82-54ef7bb820f2"), 68, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(6660), new DateOnly(2002, 8, 14), new DateOnly(2003, 3, 10), 3, "Demoras en WDP F2", new Guid("5b17118c-c980-4606-b3ee-09c4ae143724"), 3, true, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(6660) },
                    { new Guid("384bc2d3-d57c-4625-b35d-230daff7d476"), new Guid("939aed64-27b1-4d45-9803-a88ba4ccac7a"), 15, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(3450), new DateOnly(1993, 7, 20), new DateOnly(1994, 2, 10), 1, "Paros, bloqueos y sabotajes", new Guid("166c9a11-bee9-4983-968d-f9508fa1ba3b"), 3, false, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(3450) },
                    { new Guid("38f0d173-62be-464e-9e0d-476f4532991b"), new Guid("d1144533-b4f8-455b-bc82-54ef7bb820f2"), 46, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(2480), new DateOnly(2018, 4, 20), new DateOnly(2018, 11, 23), 2, "EC3 Inicio de contratos", new Guid("f13e9e26-35a2-4b73-a9dd-3cb129896db3"), 4, false, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(2480) },
                    { new Guid("38f2be36-7e39-4d30-a294-b2854f74c321"), new Guid("5034e84f-75f0-40e6-a912-184e68f210dd"), 51, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(7970), new DateOnly(1982, 12, 8), new DateOnly(1983, 7, 1), 2, "Stand by de equipos", new Guid("1bc586e4-df03-4d96-9496-06820afdb285"), 2, true, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(7970) },
                    { new Guid("3aaf03ff-956a-4128-bd85-de493c287d00"), new Guid("8a0a6aed-631b-4009-bb97-23e072668d84"), 43, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(4930), new DateOnly(1979, 5, 3), new DateOnly(1980, 2, 2), 3, "Entrega oficial de las facilidades a la operación fuera de la fecha requerida", new Guid("77c788ca-fe12-4985-8d20-ab7fb5f2f703"), 2, false, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(4930) },
                    { new Guid("3b62d744-5580-4739-aabc-2692f1b19045"), new Guid("939aed64-27b1-4d45-9803-a88ba4ccac7a"), 43, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(5470), new DateOnly(1990, 7, 17), new DateOnly(1991, 3, 8), 1, "Interrupción de obra", new Guid("770f30a7-37b5-4a3a-9128-47c440aa0b73"), 3, true, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(5470) },
                    { new Guid("3be45bfd-75d1-455e-ac05-e6e6a23dcf16"), new Guid("8a0a6aed-631b-4009-bb97-23e072668d84"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(7260), new DateOnly(1987, 7, 24), new DateOnly(1987, 9, 2), 2, "Demoras en Ingenieria de detalle", new Guid("7d07bd3e-bd71-4d0f-8b1c-e601b5f0172d"), 1, true, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(7260) },
                    { new Guid("40e310fe-82a4-487e-a233-9d9f5bb6ec53"), new Guid("d1144533-b4f8-455b-bc82-54ef7bb820f2"), 11, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(4480), new DateOnly(1981, 12, 14), new DateOnly(1982, 1, 8), 3, "C12 Fallas adm en contrato", new Guid("0ce0850f-db55-4005-b779-532a45f03103"), 3, false, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(4480) },
                    { new Guid("4724e85b-fb9b-4cba-94f4-579b795bec17"), new Guid("4f069308-68af-4b5d-ab8c-ed1103dcba47"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(1710), new DateOnly(1979, 9, 28), new DateOnly(1980, 7, 26), 3, "Entregables para AR", new Guid("1bc586e4-df03-4d96-9496-06820afdb285"), 4, false, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(1710) },
                    { new Guid("4923c055-f8d7-4564-8e4f-af04f43ce7cb"), new Guid("dc9f763b-8ce6-49a9-9261-a3ac7287501d"), 22, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(6780), new DateOnly(2009, 8, 4), new DateOnly(2010, 4, 16), 3, "Incertidumbre en calidad materiales", new Guid("118913f3-63bb-42b4-be97-03085d91bd3f"), 1, false, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(6780) },
                    { new Guid("4bd7de87-56bd-46e7-96eb-43eb371bcf65"), new Guid("4c4c9430-0bcc-4d60-b409-d8c6109cbc8d"), 48, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(7690), new DateOnly(2005, 8, 30), new DateOnly(2006, 6, 23), 1, "Paros, Bloqueos y Sabotajes", new Guid("042fa692-ba22-490d-8aab-2d9795bb5dd3"), 3, true, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(7690) },
                    { new Guid("4d2b8a9b-0207-41a9-9f1e-a3fc80e69955"), new Guid("5034e84f-75f0-40e6-a912-184e68f210dd"), 60, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(3130), new DateOnly(1990, 1, 16), new DateOnly(1990, 10, 24), 1, "No alcanzar condiciones pta marcha", new Guid("77c788ca-fe12-4985-8d20-ab7fb5f2f703"), 2, false, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(3130) },
                    { new Guid("4e1b42fc-3a79-41e9-be8f-260412392f8c"), new Guid("939aed64-27b1-4d45-9803-a88ba4ccac7a"), 39, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(9700), new DateOnly(2013, 1, 25), new DateOnly(2013, 3, 23), 3, "No contar con separador para produ", new Guid("118913f3-63bb-42b4-be97-03085d91bd3f"), 3, false, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(9700) },
                    { new Guid("4f11addb-8940-4ab6-a130-456690f745ae"), new Guid("dc9f763b-8ce6-49a9-9261-a3ac7287501d"), 57, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(8120), new DateOnly(1979, 5, 16), new DateOnly(1979, 12, 29), 2, "Ejecutar actividades no planeadas", new Guid("beb4d876-0a78-4a91-a8ce-0a4f11264e76"), 2, false, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(8120) },
                    { new Guid("4f7e1da9-0474-4838-843c-e8aa13d29a3a"), new Guid("5034e84f-75f0-40e6-a912-184e68f210dd"), 10, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(4230), new DateOnly(2005, 1, 25), new DateOnly(2005, 2, 3), 2, "Inclumplimiento RETIE", new Guid("81587d2c-1a8a-4a39-b07d-f09013d539bd"), 2, true, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(4230) },
                    { new Guid("5afb9dd4-ce31-4130-b190-f2b7f6acaec9"), new Guid("5034e84f-75f0-40e6-a912-184e68f210dd"), 49, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(4510), new DateOnly(2033, 6, 2), new DateOnly(2033, 12, 14), 1, "Inconvenientes sociales", new Guid("29748b6c-3f6e-4fb8-a1d5-01264251610b"), 3, false, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(4510) },
                    { new Guid("5d5fdcac-12fd-462e-a9a9-63e367e3efff"), new Guid("cd821c53-efd4-465c-939c-5acc03b1f77f"), 36, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(5360), new DateOnly(1991, 11, 22), new DateOnly(1992, 8, 14), 2, "EC3 Licencias/permisos ambientales", new Guid("6bff6f50-feaf-4809-89fd-9f12216c1592"), 4, true, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(5360) },
                    { new Guid("6154f109-a17f-4d65-a9ea-dc53afe48952"), new Guid("8a0a6aed-631b-4009-bb97-23e072668d84"), 63, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(6980), new DateOnly(2028, 3, 2), new DateOnly(2029, 1, 1), 3, "No disponibilidad de materiales en tiempo y calidad (válvulas)", new Guid("8fb98f73-9bf7-41d8-ad1e-311a1c40b1c9"), 1, true, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(6980) },
                    { new Guid("6573c192-fae9-4d09-86db-b5ad9fbd730f"), new Guid("5034e84f-75f0-40e6-a912-184e68f210dd"), 22, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(280), new DateOnly(2008, 7, 4), new DateOnly(2008, 8, 19), 3, "Demora inicio compras", new Guid("42ccf3a4-c3b0-4bd7-84ec-df184bdcc8d9"), 3, false, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(280) },
                    { new Guid("693f42af-3af2-44be-b684-e2917f160786"), new Guid("8a0a6aed-631b-4009-bb97-23e072668d84"), 48, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(1860), new DateOnly(2022, 2, 10), new DateOnly(2023, 2, 26), 3, "Paros, bloqueos, sabotajes", new Guid("90d26be3-b396-4510-900e-97fb1cf52ee1"), 2, false, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(1860) },
                    { new Guid("6c896dbb-deaf-4d8b-8731-beb49ebaa906"), new Guid("d2c1621f-2efa-4c6e-a614-fc20f67a8fda"), 25, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(1430), new DateOnly(1980, 5, 7), new DateOnly(1980, 7, 5), 1, "Cancelación de pozos", new Guid("159fa26a-a787-4a66-b91d-f2fd01622fa3"), 3, false, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(1430) },
                    { new Guid("6de46174-0f66-4953-a6c7-33b860b83767"), new Guid("cc7ef7a0-2f09-4a54-8527-87d0de4425a9"), 61, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(7410), new DateOnly(2023, 10, 18), new DateOnly(2023, 10, 19), 3, "Mayores costos Ingeniería Básica", new Guid("c7ec5b9e-a991-4ec2-9fe8-814728783c7a"), 1, true, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(7410) },
                    { new Guid("6f1b1ea8-e985-4fc3-ba75-82f36b2c2c32"), new Guid("dc9f763b-8ce6-49a9-9261-a3ac7287501d"), 24, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(8980), new DateOnly(2021, 12, 20), new DateOnly(2022, 4, 19), 3, "Entrega tardía tanque", new Guid("9decd84d-b130-4e4e-b097-ead56206e6e0"), 3, false, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(8980) },
                    { new Guid("71c3466d-7f92-4db4-80f5-a01297f00889"), new Guid("8a0a6aed-631b-4009-bb97-23e072668d84"), 20, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(4020), new DateOnly(1986, 2, 21), new DateOnly(1987, 2, 4), 2, "C12 Hallazgo Arqueológico", new Guid("e2a82b3c-3e03-4e4a-bcbf-9ec7772ae552"), 2, false, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(4020) },
                    { new Guid("787d62ca-39fc-4f07-a298-dc3897fe6ffb"), new Guid("c366fcbc-bd3f-4fdf-98c0-ad4c0720b378"), 62, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(5760), new DateOnly(2032, 1, 3), new DateOnly(2032, 6, 29), 3, "Cambios ingeniería básica", new Guid("5ad7ef59-b58d-409c-845b-4e940cf227f1"), 2, false, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(5760) },
                    { new Guid("7c2cd3e1-06dc-4512-a5b5-fddf5877e16c"), new Guid("3deb0dab-f2fe-4df2-a48f-7af824c60592"), 37, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(4650), new DateOnly(1979, 11, 1), new DateOnly(1980, 10, 27), 2, "Entrega oficial de los cluster", new Guid("81587d2c-1a8a-4a39-b07d-f09013d539bd"), 1, true, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(4650) },
                    { new Guid("8070c457-76ce-4b0b-a925-28234677179e"), new Guid("4f069308-68af-4b5d-ab8c-ed1103dcba47"), 11, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(3590), new DateOnly(2002, 6, 5), new DateOnly(2003, 4, 5), 3, "No alcanzar condiciones pta marcha", new Guid("42ccf3a4-c3b0-4bd7-84ec-df184bdcc8d9"), 2, false, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(3590) },
                    { new Guid("8455b558-52c0-48ff-9927-06f24cfc3b75"), new Guid("d2c1621f-2efa-4c6e-a614-fc20f67a8fda"), 24, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(4080), new DateOnly(2012, 9, 18), new DateOnly(2013, 10, 6), 1, "Cambios de normativas sobre el plan de manejo ambiental del campo", new Guid("5f61fe86-dced-412b-b24c-2e07f52a22a1"), 2, true, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(4090) },
                    { new Guid("87ea0388-1897-47e4-a37d-fb061dc84f55"), new Guid("308b1695-1722-4773-8661-01336ecd1998"), 34, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(2630), new DateOnly(2012, 11, 5), new DateOnly(2013, 7, 6), 2, "EA1 Inicio de contratos", new Guid("770f30a7-37b5-4a3a-9128-47c440aa0b73"), 3, true, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(2630) },
                    { new Guid("8aed8cb9-2cb3-406b-a993-069395b07493"), new Guid("939aed64-27b1-4d45-9803-a88ba4ccac7a"), 10, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(6210), new DateOnly(2030, 11, 18), new DateOnly(2031, 8, 10), 3, "Licencia captación y disposición", new Guid("9e546a20-0121-4e32-906c-f8a0634495ca"), 3, false, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(6210) },
                    { new Guid("8e144e0a-de97-4e42-be87-08cd4b4d9f05"), new Guid("e6211a79-a7b5-42cf-ad8d-810b7c44db33"), 7, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(6350), new DateOnly(2019, 4, 16), new DateOnly(2019, 11, 10), 2, "Retraso prueba inyectividad", new Guid("c910bde1-a668-4da2-a23f-28771734ca91"), 1, true, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(6350) },
                    { new Guid("91e1664c-202d-41df-a3c7-9f9f61f3bb33"), new Guid("cd821c53-efd4-465c-939c-5acc03b1f77f"), 45, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(7540), new DateOnly(1983, 7, 13), new DateOnly(1984, 8, 7), 2, "Dilución responsabilidad", new Guid("b9825b5b-1d70-4fd8-b5f5-d92916aca519"), 3, true, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(7540) },
                    { new Guid("96849172-c7dc-4cb5-812e-5970cf0a9b8f"), new Guid("e6211a79-a7b5-42cf-ad8d-810b7c44db33"), 61, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(6320), new DateOnly(2016, 4, 11), new DateOnly(2016, 12, 11), 1, "Incumplimiento de las paradas", new Guid("ec8210c2-7e06-43ec-bb41-fb17c25cedcf"), 4, true, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(6320) },
                    { new Guid("991ef344-a008-4884-8e7a-3c8f4220a938"), new Guid("3deb0dab-f2fe-4df2-a48f-7af824c60592"), 8, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(9260), new DateOnly(2032, 8, 27), new DateOnly(2033, 8, 27), 3, "Fallas adm en contrato", new Guid("2d744abb-27db-4433-9110-30c0cfef9a98"), 3, false, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(9260) },
                    { new Guid("9b0255a9-35da-4434-ae67-e202537e2569"), new Guid("d2c1621f-2efa-4c6e-a614-fc20f67a8fda"), 57, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(5330), new DateOnly(1979, 6, 18), new DateOnly(1980, 1, 16), 2, "Interrupción de obra", new Guid("68be25c5-fbef-40d2-947b-afe5b5b5108e"), 4, true, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(5340) },
                    { new Guid("9e9c3b2f-fee7-465e-91a6-f98e6031aa16"), new Guid("3deb0dab-f2fe-4df2-a48f-7af824c60592"), 57, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(710), new DateOnly(2009, 4, 1), new DateOnly(2009, 12, 22), 3, "Rediseños de ingeniería", new Guid("29748b6c-3f6e-4fb8-a1d5-01264251610b"), 2, false, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(710) },
                    { new Guid("9f7307c7-dad0-4fa2-9d85-121b685ebddd"), new Guid("939aed64-27b1-4d45-9803-a88ba4ccac7a"), 49, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(5640), new DateOnly(2021, 4, 8), new DateOnly(2021, 11, 3), 3, "No tener disponibilidad energética", new Guid("f13e9e26-35a2-4b73-a9dd-3cb129896db3"), 1, false, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(5640) },
                    { new Guid("a1dcec6e-ed9a-4394-b5ad-cf280b0a1ab1"), new Guid("4c4c9430-0bcc-4d60-b409-d8c6109cbc8d"), 31, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(5210), new DateOnly(1998, 1, 22), new DateOnly(1999, 1, 18), 3, "EC3 Arqueología preventiva", new Guid("c26cd87c-a139-4bb6-a96c-94ad9a4302e6"), 4, false, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(5210) },
                    { new Guid("a5f7052e-5d61-4dcf-b0cc-7e8915d7bdc2"), new Guid("cd821c53-efd4-465c-939c-5acc03b1f77f"), 7, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(3280), new DateOnly(1997, 1, 6), new DateOnly(1998, 2, 7), 2, "Paros, bloqueos y sabotajes", new Guid("5ef74348-1c30-445e-bdf0-82c60837b448"), 1, true, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(3280) },
                    { new Guid("a712d9ed-dc44-49fa-8fd0-99eb608d48b0"), new Guid("4f069308-68af-4b5d-ab8c-ed1103dcba47"), 43, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(5920), new DateOnly(1998, 12, 13), new DateOnly(1999, 1, 30), 1, "Paros, bloqueos y sabotajes", new Guid("fea11a07-2665-4c87-a480-3b6d706af1b9"), 1, false, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(5920) },
                    { new Guid("a93e5960-331a-4109-b31e-35d9fc236574"), new Guid("8a0a6aed-631b-4009-bb97-23e072668d84"), 43, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(6190), new DateOnly(2016, 5, 16), new DateOnly(2017, 1, 15), 3, "No disponibilidad densitómetro", new Guid("a5ba9221-3e32-4862-904e-60a1fdc2c4f1"), 2, true, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(6190) },
                    { new Guid("a99c2e80-65da-43bd-b05d-a39d404eb387"), new Guid("cc7ef7a0-2f09-4a54-8527-87d0de4425a9"), 12, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(4910), new DateOnly(1989, 3, 3), new DateOnly(1989, 7, 9), 1, "EA1 No disponibilidad de materiales", new Guid("6ef9b6ec-063e-40ea-8e9a-79c9227c1bc0"), 3, true, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(4910) },
                    { new Guid("aaf79c3a-6c25-4594-a131-0ea1e851bcf1"), new Guid("3deb0dab-f2fe-4df2-a48f-7af824c60592"), 50, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(2840), new DateOnly(2018, 7, 5), new DateOnly(2019, 5, 4), 1, "Demoras por tramite Arqueológico", new Guid("6bff6f50-feaf-4809-89fd-9f12216c1592"), 4, false, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(2840) },
                    { new Guid("ab1439b0-c3af-487b-9515-cdec13485fe7"), new Guid("3deb0dab-f2fe-4df2-a48f-7af824c60592"), 47, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(7830), new DateOnly(1990, 12, 2), new DateOnly(1991, 1, 1), 1, "Afectación sistema eléctrico contro", new Guid("9fefef3e-c3b1-47c4-bfb0-dcece836cf3c"), 4, true, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(7830) },
                    { new Guid("ac84d9bc-0e61-4994-8a8f-91040993bb8b"), new Guid("939aed64-27b1-4d45-9803-a88ba4ccac7a"), 45, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(2770), new DateOnly(2007, 10, 23), new DateOnly(2008, 10, 11), 1, "Inicio de contratos", new Guid("994a4f56-a7a9-41d2-8c8d-148c81a7a220"), 1, false, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(2770) },
                    { new Guid("af6bf218-3527-4d80-9529-f07d410910f4"), new Guid("4c4c9430-0bcc-4d60-b409-d8c6109cbc8d"), 34, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(1130), new DateOnly(2006, 7, 9), new DateOnly(2006, 9, 5), 2, "Variación de precios", new Guid("770f30a7-37b5-4a3a-9128-47c440aa0b73"), 1, true, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(1130) },
                    { new Guid("b02ae497-055c-4fad-837d-825b6851de11"), new Guid("5034e84f-75f0-40e6-a912-184e68f210dd"), 24, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(9550), new DateOnly(2019, 6, 4), new DateOnly(2019, 7, 18), 1, "Inicio perforación", new Guid("5ad7ef59-b58d-409c-845b-4e940cf227f1"), 3, false, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(9550) },
                    { new Guid("b228e24d-a41e-4610-871b-a0593832353e"), new Guid("cd821c53-efd4-465c-939c-5acc03b1f77f"), 19, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(9420), new DateOnly(1985, 2, 19), new DateOnly(1985, 12, 3), 3, "Fallas en los Equipos a suministrar", new Guid("873952a4-c8c5-44b6-bd36-09ac018902eb"), 2, true, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(9420) },
                    { new Guid("b34ffdd7-7918-4a48-baf3-0177dea9af1f"), new Guid("cd821c53-efd4-465c-939c-5acc03b1f77f"), 32, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(2270), new DateOnly(1998, 6, 18), new DateOnly(1999, 3, 18), 2, "No contar con proveedores", new Guid("9decd84d-b130-4e4e-b097-ead56206e6e0"), 1, true, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(2270) },
                    { new Guid("b3d22775-8cfc-4448-9e67-0eed7a56f6c4"), new Guid("4c4c9430-0bcc-4d60-b409-d8c6109cbc8d"), 30, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(4630), new DateOnly(2011, 6, 18), new DateOnly(2012, 5, 8), 1, "Retrasos instalación medición", new Guid("9decd84d-b130-4e4e-b097-ead56206e6e0"), 2, false, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(4630) },
                    { new Guid("b5cef553-8ed5-4d1d-ac49-fb010a494a95"), new Guid("4c4c9430-0bcc-4d60-b409-d8c6109cbc8d"), 65, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(3650), new DateOnly(1988, 12, 3), new DateOnly(1989, 11, 6), 1, "Retraso entrega de Prognosis", new Guid("42ccf3a4-c3b0-4bd7-84ec-df184bdcc8d9"), 1, false, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(3650) },
                    { new Guid("b849c9b3-a3c9-45ac-9a9c-b6f44d1ed855"), new Guid("dc9f763b-8ce6-49a9-9261-a3ac7287501d"), 24, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(9850), new DateOnly(2021, 12, 26), new DateOnly(2022, 1, 29), 1, "Demoras radicación PS polímero", new Guid("6bff6f50-feaf-4809-89fd-9f12216c1592"), 4, true, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(9850) },
                    { new Guid("b8e67a22-cf1e-428d-84b2-37b50b703958"), new Guid("5034e84f-75f0-40e6-a912-184e68f210dd"), 26, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(3000), new DateOnly(1983, 1, 9), new DateOnly(1983, 8, 15), 1, "No disponibilidad líneas de flujo", new Guid("e8deabee-5757-464e-a371-8127980f5f54"), 2, false, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(3000) },
                    { new Guid("bdb735f1-b98f-4601-8730-a5ad4ecd5da2"), new Guid("5034e84f-75f0-40e6-a912-184e68f210dd"), 43, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(850), new DateOnly(1979, 11, 30), new DateOnly(1980, 1, 28), 1, "No contar con material perforación", new Guid("e7e40b0e-5607-430e-9840-902d0e78c9bc"), 3, false, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(850) },
                    { new Guid("bf6df839-ce64-4f91-8dd5-8151737b72bf"), new Guid("dc9f763b-8ce6-49a9-9261-a3ac7287501d"), 29, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(6650), new DateOnly(2033, 5, 25), new DateOnly(2033, 12, 16), 1, "Fugas o Fallas sistema tubería", new Guid("50285870-e1e2-41c4-adeb-0916308818a7"), 4, true, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(6650) },
                    { new Guid("c0a3819d-8788-425c-8074-538614ff3489"), new Guid("dc9f763b-8ce6-49a9-9261-a3ac7287501d"), 28, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(4360), new DateOnly(2012, 11, 2), new DateOnly(2013, 11, 3), 3, "Restricción operacional de pozos", new Guid("e737e669-e15d-4edd-b27b-a920644a8b77"), 2, false, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(4360) },
                    { new Guid("c10eccbf-f61d-45e3-a37e-7920cbb9a44e"), new Guid("8a0a6aed-631b-4009-bb97-23e072668d84"), 34, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(570), new DateOnly(1999, 6, 20), new DateOnly(2000, 2, 17), 1, "Disponibilidad Recursos ejecución", new Guid("f13e9e26-35a2-4b73-a9dd-3cb129896db3"), 1, false, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(570) },
                    { new Guid("c1595368-ad34-41aa-bf70-d79ffe12c89c"), new Guid("4c4c9430-0bcc-4d60-b409-d8c6109cbc8d"), 42, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(8710), new DateOnly(1997, 10, 3), new DateOnly(1998, 2, 1), 2, "No contar con separador para produ", new Guid("90d26be3-b396-4510-900e-97fb1cf52ee1"), 3, true, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(8710) },
                    { new Guid("c2ceaf8f-492a-4190-a7cc-0b1f49329cd1"), new Guid("3deb0dab-f2fe-4df2-a48f-7af824c60592"), 56, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(5620), new DateOnly(2013, 6, 29), new DateOnly(2013, 7, 30), 1, "Hallazgos arqueológicos.", new Guid("2d744abb-27db-4433-9110-30c0cfef9a98"), 2, true, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(5620) },
                    { new Guid("c57eaac5-a657-40a5-bdc4-57f6e389e677"), new Guid("939aed64-27b1-4d45-9803-a88ba4ccac7a"), 23, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(6800), new DateOnly(2017, 12, 28), new DateOnly(2018, 5, 7), 3, "LI1 No oportunidad en Prognosis", new Guid("6ef9b6ec-063e-40ea-8e9a-79c9227c1bc0"), 4, true, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(6800) },
                    { new Guid("c8c53c46-59d2-40c9-a0d0-6fb4ee23af35"), new Guid("4c4c9430-0bcc-4d60-b409-d8c6109cbc8d"), 67, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(8260), new DateOnly(1992, 6, 27), new DateOnly(1992, 9, 16), 2, "Afectación reputación Ecopetrol", new Guid("c7ec5b9e-a991-4ec2-9fe8-814728783c7a"), 1, false, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(8260) },
                    { new Guid("cac6e56e-4dab-4aef-9a7a-c63c324250ed"), new Guid("d2c1621f-2efa-4c6e-a614-fc20f67a8fda"), 47, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(5060), new DateOnly(1991, 9, 20), new DateOnly(1991, 12, 1), 2, "Retrasos instalación medición", new Guid("e7e40b0e-5607-430e-9840-902d0e78c9bc"), 4, true, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(5060) },
                    { new Guid("cd882d75-95b9-4ea6-a041-38dd541578fb"), new Guid("3deb0dab-f2fe-4df2-a48f-7af824c60592"), 45, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(4330), new DateOnly(1980, 9, 6), new DateOnly(1980, 11, 8), 1, "EA1 Fallas adm en contrato", new Guid("9c41b995-490b-47f6-8d83-cda9f7af4c52"), 3, false, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(4330) },
                    { new Guid("cf49476f-471e-442a-8da4-1dda3a819487"), new Guid("dc9f763b-8ce6-49a9-9261-a3ac7287501d"), 7, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(6470), new DateOnly(2006, 5, 19), new DateOnly(2006, 9, 24), 1, "No perforar 3 pozos contingentes", new Guid("b9825b5b-1d70-4fd8-b5f5-d92916aca519"), 4, false, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(6470) },
                    { new Guid("d11441ac-23b8-4fdc-a680-46b170c08675"), new Guid("d2c1621f-2efa-4c6e-a614-fc20f67a8fda"), 21, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(1000), new DateOnly(2000, 7, 12), new DateOnly(2000, 10, 23), 1, "Limitacion de la capacidad", new Guid("5ef74348-1c30-445e-bdf0-82c60837b448"), 3, false, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(1000) },
                    { new Guid("d4dbe123-9848-462c-8ae8-514128bf8040"), new Guid("308b1695-1722-4773-8661-01336ecd1998"), 51, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(2700), new DateOnly(2010, 4, 22), new DateOnly(2011, 1, 20), 2, "Modificación de alcance", new Guid("beb4d876-0a78-4a91-a8ce-0a4f11264e76"), 2, true, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(2700) },
                    { new Guid("da40d4fa-2bd3-4d36-846a-8da26a6a1141"), new Guid("3deb0dab-f2fe-4df2-a48f-7af824c60592"), 28, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(6070), new DateOnly(2025, 3, 27), new DateOnly(2026, 4, 12), 2, "Cambios Básica-detalle", new Guid("c910bde1-a668-4da2-a23f-28771734ca91"), 4, true, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(6070) },
                    { new Guid("dbbd4093-7fb7-4fa0-b997-25a2fa7e6fd0"), new Guid("8a0a6aed-631b-4009-bb97-23e072668d84"), 49, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(3740), new DateOnly(1980, 5, 18), new DateOnly(1980, 9, 10), 2, "Dem. firma acta inicio Cont Termic", new Guid("159fa26a-a787-4a66-b91d-f2fd01622fa3"), 3, true, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(3740) },
                    { new Guid("e56414f6-3d9a-4dc1-b263-33431b2fa718"), new Guid("4f069308-68af-4b5d-ab8c-ed1103dcba47"), 41, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(5890), new DateOnly(2018, 5, 31), new DateOnly(2018, 11, 6), 3, "Suscripción de OT de ingeneiría", new Guid("1df4b57d-96a3-43ca-bcb7-8d5b1d221be3"), 3, true, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(5890) },
                    { new Guid("eaf5c2b3-22d7-4337-a9ad-ef38bfc648a1"), new Guid("e6211a79-a7b5-42cf-ad8d-810b7c44db33"), 26, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(130), new DateOnly(2024, 12, 25), new DateOnly(2025, 6, 28), 1, "Demoras en Proceso de Selección de proveedor de polímero entrecruzado.", new Guid("beb4d876-0a78-4a91-a8ce-0a4f11264e76"), 1, true, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(130) },
                    { new Guid("eff8fc2f-d201-48ff-baca-3220c61c667b"), new Guid("3deb0dab-f2fe-4df2-a48f-7af824c60592"), 35, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(8840), new DateOnly(1989, 10, 24), new DateOnly(1990, 10, 16), 3, "Demora a respuesta TQ`s", new Guid("42ccf3a4-c3b0-4bd7-84ec-df184bdcc8d9"), 1, false, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(8840) },
                    { new Guid("f366a289-ee2e-40c3-9f1b-4281da34e480"), new Guid("8a0a6aed-631b-4009-bb97-23e072668d84"), 39, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(5510), new DateOnly(2009, 4, 14), new DateOnly(2010, 2, 11), 3, "Retraso prueba inyectividad", new Guid("5ef74348-1c30-445e-bdf0-82c60837b448"), 4, false, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(5510) },
                    { new Guid("f584fd3a-5cf2-4ff3-9a4b-6f04c39f76cf"), new Guid("4f069308-68af-4b5d-ab8c-ed1103dcba47"), 23, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(410), new DateOnly(2028, 6, 29), new DateOnly(2028, 11, 21), 3, "Modif contractual de contratos", new Guid("68be25c5-fbef-40d2-947b-afe5b5b5108e"), 4, false, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(410) },
                    { new Guid("f5c66139-2c28-43c9-9a06-6cf082157aa0"), new Guid("5034e84f-75f0-40e6-a912-184e68f210dd"), 24, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(2420), new DateOnly(2015, 7, 17), new DateOnly(2016, 4, 9), 1, "Demoras por tramite Arqueológico", new Guid("90d26be3-b396-4510-900e-97fb1cf52ee1"), 1, true, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(2420) },
                    { new Guid("f9589ccf-3f57-4dfe-ab9b-d13f65a97cd7"), new Guid("8a0a6aed-631b-4009-bb97-23e072668d84"), 38, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(6040), new DateOnly(1983, 9, 20), new DateOnly(1983, 10, 21), 2, "Hallazgos arqueológicos.", new Guid("6e829b96-356b-4f13-89b9-bf072f541e46"), 2, true, new Guid("f0e6cd1e-cd39-44f3-929b-2e036802e231"), 2, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(6040) },
                    { new Guid("fa0a470b-b2c6-4b56-aeec-2737fa38cec1"), new Guid("8a0a6aed-631b-4009-bb97-23e072668d84"), 30, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(3200), new DateOnly(1992, 8, 19), new DateOnly(1993, 1, 24), 3, "PR1 Integridad pozos Workover", new Guid("1bc586e4-df03-4d96-9496-06820afdb285"), 3, true, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(3200) },
                    { new Guid("fa17e65d-c8eb-4f6c-9ab3-06c920df0c12"), new Guid("939aed64-27b1-4d45-9803-a88ba4ccac7a"), 57, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(2140), new DateOnly(2009, 4, 5), new DateOnly(2010, 4, 29), 1, "Modific. Contractuales Opt. Term", new Guid("5ad7ef59-b58d-409c-845b-4e940cf227f1"), 3, true, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 524, DateTimeKind.Utc).AddTicks(2140) },
                    { new Guid("fa40760f-4400-41c0-801d-21b3e32320c1"), new Guid("d1144533-b4f8-455b-bc82-54ef7bb820f2"), 16, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(6500), new DateOnly(2024, 4, 20), new DateOnly(2024, 10, 21), 2, "Cambios Básica-detalle", new Guid("770f30a7-37b5-4a3a-9128-47c440aa0b73"), 2, true, new Guid("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(6500) },
                    { new Guid("fee043bc-519d-451e-8c09-c032654aa82d"), new Guid("3deb0dab-f2fe-4df2-a48f-7af824c60592"), 27, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(3800), new DateOnly(2015, 12, 28), new DateOnly(2016, 4, 19), 1, "Ingeniería básica", new Guid("a078a228-0379-41ee-867b-5809af0af67c"), 4, true, new Guid("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), 1, new DateTime(2023, 9, 10, 3, 50, 58, 523, DateTimeKind.Utc).AddTicks(3800) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Risks_CategoryId",
                table: "Risks",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Risks_OwnerId",
                table: "Risks",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Risks_TreatmentId",
                table: "Risks",
                column: "TreatmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Risks");

            migrationBuilder.DropTable(
                name: "RiskCategory");

            migrationBuilder.DropTable(
                name: "RiskOwner");

            migrationBuilder.DropTable(
                name: "RiskTreatment");
        }
    }
}
