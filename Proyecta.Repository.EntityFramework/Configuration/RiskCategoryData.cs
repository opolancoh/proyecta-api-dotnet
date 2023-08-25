using Proyecta.Core.Entities;

namespace Proyecta.Repository.EntityFramework.Configuration;

public static class RiskCategoryData
{
    public static List<RiskCategory> Data =>
        new()
        {
            new()
            {
                Id = Guid.Parse("308b1695-1722-4773-8661-01336ecd1998"),
                Name = "HSE y seguridad física"
            },
            new()
            {
                Id = Guid.Parse("3deb0dab-f2fe-4df2-a48f-7af824c60592"),
                Name = "Arqueología"
            },
            new()
            {
                Id = Guid.Parse("4c4c9430-0bcc-4d60-b409-d8c6109cbc8d"),
                Name = "Comisionamiento y arranque"
            },
            new()
            {
                Id = Guid.Parse("4f069308-68af-4b5d-ab8c-ed1103dcba47"),
                Name = "Ambiental"
            },
            new()
            {
                Id = Guid.Parse("5034e84f-75f0-40e6-a912-184e68f210dd"),
                Name = "Abastecimiento"
            },
            new()
            {
                Id = Guid.Parse("8a0a6aed-631b-4009-bb97-23e072668d84"),
                Name = "Ingeniería"
            },
            new()
            {
                Id = Guid.Parse("939aed64-27b1-4d45-9803-a88ba4ccac7a"),
                Name = "Perforación, completamiento y workover"
            },
            new()
            {
                Id = Guid.Parse("c366fcbc-bd3f-4fdf-98c0-ad4c0720b378"),
                Name = "Entorno"
            },
            new()
            {
                Id = Guid.Parse("cc7ef7a0-2f09-4a54-8527-87d0de4425a9"),
                Name = "Yacimientos"
            },
            new()
            {
                Id = Guid.Parse("cd821c53-efd4-465c-939c-5acc03b1f77f"),
                Name = "Gerenciamiento del proyecto"
            },
            new()
            {
                Id = Guid.Parse("d1144533-b4f8-455b-bc82-54ef7bb820f2"),
                Name = "Montaje y construcción"
            },
            new()
            {
                Id = Guid.Parse("d2c1621f-2efa-4c6e-a614-fc20f67a8fda"),
                Name = "Legislativo, normativo y/o tributario"
            },
            new()
            {
                Id = Guid.Parse("dc9f763b-8ce6-49a9-9261-a3ac7287501d"),
                Name = "Inmobiliario"
            },
            new()
            {
                Id = Guid.Parse("e6211a79-a7b5-42cf-ad8d-810b7c44db33"),
                Name = "Calidad y materiales"
            }
        };
}