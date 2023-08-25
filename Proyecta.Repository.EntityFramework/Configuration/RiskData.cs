using Proyecta.Core.Entities;

namespace Proyecta.Repository.EntityFramework.Configuration;

public static class RiskData
{
    public static List<Risk> GetData()
    {
        var riskNames = GetRiskNames();

        return (from name in riskNames
            let dateFrom = GetRandomDateOnly(1979, 2033)
            let dateTo = dateFrom.AddDays(GetRandomNumber(1, 400))
            select new Risk
            {
                Id = Guid.NewGuid(),
                Name = name,
                Code = GetRandomNumber(1, 69),
                CategoryId = RiskCategoryData.Data[GetRandomNumber(0, RiskCategoryData.Data.Count - 1)].Id,
                Type = GetRandomEnumValue<RiskType>(),
                OwnerId = RiskOwnerData.Data[GetRandomNumber(0, RiskOwnerData.Data.Count - 1)].Id,
                Phase = GetRandomEnumValue<RiskPhase>(),
                Manageability = GetRandomEnumValue<RiskManageability>(),
                TreatmentId = RiskTreatmentData.Data[GetRandomNumber(0, RiskTreatmentData.Data.Count - 1)].Id,
                DateFrom = dateFrom,
                DateTo = dateTo,
                State = GetRandomBoolean(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }).ToList();
    }

    private static DateOnly GetRandomDateOnly(int minYear, int maxYear)
    {
        var random = new Random();
        var startDate = new DateOnly(minYear, 1, 1);
        var endDate = new DateOnly(maxYear, 12, 31);

        var startDayNumber = startDate.DayNumber;
        var endDayNumber = endDate.DayNumber;

        var randomDayNumber = random.Next(startDayNumber, endDayNumber + 1);

        return DateOnly.FromDayNumber(randomDayNumber);
    }

    private static int GetRandomNumber(int min, int max)
    {
        Random random = new Random();
        return random.Next(min, max + 1); // Generates a random number between min and max
    }

    // Generic method to get a random enum value
    static T GetRandomEnumValue<T>() where T : Enum
    {
        var random = new Random();
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(random.Next(values.Length));
    }

    static bool GetRandomBoolean()
    {
        Random random = new Random();
        int randomNumber = random.Next(0, 2); // Generates a random number: 0 or 1
        return randomNumber == 1;
    }

    private static List<string> GetRiskNames()
    {
        return new List<string>
        {
            "EC3 Inicio de contratos",
            "EA1 Inicio de contratos",
            "Inicio de contratos",
            "Inicio de contratos",
            "PR1 Equipo que no se amolden a vias",
            "PR1 Integridad pozos Workover",
            "Cambio estimados F3 (WDP F2)",
            "Disponibilidad de recursos humanos",
            "Retraso entrega de Prognosis",
            "Ingeniería básica",
            "Cambios en las tarifas del Rig.",
            "Cambios de normativas sobre el plan de manejo ambiental del campo",
            "Inclumplimiento RETIE",
            "Restricción operacional de pozos",
            "Inconvenientes sociales",
            "Entrega oficial de los cluster",
            "Retrasos campaña de perforación",
            "Entrega oficial de las facilidades a la operación fuera de la fecha requerida",
            "C12 Arqueología preventiva",
            "EC3 Arqueología preventiva",
            "EC3 Licencias/permisos ambientales",
            "Retraso prueba inyectividad",
            "No tener disponibilidad energética",
            "Paros, bloqueos y sabotajes",
            "Paros, bloqueos y sabotajes",
            "Cambios Básica-detalle",
            "Licencia captación y disposición",
            "Retraso prueba inyectividad",
            "Cambios Básica-detalle",
            "Fugas o Fallas sistema tubería",
            "Incertidumbre en calidad materiales",
            "No disponibilidad de materiales en tiempo y calidad (válvulas)",
            "Retrasos en entrega de Construcción",
            "Demoras en Ingenieria de detalle",
            "Mayores costos Ingeniería Básica",
            "Dilución responsabilidad",
            "Paros, Bloqueos y Sabotajes",
            "Afectación sistema eléctrico contro",
            "Stand by de equipos",
            "Ejecutar actividades no planeadas",
            "Afectación reputación Ecopetrol",
            "Retrasos en entrega de Construcción de los clusters",
            "Dificultad en el acceso a las áreas",
            "No contar con separador para produ",
            "Demora a respuesta TQ`s",
            "Entrega tardía tanque",
            "No disponibilidad de materiales",
            "Fallas adm en contrato",
            "Fallas en los Equipos a suministrar",
            "Inicio perforación",
            "No contar con separador para produ",
            "Demoras radicación PS polímero",
            "Hallazgos Arqueológicos",
            "Demoras en Proceso de Selección de proveedor de polímero entrecruzado.",
            "Demora inicio compras",
            "Modif contractual de contratos",
            "Disponibilidad Recursos ejecución",
            "Rediseños de ingeniería",
            "No contar con material perforación",
            "Limitacion de la capacidad",
            "Variación de precios",
            "Retrasos en la ejecución por paros",
            "Cancelación de pozos",
            "No disponibilidad de recursos",
            "Entregables para AR",
            "Paros, bloqueos, sabotajes",
            "Adecuación sistema contraincendio",
            "Modific. Contractuales Opt. Term",
            "No contar con proveedores",
            "Demoras por tramite Arqueológico",
            "Demora autorización uso presupuesto",
            "Modificación de alcance",
            "Demoras por tramite Arqueológico",
            "No disponibilidad líneas de flujo",
            "No alcanzar condiciones pta marcha",
            "Paros, bloqueos y sabotajes",
            "Paros, bloqueos y sabotajes",
            "No alcanzar condiciones pta marcha",
            "Dem. firma acta inicio Cont Termic",
            "Daños a instalaciones operativas",
            "C12 Hallazgo Arqueológico",
            "Daños a instalaciones operativas",
            "EA1 Fallas adm en contrato",
            "C12 Fallas adm en contrato",
            "Retrasos instalación medición",
            "No disponibilidad de materiales",
            "EA1 No disponibilidad de materiales",
            "Retrasos instalación medición",
            "Impactos de la Operación del DEMAG",
            "Interrupción de obra",
            "Interrupción de obra",
            "Hallazgos arqueológicos.",
            "Cambios ingeniería básica",
            "Suscripción de OT de ingeneiría",
            "Hallazgos arqueológicos.",
            "No disponibilidad densitómetro",
            "Incumplimiento de las paradas",
            "No perforar 3 pozos contingentes",
            "Demoras en WDP F2",
            "LI1 No oportunidad en Prognosis"
        };
    }
}